using PortalTCMSP.Domain.Entities.ServicosEntity.CartaServicosUsuarioEntity;
using PortalTCMSP.Domain.Repositories.Home;

namespace PortalTCMSP.Domain.Repositories.Servicos.CartaServicosUsuario
{
    public interface ICartaServicosUsuarioDescritivoItemDetalheRepository : IBaseRepository<CartaServicosUsuarioDescritivoItemDetalhe>
    {
        Task CreateDescritivoItemDetalheAsync(long IdCartaServicosUsuarioItemDetalhe, IEnumerable<CartaServicosUsuarioDescritivoItemDetalhe> novos);
        Task UpdateDescritivoItemDetalheAsync(long IdCartaServicosUsuarioItemDetalhe, IEnumerable<CartaServicosUsuarioDescritivoItemDetalhe> novos);
    }
}
