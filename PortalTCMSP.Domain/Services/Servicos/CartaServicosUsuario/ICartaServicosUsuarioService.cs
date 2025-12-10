using PortalTCMSP.Domain.DTOs.Requests.Servicos.CartaServicosUsuario;
using PortalTCMSP.Domain.DTOs.Requests.Servicos.CartaServicosUsuario.Application.DTO.Servicos.CartaServicosUsuarioDto;
using PortalTCMSP.Domain.DTOs.Responses.Servicos.CartaServicosUsuario.Application.DTO.Servicos.CartaServicosUsuarioDto;

namespace PortalTCMSP.Domain.Services.Servicos.CartaServicosUsuario
{
    public interface ICartaServicosUsuarioService
    {
        Task<CartaServicosUsuarioResponse?> GetSearchAsync(CartaServicosUsuarioDescritivoItemDetalheSearchRequest request);
        Task<IEnumerable<CartaServicosUsuarioResponse>> GetAllAsync();
        Task<CartaServicosUsuarioResponse?> GetByIdAsync(long id);

        Task<CartaServicosUsuarioResponse> CreateAsync(CartaServicosUsuarioRequest request);
        Task<CartaServicosUsuarioResponse?> CreateServicosAsync(long idCartaServicosUsuario, IEnumerable<CartaServicosUsuarioServicoRequest> novos);
        Task<CartaServicosUsuarioResponse?> CreateServicosItensAsync(long idCartaServicosUsuarioServico, IEnumerable<CartaServicosUsuarioServicoItemRequest> novos);
        Task<CartaServicosUsuarioResponse?> CreateServicosItensDetalhesAsync(long idCartaServicosUsuarioServicoItem, IEnumerable<CartaServicosUsuarioItemDetalheRequest> novos);
        Task<CartaServicosUsuarioResponse?> CreateDescritivoItemDetalheAsync(long idCartaServicosUsuarioItemDetalhe, IEnumerable<CartaServicosUsuarioDescritivoItemDetalheRequest> novos);

        Task<bool> UpdateAsync(long id, CartaServicosUsuarioRequest request);

        Task<CartaServicosUsuarioResponse?> UpdateServicosAsync(long idCartaServicosUsuario, IEnumerable<CartaServicosUsuarioServicoUpdate> novos);
        Task<CartaServicosUsuarioResponse?> UpdateServicosItensAsync(long idCartaServicosUsuarioServico, IEnumerable<CartaServicosUsuarioServicoItemUpdate> novos);
        Task<CartaServicosUsuarioResponse?> UpdateServicosItensDetalhesAsync(long idCartaServicosUsuarioServicoItem, IEnumerable<CartaServicosUsuarioItemDetalheUpdate> novos);
        Task<CartaServicosUsuarioResponse?> UpdateDescritivoItemDetalheAsync(long idCartaServicosUsuarioItemDetalhe, IEnumerable<CartaServicosUsuarioDescritivoItemDetalheUpdate> novos);

        Task<bool> DeleteAsync(long id);
        Task<CartaServicosUsuarioResponse?> GetWithChildrenBySlugAtivoAsync(string slug);
        Task<bool> DisableAsync(long id);
    }
}
