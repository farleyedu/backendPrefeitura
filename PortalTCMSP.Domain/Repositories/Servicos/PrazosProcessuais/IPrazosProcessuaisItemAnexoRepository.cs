using PortalTCMSP.Domain.Entities.ServicosEntity.PrazosProcessuaisEntity;
using PortalTCMSP.Domain.Repositories.Home;

namespace PortalTCMSP.Domain.Repositories.Servicos.PrazosProcessuais
{
    public interface IPrazosProcessuaisItemAnexoRepository : IBaseRepository<PrazosProcessuaisItemAnexo>
    {
        Task CreateAnexoAsync(long id, IEnumerable<PrazosProcessuaisItemAnexo> novos);
        Task UpdateAnexoAsync(long id, IEnumerable<PrazosProcessuaisItemAnexo> novos);
    }
}
