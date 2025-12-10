using PortalTCMSP.Domain.Entities.ServicosEntity.MultasProcedimentosEntity;
using PortalTCMSP.Domain.Repositories.Home;

namespace PortalTCMSP.Domain.Repositories.Servicos.MultasProcedimentos
{
    public interface IMultasProcedimentosPortariaRelacionadaRepository : IBaseRepository<MultasProcedimentosPortariaRelacionada>
    {
        Task CreatePortariaRelacionadaAsync(long id, IEnumerable<MultasProcedimentosPortariaRelacionada> novos);
        Task UpdatePortariaRelacionadaAsync(long id, IEnumerable<MultasProcedimentosPortariaRelacionada> novos);
    }
}
