using Microsoft.EntityFrameworkCore;
using PortalTCMSP.Domain.Entities.ServicosEntity.MultasProcedimentosEntity;
using PortalTCMSP.Domain.Repositories.Servicos.MultasProcedimentos;
using PortalTCMSP.Infra.Data.Context;
using PortalTCMSP.Infra.Data.Repositories.Home;

namespace PortalTCMSP.Infra.Data.Repositories.Servicos.MultasProcedimentos
{
    public class MultasProcedimentosProcedimentoRepository(PortalTCMSPContext context)
       : BaseRepository<MultasProcedimentosProcedimento>(admanagerContext: context), IMultasProcedimentosProcedimentoRepository
    {
        public async Task CreateProcedimentosAsync(long id, IEnumerable<MultasProcedimentosProcedimento> novos)
        {
            var entity = await context.MultasProcedimentos
                .Include(mp => mp.Procedimentos)
                .FirstOrDefaultAsync(mp => mp.Id == id) ?? throw new InvalidOperationException($"MultasProcedimentos with Id {id} not found.");
            entity.Procedimentos ??= [];

            foreach (var novo in novos)
                entity.Procedimentos.Add(novo);

            context.MultasProcedimentos.Update(entity);
        }

        public async Task UpdateProcedimentosAsync(long id, IEnumerable<MultasProcedimentosProcedimento> novos)
        {
            var entity = await context.MultasProcedimentos
                .Include(mp => mp.Procedimentos)
                .FirstOrDefaultAsync(mp => mp.Id == id) ?? throw new InvalidOperationException($"MultasProcedimentos with Id {id} not found.");
            entity.Procedimentos ??= [];

            context.MultasProcedimentos.Update(entity);
        }
    }
}
