using PortalTCMSP.Domain.Entities.ServicosEntity.CartaServicosUsuarioEntity;
using PortalTCMSP.Domain.Repositories.Home;

namespace PortalTCMSP.Domain.Repositories.Servicos.CartaServicosUsuario
{
    public interface ICartaServicosUsuarioServicoItemRepository : IBaseRepository<CartaServicosUsuarioServicoItem>
    {
        Task CreateServicosItensAsync(long IdCartaServicosUsuarioServico, IEnumerable<CartaServicosUsuarioServicoItem> novos);
        Task UpdateServicosItensAsync(long IdCartaServicosUsuarioServico, IEnumerable<CartaServicosUsuarioServicoItem> novos);
    }
}
