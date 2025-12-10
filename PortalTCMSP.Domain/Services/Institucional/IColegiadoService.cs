using PortalTCMSP.Domain.DTOs.Requests.Institucional.Colegiado;
using PortalTCMSP.Domain.DTOs.Responses.Institucional.Colegiado;
using PortalTCMSP.Domain.Entities.InstitucionalEntity;

namespace PortalTCMSP.Domain.Services.Institucional
{
    public interface IColegiadoService
    {
        Task<ColegiadoResponse?> GetAsync(long id, CancellationToken ct = default);
        Task<List<ColegiadoResponse>> SearchAsync(string? Ativo, DateTime? publicadoAte, CancellationToken ct = default);
        Task<long> CreateAsync(CreateColegiadoRequest req, CancellationToken ct = default);
        Task UpdateAsync(long id, UpdateColegiadoRequest req, CancellationToken ct = default);
        Task SoftDeleteAsync(long id, CancellationToken ct = default);
        Task<ColegiadoResponse?> GetBySlugAsync(string slug, CancellationToken ct = default);
    }
}
