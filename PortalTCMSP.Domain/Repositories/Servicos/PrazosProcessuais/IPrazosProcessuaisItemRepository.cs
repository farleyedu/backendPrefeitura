using PortalTCMSP.Domain.Entities.ServicosEntity.PrazosProcessuaisEntity;
using PortalTCMSP.Domain.Repositories.Home;

namespace PortalTCMSP.Domain.Repositories.Servicos.PrazosProcessuais
{
    public interface IPrazosProcessuaisItemRepository : IBaseRepository<PrazosProcessuaisItem>
    {
        Task CreatePrazosProcessuaisItemAsync(long id, IEnumerable<PrazosProcessuaisItem> novos);
        Task UpdatePrazosProcessuaisItemAsync(long id, IEnumerable<PrazosProcessuaisItem> novos);
    }
}
