using PortalTCMSP.Domain.Entities.InstitucionalEntity;
using PortalTCMSP.Domain.Repositories.Home;

namespace PortalTCMSP.Domain.Repositories.Institucional
{
    public interface IInstitucionalRepository : IBaseRepository<PortalTCMSP.Domain.Entities.InstitucionalEntity.Institucional>
    {
        Task<PortalTCMSP.Domain.Entities.InstitucionalEntity.Institucional?> GetByIdWithTreeAsync(long id, CancellationToken ct = default);
        Task<List<PortalTCMSP.Domain.Entities.InstitucionalEntity.Institucional>> SearchAsync(string? Ativo, DateTime? publicadoAte, CancellationToken ct = default);
        Task<PortalTCMSP.Domain.Entities.InstitucionalEntity.Institucional?> GetBySlugWithTreeAsync(string slug, CancellationToken ct = default);
    }
}
