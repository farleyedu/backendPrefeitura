using PortalTCMSP.Domain.DTOs.Requests.SessaoPlenaria.Ata;
using PortalTCMSP.Domain.DTOs.Responses.Base;
using PortalTCMSP.Domain.DTOs.Responses.SessaoPlenaria.Ata;

namespace PortalTCMSP.Domain.Services.SessaoPlenaria
{
    public interface ISessaoPlenariaAtaService
    {
        Task<IEnumerable<SessaoPlenariaAtaResponse>> GetAllAsync();
        Task<SessaoPlenariaAtaResponse?> GetByIdAsync(long id);
        Task<long> CreateAsync(SessaoPlenariaAtaCreateRequest request);
        Task<bool> UpdateAsync(long id, SessaoPlenariaAtaUpdateRequest request);
        Task<bool> DeleteAsync(long id);
        Task<ResultadoPaginadoResponse<SessaoPlenariaAtaResponse>?> GetAllAtas(SessaoPlenariaAtaSearchRequest request);
    }
}
