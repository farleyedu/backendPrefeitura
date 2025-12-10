using PortalTCMSP.Domain.Entities.ServicosEntity.CartorioEntity;
using PortalTCMSP.Domain.Repositories.Home;

namespace PortalTCMSP.Domain.Repositories.Servicos.Cartorio
{
    public interface ICartorioRepository : IBaseRepository<Entities.ServicosEntity.CartorioEntity.Cartorio>
    {
        Task<Entities.ServicosEntity.CartorioEntity.Cartorio?> GetWithChildrenByIdAsync(long id);
        Task<List<Entities.ServicosEntity.CartorioEntity.Cartorio>> AllWithChildrenAsync();
        Task<bool> DisableAsync(long id);
        Task<Entities.ServicosEntity.CartorioEntity.Cartorio?> GetBySlugAtivoAsync(string slug);
        Task<Entities.ServicosEntity.CartorioEntity.Cartorio?> GetWithChildrenBySlugAtivoAsync(string slug);
        
        Task ReplaceAtendimentosAsync(long id, IEnumerable<CartorioAtendimento> novos);
    }
}
