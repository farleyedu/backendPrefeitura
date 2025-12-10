using PortalTCMSP.Domain.DTOs.Requests.Fiscalizacao.SecretariaControleExterno;
using PortalTCMSP.Domain.DTOs.Responses.Fiscalizacao.SecretariaControleExterno;

namespace PortalTCMSP.Domain.Services.Fiscalizacao
{
    public interface IFiscalizacaoSecretariaControleExternoSeervice
    {
        Task<IEnumerable<FiscalizacaoSecretariaResponse>> GetAllAsync();
        Task<FiscalizacaoSecretariaResponse?> GetByIdAsync(long id);
        Task<FiscalizacaoSecretariaResponse?> GetBySlugAsync(string slug);
        Task<long> CreateAsync(FiscalizacaoSecretariaCreateRequest request);
        Task<bool> UpdateAsync(long id, FiscalizacaoSecretariaUpdateRequest request);
        Task<bool> DeleteAsync(long id);
    }
}
