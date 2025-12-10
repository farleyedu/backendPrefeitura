using PortalTCMSP.Domain.DTOs.Requests.Institucional.Legislacao;
using PortalTCMSP.Domain.DTOs.Responses.Institucional.Legislacao;

namespace PortalTCMSP.Domain.Services.Institucional
{
    public interface ILegislacaoService
    {
        Task<LegislacaoResponse?> GetAsync(long id, CancellationToken ct = default);
        Task<LegislacaoResponse?> GetBySlugAsync(string slug, CancellationToken ct = default);
        Task<List<LegislacaoResponse>> SearchAsync(string? Ativo, DateTime? publicadoAte, CancellationToken ct = default);
        Task<long> CreateAsync(CreateLegislacaoRequest req, CancellationToken ct = default);
        Task UpdateAsync(long id, UpdateLegislacaoRequest req, CancellationToken ct = default);
        Task SoftDeleteAsync(long id, CancellationToken ct = default);
    }
}
