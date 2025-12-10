using PortalTCMSP.Domain.DTOs.Requests.Institucional.Competencias;
using PortalTCMSP.Domain.DTOs.Responses.Institucional.Competencias;

namespace PortalTCMSP.Domain.Services.Institucional
{
    public interface ICompetenciasService
    {
        Task<CompetenciasResponse?> GetAsync(long id, CancellationToken ct = default);
        Task<CompetenciasResponse?> GetBySlugAsync(string slug, CancellationToken ct = default);
        Task<List<CompetenciasResponse>> SearchAsync(string? Ativo, DateTime? publicadoAte, CancellationToken ct = default);
        Task<long> CreateAsync(CreateCompetenciasRequest req, CancellationToken ct = default);
        Task UpdateAsync(long id, UpdateCompetenciasRequest req, CancellationToken ct = default);
        Task SoftDeleteAsync(long id, CancellationToken ct = default);
    }
}
