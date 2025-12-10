using PortalTCMSP.Domain.DTOs.Requests.Servicos.OficioseIntimacoes;
using PortalTCMSP.Domain.DTOs.Responses.Servicos.OficioseIntimacoes;

namespace PortalTCMSP.Domain.Services.Servicos.OficioseIntimacoes
{
    public interface IOficioseIntimacoesService
    {
        Task<IEnumerable<OficioseIntimacoesResponse>> GetAllAsync();
        Task<OficioseIntimacoesResponse?> GetByIdAsync(long id);
        Task<long> CreateAsync(OficioseIntimacoesCreateRequest request);
        Task<bool> CreateSecoesAsync(long id, List<OficioseIntimacoesSecaoRequest> novos);
        Task<bool> CreateSecaoItensAsync(long idSecao, List<OficioseIntimacoesSecaoItemRequest> novos);
        Task<bool> UpdateAsync(long id, OficioseIntimacoesUpdateRequest request);
        Task<bool> UpdateSecoesAsync(long id, List<OficioseIntimacoesSecaoUpdate> novos);
        Task<bool> UpdateSecaoItensAsync(long idSecao, List<OficioseIntimacoesSecaoItemUpdate> novos);
        Task<bool> DeleteAsync(long id);
        Task<OficioseIntimacoesResponse?> GetWithChildrenBySlugAtivoAsync(string slug);
        Task<bool> DisableAsync(long id);
    }
}
