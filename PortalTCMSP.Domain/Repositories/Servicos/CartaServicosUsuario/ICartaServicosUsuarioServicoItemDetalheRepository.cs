using PortalTCMSP.Domain.Entities.ServicosEntity.CartaServicosUsuarioEntity;
using PortalTCMSP.Domain.Repositories.Home;

namespace PortalTCMSP.Domain.Repositories.Servicos.CartaServicosUsuario
{
    public interface ICartaServicosUsuarioServicoItemDetalheRepository : IBaseRepository<CartaServicosUsuarioItemDetalhe>
    {
        Task CreateServicosItensDetalhesAsync(long IdCartaServicosUsuarioServicoItem, IEnumerable<CartaServicosUsuarioItemDetalhe> novos);
        Task UpdateServicosItensDetalhesAsync(long IdCartaServicosUsuarioServicoItem, IEnumerable<CartaServicosUsuarioItemDetalhe> novos);
    }
}
