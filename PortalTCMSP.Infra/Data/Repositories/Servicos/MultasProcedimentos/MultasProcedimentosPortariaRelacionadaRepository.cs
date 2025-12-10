using Microsoft.EntityFrameworkCore;
using PortalTCMSP.Domain.Entities.ServicosEntity.MultasProcedimentosEntity;
using PortalTCMSP.Domain.Repositories.Servicos.MultasProcedimentos;
using PortalTCMSP.Infra.Data.Context;
using PortalTCMSP.Infra.Data.Repositories.Home;

namespace PortalTCMSP.Infra.Data.Repositories.Servicos.MultasProcedimentos
{
    public class MultasProcedimentosPortariaRelacionadaRepository(PortalTCMSPContext context)
   : BaseRepository<MultasProcedimentosPortariaRelacionada>(admanagerContext: context), IMultasProcedimentosPortariaRelacionadaRepository
    {
        public async Task CreatePortariaRelacionadaAsync(long id, IEnumerable<MultasProcedimentosPortariaRelacionada> novos)
        {
            var entity = await context.MultasProcedimentos
                .Include(mp => mp.PortariasRelacionadas)
                .FirstOrDefaultAsync(mp => mp.Id == id) ?? throw new InvalidOperationException($"PortariaRelacionada with Id {id} not found.");
            entity.PortariasRelacionadas ??= [];

            foreach (var novo in novos)
                entity.PortariasRelacionadas.Add(novo);

            context.MultasProcedimentos.Update(entity);
        }

        public async Task UpdatePortariaRelacionadaAsync(long id, IEnumerable<MultasProcedimentosPortariaRelacionada> novos)
        {
            var entity = await context.MultasProcedimentos
                .Include(mp => mp.PortariasRelacionadas)
                .FirstOrDefaultAsync(mp => mp.Id == id) ?? throw new InvalidOperationException($"MultasProcedimentos with Id {id} not found.");
            entity.PortariasRelacionadas ??= [];

            context.MultasProcedimentos.Update(entity);
        }
    }
}
