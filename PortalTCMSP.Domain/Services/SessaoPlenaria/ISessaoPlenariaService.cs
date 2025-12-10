using PortalTCMSP.Domain.DTOs.Requests.SessaoPlenaria.Base;
using PortalTCMSP.Domain.DTOs.Responses.SessaoPlenaria.Base;

namespace PortalTCMSP.Domain.Services.SessaoPlenaria
{
    public interface ISessaoPlenariaService
    {
        Task<IEnumerable<SessaoPlenariaResponse>> GetAllAsync();
        Task<SessaoPlenariaResponse?> GetByIdAsync(long id);
        Task<SessaoPlenariaResponse?> GetBySlugAsync(string slug);
        Task<long> CreateAsync(SessaoPlenariaCreateRequest request);
        Task<bool> UpdateAsync(long id, SessaoPlenariaUpdateRequest request);
        Task<bool> DeleteAsync(long id);
        Task<IEnumerable<SessaoPlenariaResponse>> GetAtivosAsync();
        Task<SessaoPlenariaResponse?> GetAtivaAsync();
    }
}
