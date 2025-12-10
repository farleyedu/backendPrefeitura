using PortalTCMSP.Domain.DTOs.Requests.Servicos.PrazosProcessuais;
using PortalTCMSP.Domain.DTOs.Responses.Servicos.PrazosProcessuais;

namespace PortalTCMSP.Domain.Services.Servicos.PrazosProcessuais
{
    public interface IPrazosProcessuaisService
    {
        Task<IEnumerable<PrazosProcessuaisResponse>> GetAllAsync();
        Task<PrazosProcessuaisResponse?> GetByIdAsync(long id);

        Task<PrazosProcessuaisResponse> CreateAsync(PrazosProcessuaisCreateRequest request);
        Task<bool> CreatePrazosProcessuaisItemAsync(long id, IEnumerable<PrazosProcessuaisItemRequest> novos);
        Task<bool> CreateAnexoAsync(long id, IEnumerable<PrazosProcessuaisItemAnexoRequest> novos);

        Task<bool> UpdateAsync(long id, PrazosProcessuaisCreateRequest request);
        Task<bool> UpdatePrazosProcessuaisItemAsync(long id, IEnumerable<PrazosProcessuaisItemUpdate> novos);
        Task<bool> UpdateAnexoAsync(long id, IEnumerable<PrazosProcessuaisItemAnexoUpdate> novos);

        Task<bool> DeleteAsync(long id);
        Task<PrazosProcessuaisResponse?> GetWithChildrenBySlugAtivoAsync(string slug);
        Task<bool> DisableAsync(long id);
    }
}
