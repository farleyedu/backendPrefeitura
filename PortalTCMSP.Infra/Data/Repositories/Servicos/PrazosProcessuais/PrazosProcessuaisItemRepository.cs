using Microsoft.EntityFrameworkCore;
using PortalTCMSP.Domain.Entities.ServicosEntity.PrazosProcessuaisEntity;
using PortalTCMSP.Domain.Repositories.Servicos.PrazosProcessuais;
using PortalTCMSP.Infra.Data.Context;
using PortalTCMSP.Infra.Data.Repositories.Home;

namespace PortalTCMSP.Infra.Data.Repositories.Servicos.PrazosProcessuais
{
    public class PrazosProcessuaisItemRepository(PortalTCMSPContext context)
    : BaseRepository<PrazosProcessuaisItem>(context), IPrazosProcessuaisItemRepository
    {
        public async Task CreatePrazosProcessuaisItemAsync(long id, IEnumerable<PrazosProcessuaisItem> novos)
        {
            var entity = await context.PrazosProcessuais
                           .Include(c => c.PrazosProcessuaisItens)
                           .FirstOrDefaultAsync(c => c.Id == id)
                           ?? throw new InvalidOperationException($"PrazosProcessuais with Id {id} not found.");

            foreach (var novo in novos)
                entity.PrazosProcessuaisItens.Add(novo);

            context.PrazosProcessuais.Update(entity);
        }

        public async Task UpdatePrazosProcessuaisItemAsync(long id, IEnumerable<PrazosProcessuaisItem> novos)
        {
            var entity = await context.PrazosProcessuais
                           .Include(c => c.PrazosProcessuaisItens)
                           .FirstOrDefaultAsync(c => c.Id == id)
                           ?? throw new InvalidOperationException($"PrazosProcessuais with Id {id} not found.");

            foreach (var novo in novos)
            {
                var existente = entity.PrazosProcessuaisItens.FirstOrDefault(i => i.Id == novo.Id);
                if (existente != null)
                {
                    entity.PrazosProcessuaisItens.Remove(existente);
                    entity.PrazosProcessuaisItens.Add(novo);
                }
            }

            context.PrazosProcessuais.Update(entity);
        }
    }
}
