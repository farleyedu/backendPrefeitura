using PortalTCMSP.Domain.DTOs.Requests.Servicos.CartaServicosUsuario;
using PortalTCMSP.Domain.Entities.ServicosEntity.CartaServicosUsuarioEntity;
using PortalTCMSP.Domain.Repositories.Home;

namespace PortalTCMSP.Domain.Repositories.Servicos.CartaServicosUsuario
{
    namespace Domain.Interfaces.Servicos.CartaServicosUsuarioInterface
    {
        public interface ICartaServicosUsuarioRepository : IBaseRepository<Entities.ServicosEntity.CartaServicosUsuarioEntity.CartaServicosUsuario>
        {
            Task<Entities.ServicosEntity.CartaServicosUsuarioEntity.CartaServicosUsuario?> Search(CartaServicosUsuarioDescritivoItemDetalheSearchRequest request);
            Task<Entities.ServicosEntity.CartaServicosUsuarioEntity.CartaServicosUsuario?> GetWithChildrenByIdAsync(long id);
            Task<List<Entities.ServicosEntity.CartaServicosUsuarioEntity.CartaServicosUsuario>> AllWithChildrenAsync();
            Task<bool> DisableAsync(long id);
            Task<Entities.ServicosEntity.CartaServicosUsuarioEntity.CartaServicosUsuario?> GetBySlugAtivoAsync(string slug);
            Task<Entities.ServicosEntity.CartaServicosUsuarioEntity.CartaServicosUsuario?> GetWithChildrenBySlugAtivoAsync(string slug);
            
            Task ReplaceServicosAsync(long id, IEnumerable<CartaServicosUsuarioServico> novos);
            Task ReplaceServicosItensAsync(long id, IEnumerable<CartaServicosUsuarioServicoItem> novos);
            Task ReplaceItemDetalheAsync(long id, IEnumerable<CartaServicosUsuarioItemDetalhe> novos);
            Task ReplaceDescritivoItemDetalheAsync(long id, IEnumerable<CartaServicosUsuarioDescritivoItemDetalhe> novos);
            Task<Entities.ServicosEntity.CartaServicosUsuarioEntity.CartaServicosUsuario?> GetByServicoIdWithChildrenAsync(long servicoId);
            Task<Entities.ServicosEntity.CartaServicosUsuarioEntity.CartaServicosUsuario?> GetByServicoItemIdWithChildrenAsync(long servicoItemId);
            Task<Entities.ServicosEntity.CartaServicosUsuarioEntity.CartaServicosUsuario?> GetByItemDetalheIdWithChildrenAsync(long itemDetalheId);
            Task<Entities.ServicosEntity.CartaServicosUsuarioEntity.CartaServicosUsuario?> GetByDescritivoIdWithChildrenAsync(long descritivoId);
        }
    }
}
