using PortalTCMSP.Domain.DTOs.Requests.SessaoPlenaria.Ata;
using PortalTCMSP.Domain.DTOs.Requests.SessaoPlenaria.NotasTaquigraficas;
using PortalTCMSP.Domain.DTOs.Responses.Base;
using PortalTCMSP.Domain.DTOs.Responses.SessaoPlenaria.NotasTaquigraficas;

namespace PortalTCMSP.Domain.Services.SessaoPlenaria
{
    public interface ISessaoPlenariaNotasTaquigraficasService
    {
        Task<IEnumerable<SessaoPlenariaNotasTaquigraficasResponse>> GetAllAsync();
        Task<SessaoPlenariaNotasTaquigraficasResponse?> GetByIdAsync(long id);
        Task<long> CreateAsync(SessaoPlenariaNotasTaquigraficasCreateRequest request);
        Task<bool> UpdateAsync(long id, SessaoPlenariaNotasTaquigraficasUpdateRequest request);
        Task<bool> DeleteAsync(long id);
        Task<ResultadoPaginadoResponse<SessaoPlenariaNotasTaquigraficasResponse>?> GetAllNotas(SessaoPlenariaNotasTaquigraficasSearchRequest request);
    }
}
