using PortalTCMSP.Domain.DTOs.Responses.Institucional.Base;
using PortalTCMSP.Domain.DTOs.Responses.Institucional.Competencias;

namespace PortalTCMSP.Domain.Services.Institucional
{
    public interface IInstitucionalService
    {
        Task<List<InstitucionalResponse>> SearchAsync(string? Ativo, DateTime? publicadoAte, CancellationToken ct = default);
        Task<CompetenciasResponse?> GetBySlugAsync(string slug, CancellationToken ct = default);
    }
}
