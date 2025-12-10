using PortalTCMSP.Domain.DTOs.Requests.Institucional.Historia;
using PortalTCMSP.Domain.DTOs.Requests.Institucional.Historia.PortalTCMSP.Domain.DTOs.Requests.Institucional.Historia;
using PortalTCMSP.Domain.DTOs.Responses.Institucional.Historia;

namespace PortalTCMSP.Domain.Services.Institucional
{
    public interface IHistoriaService
    {
        Task<HistoriaResponse?> GetAsync(long id, CancellationToken ct = default);
        Task<HistoriaResponse?> GetBySlugAsync(string slug, CancellationToken ct = default);
        Task<List<HistoriaResponse>> SearchAsync(string? Ativo, DateTime? publicadoAte, CancellationToken ct = default);
        Task<long> CreateAsync(CreateHistoriaRequest req, CancellationToken ct = default);
        Task UpdateAsync(long id, UpdateHistoriaRequest req, CancellationToken ct = default);
        Task SoftDeleteAsync(long id, CancellationToken ct = default);
    }
}
