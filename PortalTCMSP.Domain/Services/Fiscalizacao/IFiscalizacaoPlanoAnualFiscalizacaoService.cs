using PortalTCMSP.Domain.DTOs.Requests.Fiscalizacao.PlanoAnualFiscalizacao;
using PortalTCMSP.Domain.DTOs.Responses.Fiscalizacao.PlanoAnualFiscalizacao;

namespace PortalTCMSP.Domain.Services.Fiscalizacao
{
    public interface IFiscalizacaoPlanoAnualFiscalizacaoService
    {
        Task<IEnumerable<FiscalizacaoPlanoAnualFiscalizacaoResolucaoResponse>> GetAllAsync();
        Task<FiscalizacaoPlanoAnualFiscalizacaoResolucaoResponse?> GetByIdAsync(long id);
        Task<FiscalizacaoPlanoAnualFiscalizacaoResolucaoResponse?> GetBySlugAsync(string slug);
        Task<long> CreateAsync(FiscalizacaoPlanoAnualFiscalizacaoResolucaoCreateRequest request);
        Task<bool> UpdateAsync(long id, FiscalizacaoPlanoAnualFiscalizacaoResolucaoUpdateRequest request);
        Task<bool> DeleteAsync(long id);
    }
}
