using Microsoft.EntityFrameworkCore;
using PortalTCMSP.Domain.Entities.ServicosEntity.PrazosProcessuaisEntity;
using PortalTCMSP.Domain.Repositories.Servicos.PrazosProcessuais;
using PortalTCMSP.Infra.Data.Context;
using PortalTCMSP.Infra.Data.Repositories.Home;

namespace PortalTCMSP.Infra.Data.Repositories.Servicos.PrazosProcessuais
{
    public class PrazosProcessuaisItemAnexoRepository(PortalTCMSPContext context)
    : BaseRepository<PrazosProcessuaisItemAnexo>(context), IPrazosProcessuaisItemAnexoRepository
    {
        public async Task CreateAnexoAsync(long id, IEnumerable<PrazosProcessuaisItemAnexo> novos)
        {
            var entity = await context.PrazosProcessuaisItem
                           .Include(c => c.Anexos)
                           .FirstOrDefaultAsync(c => c.Id == id)
                           ?? throw new InvalidOperationException($"PrazosProcessuaisItem with Id {id} not found.");

            foreach (var novo in novos)
                entity.Anexos.Add(novo);

            context.PrazosProcessuaisItem.Update(entity);
        }

        public async Task UpdateAnexoAsync(long id, IEnumerable<PrazosProcessuaisItemAnexo> novos)
        {
            var entity = await context.PrazosProcessuaisItem
                           .Include(c => c.Anexos)
                           .FirstOrDefaultAsync(c => c.Id == id)
                           ?? throw new InvalidOperationException($"PrazosProcessuaisItem with Id {id} not found.");

            var anexos = entity.Anexos.ToList();
            foreach (var novo in novos)
            {
                var existente = anexos.FirstOrDefault(i => i.Id == novo.Id);
                if (existente != null)
                {
                    entity.Anexos.Remove(existente);
                    entity.Anexos.Add(novo);
                }
            }

            context.PrazosProcessuaisItem.Update(entity);
        }
    }
}
