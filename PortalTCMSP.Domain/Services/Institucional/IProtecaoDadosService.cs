using PortalTCMSP.Domain.DTOs.Requests.Institucional.ProtecaoDados;
using PortalTCMSP.Domain.DTOs.Responses.Institucional.ProtecaoDados;

namespace PortalTCMSP.Domain.Services.Institucional
{
    public interface IProtecaoDadosService
    {
        Task<ProtecaoDadosResponse?> GetAsync(long id, CancellationToken ct = default);
        Task<ProtecaoDadosResponse?> GetBySlugAsync(string slug, CancellationToken ct = default);
        Task<List<ProtecaoDadosResponse>> SearchAsync(string? Ativo, DateTime? publicadoAte, CancellationToken ct = default);
        Task<long> CreateAsync(CreateProtecaoDadosRequest req, CancellationToken ct = default);
        Task UpdateAsync(long id, UpdateProtecaoDadosRequest req, CancellationToken ct = default);
        Task SoftDeleteAsync(long id, CancellationToken ct = default);
    }
}
