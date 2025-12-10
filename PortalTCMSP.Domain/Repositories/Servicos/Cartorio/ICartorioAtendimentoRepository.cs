using PortalTCMSP.Domain.Entities.ServicosEntity.CartorioEntity;
using PortalTCMSP.Domain.Repositories.Home;

namespace PortalTCMSP.Domain.Repositories.Servicos.Cartorio
{
    public interface ICartorioAtendimentoRepository : IBaseRepository<CartorioAtendimento>
    {
        Task CreateAtendimentosAsync(long id, IEnumerable<CartorioAtendimento> novos);
        Task UpdateAtendimentosAsync(long id, IEnumerable<CartorioAtendimento> novos);
    }
}
