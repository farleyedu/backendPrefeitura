using PortalTCMSP.Domain.DTOs.Requests.Institucional.Relatorias;
using PortalTCMSP.Domain.DTOs.Responses.Institucional.Relatorias;

namespace PortalTCMSP.Domain.Services.Institucional
{
    public interface IRelatoriasService
    {
        Task<RelatoriasResponse?> GetAsync(long id, CancellationToken ct = default);
        Task<RelatoriasResponse?> GetBySlugAsync(string slug, CancellationToken ct = default);
        Task<List<RelatoriasResponse>> SearchAsync(string? Ativo, DateTime? publicadoAte, CancellationToken ct = default);
        Task<long> CreateAsync(CreateRelatoriasRequest req, CancellationToken ct = default);
        Task UpdateAsync(long id, UpdateRelatoriasRequest req, CancellationToken ct = default);
        Task SoftDeleteAsync(long id, CancellationToken ct = default);
        Task<bool> HardDeleteAsync(long id, CancellationToken ct = default);
    }
}
