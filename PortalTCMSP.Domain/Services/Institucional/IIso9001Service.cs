using PortalTCMSP.Domain.DTOs.Requests.Institucional.Iso9001;
using PortalTCMSP.Domain.DTOs.Responses.Institucional.Iso9001;

namespace PortalTCMSP.Domain.Services.Institucional
{
    public interface IIso9001Service
    {
        Task<Iso9001Response?> GetAsync(long id, CancellationToken ct = default);
        Task<Iso9001Response?> GetBySlugAsync(string slug, CancellationToken ct = default);
        Task<List<Iso9001Response>> SearchAsync(string? Ativo, DateTime? publicadoAte, CancellationToken ct = default);
        Task<long> CreateAsync(CreateIso9001Request req, CancellationToken ct = default);
        Task UpdateAsync(long id, UpdateIso9001Request req, CancellationToken ct = default);
        Task SoftDeleteAsync(long id, CancellationToken ct = default);
    }
}
