using Microsoft.EntityFrameworkCore;
using PortalTCMSP.Domain.Entities.ServicosEntity.MultasProcedimentosEntity;
using PortalTCMSP.Domain.Repositories.Servicos.MultasProcedimentos;
using PortalTCMSP.Infra.Data.Context;
using PortalTCMSP.Infra.Data.Repositories.Home;

namespace PortalTCMSP.Infra.Data.Repositories.Servicos.MultasProcedimentos
{
    public class MultasProcedimentosRepository(PortalTCMSPContext context)
        : BaseRepository<Domain.Entities.ServicosEntity.MultasProcedimentosEntity.MultasProcedimentos>(context), IMultasProcedimentosRepository
    {
        public Task<Domain.Entities.ServicosEntity.MultasProcedimentosEntity.MultasProcedimentos?> GetWithChildrenByIdAsync(long id)
            => Set.Include(x => x.Procedimentos)
                  .Include(x => x.PortariasRelacionadas)
                  .FirstOrDefaultAsync(x => x.Id == id);

        public Task<List<Domain.Entities.ServicosEntity.MultasProcedimentosEntity.MultasProcedimentos>> AllWithChildrenAsync()
            => Set.Include(x => x.Procedimentos)
                  .Include(x => x.PortariasRelacionadas)
                  .ToListAsync();

        public async Task<bool> DisableAsync(long id)
        {
            var set = Context.Set<MultasProcedimentosPortariaRelacionada>();
            var entity = await context.MultasProcedimentos.FindAsync(id);
            if (entity is null)
                return false;

            entity.Ativo = false;
            context.MultasProcedimentos.Update(entity);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<Domain.Entities.ServicosEntity.MultasProcedimentosEntity.MultasProcedimentos?> GetBySlugAtivoAsync(string slug)
        {
            var set = Context.Set<Domain.Entities.ServicosEntity.MultasProcedimentosEntity.MultasProcedimentos>();
            return await context.MultasProcedimentos.AsNoTracking().FirstOrDefaultAsync(mp => mp.Slug == slug && mp.Ativo);
        }

        public async Task<Domain.Entities.ServicosEntity.MultasProcedimentosEntity.MultasProcedimentos?> GetWithChildrenBySlugAtivoAsync(string slug)
        {
            return await context.MultasProcedimentos
                .Include(mp => mp.Procedimentos)
                .Include(mp => mp.PortariasRelacionadas)
                .Where(mp => mp.Slug == slug && mp.Ativo)
                .FirstOrDefaultAsync();
        }

        public async Task ReplaceProcedimentosAsync(long id, IEnumerable<MultasProcedimentosProcedimento> novos)
        {
            var set = Context.Set<MultasProcedimentosProcedimento>();
            var antigos = await set.Where(t => t.IdMultasProcedimentos == id).ToListAsync();
            if (antigos.Count > 0) set.RemoveRange(antigos);
            foreach (var n in novos) n.IdMultasProcedimentos = id;
            await set.AddRangeAsync(novos);
        }


        public async Task ReplacePortariasAsync(long id, IEnumerable<MultasProcedimentosPortariaRelacionada> novas)
        {
            var set = Context.Set<MultasProcedimentosPortariaRelacionada>();
            var antigas = await set.Where(t => t.IdMultasProcedimentos == id).ToListAsync();
            if (antigas.Count > 0) set.RemoveRange(antigas);
            foreach (var n in novas) n.IdMultasProcedimentos = id;
            await set.AddRangeAsync(novas);
        }
    }
}
