using PortalTCMSP.Domain.Entities.ServicosEntity.MultasProcedimentosEntity;
using PortalTCMSP.Domain.Repositories.Home;

namespace PortalTCMSP.Domain.Repositories.Servicos.MultasProcedimentos
{
    public interface IMultasProcedimentosProcedimentoRepository : IBaseRepository<MultasProcedimentosProcedimento>
    {
        Task CreateProcedimentosAsync(long id, IEnumerable<MultasProcedimentosProcedimento> novos);
        Task UpdateProcedimentosAsync(long id, IEnumerable<MultasProcedimentosProcedimento> novos);
    }
}
