using PortalTCMSP.Domain.DTOs.Requests.Institucional.Organograma;
using PortalTCMSP.Domain.DTOs.Responses.Institucional.Organograma;

namespace PortalTCMSP.Domain.Services.Home
{
    public interface IOrganogramaService
    {
        Task<OrganogramaResponse?> GetAsync(long id, CancellationToken ct = default);
        Task<OrganogramaResponse?> GetBySlugAsync(string slug, CancellationToken ct = default);
        Task<List<OrganogramaResponse>> SearchAsync(string? Ativo, DateTime? publicadoAte, CancellationToken ct = default);
        Task<long> CreateAsync(CreateOrganogramaRequest req, CancellationToken ct = default);
        Task UpdateAsync(long id, UpdateOrganogramaRequest req, CancellationToken ct = default);
        Task SoftDeleteAsync(long id, CancellationToken ct = default);
    }
}
