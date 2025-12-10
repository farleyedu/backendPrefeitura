using PortalTCMSP.Domain.Entities.ServicosEntity.PrazosProcessuaisEntity;
using PortalTCMSP.Domain.Repositories.Home;

namespace PortalTCMSP.Domain.Repositories.Servicos.PrazosProcessuais
{
    public interface IPrazosProcessuaisRepository : IBaseRepository<Entities.ServicosEntity.PrazosProcessuaisEntity.PrazosProcessuais>
    {
        Task<Entities.ServicosEntity.PrazosProcessuaisEntity.PrazosProcessuais?> GetWithChildrenByIdAsync(long id);
        Task<List<Entities.ServicosEntity.PrazosProcessuaisEntity.PrazosProcessuais>> AllWithChildrenAsync();
        Task<bool> DisableAsync(long id);
        Task<Entities.ServicosEntity.PrazosProcessuaisEntity.PrazosProcessuais?> GetBySlugAtivoAsync(string slug);
        Task<Entities.ServicosEntity.PrazosProcessuaisEntity.PrazosProcessuais?> GetWithChildrenBySlugAtivoAsync(string slug);

        Task ReplacePrazosProcessuaisItensAsync(long id, IEnumerable<PrazosProcessuaisItem> novos);
    }
}
