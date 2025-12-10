using PortalTCMSP.Domain.DTOs.Requests.Fiscalizacao.ContasDoPrefeito;
using PortalTCMSP.Domain.DTOs.Responses.Base;
using PortalTCMSP.Domain.DTOs.Responses.Fiscalizacao.ContasDoPrefeito;

namespace PortalTCMSP.Domain.Services.Fiscalizacao
{
    public interface IFiscalizacaoContasDoPrefeitoService
    {
        Task<FiscalizacaoContasDoPrefeitoResponse?> GetByIdAsync(long id);
        Task<long> CreateAsync(FiscalizacaoContasDoPrefeitoCreateRequest request);
        Task<bool> UpdateAsync(long id, FiscalizacaoContasDoPrefeitoUpdateRequest request);
        Task<bool> DeleteAsync(long id);
        Task<ResultadoPaginadoResponse<FiscalizacaoContasDoPrefeitoResponse>?> GetListAsync(FiscalizacaoContasDoPrefeitoSearchRequest request);
    }
}
