using Microsoft.EntityFrameworkCore;
using NPOI.SS.Formula.Functions;
using PortalTCMSP.Domain.Entities.ServicosEntity.CartaServicosUsuarioEntity;
using PortalTCMSP.Domain.Entities.ServicosEntity.PrazosProcessuaisEntity;
using PortalTCMSP.Domain.Repositories.Servicos.PrazosProcessuais;
using PortalTCMSP.Infra.Data.Context;
using PortalTCMSP.Infra.Data.Repositories.Home;

namespace PortalTCMSP.Infra.Data.Repositories.Servicos.PrazosProcessuais
{
    public class PrazosProcessuaisRepository(PortalTCMSPContext context)
        : BaseRepository<Domain.Entities.ServicosEntity.PrazosProcessuaisEntity.PrazosProcessuais>(context), IPrazosProcessuaisRepository
    {
        public async Task<Domain.Entities.ServicosEntity.PrazosProcessuaisEntity.PrazosProcessuais?> GetWithChildrenByIdAsync(long id)
        {
            return await context.PrazosProcessuais
                .Include(p => p.PrazosProcessuaisItens)
                    .ThenInclude(i => i.Anexos)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<Domain.Entities.ServicosEntity.PrazosProcessuaisEntity.PrazosProcessuais>> AllWithChildrenAsync()
        {
            return await context.PrazosProcessuais
                .Include(p => p.PrazosProcessuaisItens)
                    .ThenInclude(i => i.Anexos)
                .AsNoTracking()
                .OrderByDescending(p => p.DataCriacao)
                .ToListAsync();
        }

        public async Task<bool> DisableAsync(long id)
        {
            var entity = await context.PrazosProcessuais.FindAsync(id);
            if (entity is null) return false;

            entity.Ativo = false;
            context.PrazosProcessuais.Update(entity);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<Domain.Entities.ServicosEntity.PrazosProcessuaisEntity.PrazosProcessuais?> GetBySlugAtivoAsync(string slug)
             => await context.PrazosProcessuais.AsNoTracking().FirstOrDefaultAsync(c => c.Slug == slug && c.Ativo);

        public async Task<Domain.Entities.ServicosEntity.PrazosProcessuaisEntity.PrazosProcessuais?> GetWithChildrenBySlugAtivoAsync(string slug)
        {
            return await context.PrazosProcessuais
                .Include(p => p.PrazosProcessuaisItens.Where(i => i.Ativo))
                    .ThenInclude(i => i.Anexos.Where(a => a.Ativo))
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Slug == slug && p.Ativo);
        }

        public async Task ReplacePrazosProcessuaisItensAsync(long id, IEnumerable<PrazosProcessuaisItem> novos)
        {
            var set = context.Set<PrazosProcessuaisItem>();
            var antigas = await set
                .Where(x => x.IdPrazosProcessuais == id)
                .ToListAsync();

            if (antigas.Count > 0)
                set.RemoveRange(antigas);

            foreach (var n in novos)
                n.IdPrazosProcessuais = id;

            await set.AddRangeAsync(novos);
        }
    }
}
