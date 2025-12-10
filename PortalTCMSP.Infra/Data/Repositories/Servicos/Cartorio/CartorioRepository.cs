using Microsoft.EntityFrameworkCore;
using PortalTCMSP.Domain.Entities.ServicosEntity.CartorioEntity;
using PortalTCMSP.Domain.Repositories.Servicos.Cartorio;
using PortalTCMSP.Infra.Data.Context;
using PortalTCMSP.Infra.Data.Repositories.Home;

namespace PortalTCMSP.Infra.Data.Repositories.Servicos.Cartorio
{
    public class CartorioRepository(PortalTCMSPContext context)
        : BaseRepository<Domain.Entities.ServicosEntity.CartorioEntity.Cartorio>(context), ICartorioRepository
    {
        public Task<Domain.Entities.ServicosEntity.CartorioEntity.Cartorio?> GetWithChildrenByIdAsync(long id)
            => Set.Include(x => x.Atendimentos)
                  .FirstOrDefaultAsync(x => x.Id == id);

        public Task<List<Domain.Entities.ServicosEntity.CartorioEntity.Cartorio>> AllWithChildrenAsync()
            => Set.Include(x => x.Atendimentos).ToListAsync();

        public async Task<bool> DisableAsync(long id)
        {
            var entity = await context.Cartorio.FindAsync(id);
            if (entity is null) return false;

            entity.Ativo = false;
            context.Cartorio.Update(entity);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<Domain.Entities.ServicosEntity.CartorioEntity.Cartorio?> GetWithChildrenBySlugAtivoAsync(string slug)
            => await context.Cartorio
                .Include(c => c.Atendimentos)
                .FirstOrDefaultAsync(c => c.Slug == slug && c.Ativo);

        public async Task<Domain.Entities.ServicosEntity.CartorioEntity.Cartorio?> GetBySlugAtivoAsync(string slug)
            => await context.Cartorio.AsNoTracking().FirstOrDefaultAsync(c => c.Slug == slug && c.Ativo);

        public async Task ReplaceAtendimentosAsync(long id, IEnumerable<CartorioAtendimento> novos)
        {
            var set = Context.Set<CartorioAtendimento>();
            var antigos = await set.Where(a => a.IdCartorio == id).ToListAsync();
            if (antigos.Count > 0) set.RemoveRange(antigos);
            foreach (var n in novos) n.IdCartorio = id;
            await set.AddRangeAsync(novos);
        }
    }
}
