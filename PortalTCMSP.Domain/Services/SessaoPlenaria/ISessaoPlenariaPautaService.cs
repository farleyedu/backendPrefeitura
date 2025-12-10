using PortalTCMSP.Domain.DTOs.Requests.SessaoPlenaria.Pauta;
using PortalTCMSP.Domain.DTOs.Responses.Base;
using PortalTCMSP.Domain.DTOs.Responses.SessaoPlenaria.Pauta;

namespace PortalTCMSP.Domain.Services.SessaoPlenaria
{
    public interface ISessaoPlenariaPautaService
    {
        Task<SessaoPlenariaPautaResponse?> GetByIdAsync(long id);
        Task<long> CreateAsync(SessaoPlenariaPautaCreateRequest request);
        Task<bool> UpdateAsync(long id, SessaoPlenariaPautaUpdateRequest request);
        Task<bool> DeleteAsync(long id);
        Task<ResultadoPaginadoResponse<SessaoPlenariaPautaResponse>?> GetAllPautas(SessaoPlenariaPautaSearchRequest request);
    }
}
