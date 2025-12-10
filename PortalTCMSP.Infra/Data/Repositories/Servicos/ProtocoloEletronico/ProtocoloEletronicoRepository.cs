using Microsoft.EntityFrameworkCore;
using PortalTCMSP.Domain.Entities.ServicosEntity.ProtocoloEletronicoEntity;
using PortalTCMSP.Domain.Repositories.Servicos.ProtocoloEletronico;
using PortalTCMSP.Infra.Data.Context;
using PortalTCMSP.Infra.Data.Repositories.Home;

namespace PortalTCMSP.Infra.Data.Repositories.Servicos.ProtocoloEletronico
{
    public class ProtocoloEletronicoRepository(PortalTCMSPContext context)
        : BaseRepository<Domain.Entities.ServicosEntity.ProtocoloEletronicoEntity.ProtocoloEletronico>(context), IProtocoloEletronicoRepository
    {
        public Task<Domain.Entities.ServicosEntity.ProtocoloEletronicoEntity.ProtocoloEletronico?> GetWithChildrenByIdAsync(long id)
            => Set.Include(x => x.Acoes).FirstOrDefaultAsync(x => x.Id == id);

        public Task<List<Domain.Entities.ServicosEntity.ProtocoloEletronicoEntity.ProtocoloEletronico>> AllWithChildrenAsync()
            => Set.Include(x => x.Acoes).ToListAsync();

        public async Task<bool> DisableAsync(long id)
        {
            var entity = await context.ProtocoloEletronico.FindAsync(id);
            if (entity is null) return false;
            entity.Ativo = false;
            context.ProtocoloEletronico.Update(entity);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<Domain.Entities.ServicosEntity.ProtocoloEletronicoEntity.ProtocoloEletronico?> GetWithChildrenBySlugAtivoAsync(string slug)
            => await context.ProtocoloEletronico
                .Include(p => p.Acoes)
                .FirstOrDefaultAsync(p => p.Slug == slug && p.Ativo);

        public async Task ReplaceAcoesAsync(long id, IEnumerable<ProtocoloEletronicoAcao> novas)
        {
            var set = Context.Set<ProtocoloEletronicoAcao>();
            var antigas = await set.Where(a => a.IdProtocoloEletronico == id).ToListAsync();
            if (antigas.Count > 0) set.RemoveRange(antigas);
            foreach (var n in novas) n.IdProtocoloEletronico = id;
            await set.AddRangeAsync(novas);
        }

        public async Task<Domain.Entities.ServicosEntity.ProtocoloEletronicoEntity.ProtocoloEletronico?> GetBySlugAtivoAsync(string slug)
        {
            return await context.ProtocoloEletronico
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.Slug == slug && o.Ativo);
        }
    }

    

}
