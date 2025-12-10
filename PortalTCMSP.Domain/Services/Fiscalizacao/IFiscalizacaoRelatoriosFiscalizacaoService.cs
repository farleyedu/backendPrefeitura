using PortalTCMSP.Domain.DTOs.Requests.Fiscalizacao.RelatorioFiscalizacao;
using PortalTCMSP.Domain.DTOs.Responses.Fiscalizacao.RelatorioFiscalizacao;

namespace PortalTCMSP.Domain.Services.Fiscalizacao
{
    public interface IFiscalizacaoRelatoriosFiscalizacaoService
    {
        Task<IEnumerable<FiscalizacaoRelatorioFiscalizacaoResponse>> GetAllAsync();
        Task<FiscalizacaoRelatorioFiscalizacaoResponse?> GetByIdAsync(long id);
        Task<FiscalizacaoRelatorioFiscalizacaoResponse?> GetBySlugAsync(string slug);
        Task<long> CreateAsync(FiscalizacaoRelatorioFiscalizacaoCreateRequest request);
        Task<bool> UpdateAsync(long id, FiscalizacaoRelatorioFiscalizacaoConteudoUpdateRequest request);
        Task<bool> DeleteAsync(long id);
    }
}
