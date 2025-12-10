using PortalTCMSP.Domain.DTOs.Requests.Categoria;
using PortalTCMSP.Domain.DTOs.Responses.CategoriaResponse;

namespace PortalTCMSP.Domain.Services.Home
{
    public interface ICategoriaService
    {
        Task<IEnumerable<CategoriaResponse>> ListarAsync();
        Task<CategoriaResponse?> ObterPorIdAsync(int id);
        Task<int> CriarAsync(CategoriaCreateRequest request);
        Task<bool> AtualizarAsync(CategoriaUpdateRequest request);
        Task<bool> DeletarAsync(int id);
    }
}
