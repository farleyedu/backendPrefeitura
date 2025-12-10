using PortalTCMSP.Domain.DTOs.Requests.Noticia;
using PortalTCMSP.Domain.DTOs.Requests.NoticiaRequest;
using PortalTCMSP.Domain.DTOs.Responses.Base;
using PortalTCMSP.Domain.DTOs.Responses.Noticia;

namespace PortalTCMSP.Domain.Services.Noticia
{
    public interface INoticiaService
    {
        ResultadoPaginadoResponse<NoticiaResponse> ListarNoticias(NoticiaListarRequest request);
        Task<NoticiaCompletaResponse?> ObterNoticiaPorIdAsync(int id);
        Task<NoticiaCompletaResponse?> ObterNoticiaPorSlugAsync(string slug);
        Task<string> AdicionarAsync(NoticiaCreateRequest request);
        Task<bool> AtualizarAsync(NoticiaUpdateRequest request);
        Task<bool> PatchAsync(long id, NoticiaPatchRequest request);
        Task<bool> DeletarAsync(int id);
    }
}
