using PortalTCMSP.Domain.DTOs.Requests.Institucional.MissaoVisaoValores;
using PortalTCMSP.Domain.DTOs.Responses.Institucional.MissaoVisaoValores;

namespace PortalTCMSP.Domain.Services.Institucional
{
    public interface IMissaoVisaoValoresService
    {
        Task<MissaoVisaoValoresResponse?> GetAsync(long id, CancellationToken ct = default);
        Task<MissaoVisaoValoresResponse?> GetBySlugAsync(string slug, CancellationToken ct = default);
        Task<List<MissaoVisaoValoresResponse>> SearchAsync(string? Ativo, DateTime? publicadoAte, CancellationToken ct = default);
        Task<long> CreateAsync(CreateMissaoVisaoValoresRequest req, CancellationToken ct = default);
        Task UpdateAsync(long id, UpdateMissaoVisaoValoresRequest req, CancellationToken ct = default);
        Task SoftDeleteAsync(long id, CancellationToken ct = default);
    }
}
