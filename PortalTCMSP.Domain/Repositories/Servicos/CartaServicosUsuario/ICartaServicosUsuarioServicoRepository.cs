using PortalTCMSP.Domain.Entities.ServicosEntity.CartaServicosUsuarioEntity;
using PortalTCMSP.Domain.Repositories.Home;

namespace PortalTCMSP.Domain.Repositories.Servicos.CartaServicosUsuario
{
    public interface ICartaServicosUsuarioServicoRepository : IBaseRepository<CartaServicosUsuarioServico>
    {
        Task CreateServicosAsync(long id, IEnumerable<CartaServicosUsuarioServico> novos);
        Task UpdateServicosAsync(long id, IEnumerable<CartaServicosUsuarioServico> novos);
    }
}
