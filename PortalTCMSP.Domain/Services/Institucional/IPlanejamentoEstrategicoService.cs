using PortalTCMSP.Domain.DTOs.Requests.Institucional.PlanejamentoEstrategico;
using PortalTCMSP.Domain.DTOs.Responses.Institucional.PlanejamentoEstrategico;

namespace PortalTCMSP.Domain.Services.Institucional
{
    public interface IPlanejamentoEstrategicoService
    {
        Task<PlanejamentoEstrategicoResponse?> GetAsync(long id, CancellationToken ct = default);
        Task<PlanejamentoEstrategicoResponse?> GetBySlugAsync(string slug, CancellationToken ct = default);
        Task<List<PlanejamentoEstrategicoResponse>> SearchAsync(string? Ativo, DateTime? publicadoAte, CancellationToken ct = default);
        Task<long> CreateAsync(CreatePlanejamentoEstrategicoRequest req, CancellationToken ct = default);
        Task UpdateAsync(long id, UpdatePlanejamentoEstrategicoRequest req, CancellationToken ct = default);
        Task SoftDeleteAsync(long id, CancellationToken ct = default);
    }
}
