using PortalTCMSP.Domain.DTOs.Requests.Noticia;
using PortalTCMSP.Domain.DTOs.Responses.Base;
using PortalTCMSP.Domain.DTOs.Responses.Noticia;

namespace PortalTCMSP.Domain.Services.Noticia
{
    public interface INoticiaOldService
    {
        ResultadoPaginadoResponse<NoticiaOldResponse> ListarNoticiasOld(NoticiaOldListarRequest request);
        Task<ResultadoPaginadoResponse<NoticiaOldMappedResponse>> ListarNoticiasOldMapAsync(NoticiaOldListarRequest request);
        Task<NoticiaOldResponse?> GetByIdAsync(long id, CancellationToken ct = default);
        Task<NoticiaOldMappedResponse?> GetByIdMapAsync(long id, CancellationToken ct = default);
        Task<NoticiaOldResponse?> GetBySlugAsync(string slug, CancellationToken ct = default);
        Task<NoticiaOldMappedResponse?> GetBySlugMapAsync(string slug, CancellationToken ct = default);
    }
}
