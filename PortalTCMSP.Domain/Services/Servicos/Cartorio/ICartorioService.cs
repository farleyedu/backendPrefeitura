using PortalTCMSP.Domain.DTOs.Requests.Servicos.Cartorio;
using PortalTCMSP.Domain.DTOs.Responses.Servicos.Cartorio;

namespace PortalTCMSP.Domain.Services.Servicos.Cartorio
{
    public interface ICartorioService
    {
        Task<IEnumerable<CartorioResponse>> GetAllAsync();
        Task<CartorioResponse?> GetByIdAsync(long id);
        Task<long> CreateAsync(CartorioCreateRequest request);
        Task<bool> CreateAtendimentosAsync(long id, List<CartorioAtendimentoRequest> novos);
        Task<bool> UpdateAsync(long id, CartorioUpdateRequest request);
        Task<bool> UpdateAtendimentosAsync(long id, List<CartorioAtendimentoUpdate> novos);
        Task<bool> DeleteAsync(long id);
        Task<CartorioResponse?> GetWithChildrenBySlugAtivoAsync(string slug);
        Task<bool> DisableAsync(long id);
    }

}
