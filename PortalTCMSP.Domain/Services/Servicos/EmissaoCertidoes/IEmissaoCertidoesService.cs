using PortalTCMSP.Domain.DTOs.Requests.Servicos.EmissaoCertidoes;
using PortalTCMSP.Domain.DTOs.Responses.Servicos.EmissaoCertidoes;

namespace PortalTCMSP.Domain.Services.Servicos.EmissaoCertidoes
{
    public interface IEmissaoCertidoesService
    {
        Task<IEnumerable<EmissaoCertidoesResponse>> GetAllAsync();
        Task<EmissaoCertidoesResponse?> GetByIdAsync(long id);
        Task<long> CreateAsync(EmissaoCertidoesCreateRequest request);
        Task<bool> CreateAcoesAsync(long id, List<EmissaoCertidoesAcaoRequest> novos);
        Task<bool> CreateSecoesAsync(long id, List<EmissaoCertidoesSecaoOrientacaoRequest> novos);
        Task<bool> UpdateAsync(long id, EmissaoCertidoesUpdateRequest request);
        Task<bool> UpdateAcoesAsync(long id, List<EmissaoCertidoesAcaoUpdate> novos);
        Task<bool> UpdateSecoesAsync(long id, List<EmissaoCertidoesSecaoOrientacaoUpdate> novos);
        Task<bool> DeleteAsync(long id);
        Task<EmissaoCertidoesResponse?> GetWithChildrenBySlugAtivoAsync(string slug);
        Task<bool> DisableAsync(long id);
    }
}
