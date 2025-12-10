using PortalTCMSP.Domain.DTOs.Requests.SessaoPlenaria.Jurispudencia;
using PortalTCMSP.Domain.DTOs.Responses.SessaoPlenaria.Jurispudencia;

namespace PortalTCMSP.Domain.Services.SessaoPlenaria
{
    public interface ISessaoPlenariaJurispudenciaService
    {
        Task<SessaoPlenariaJurisprudenciaResponse?> GetAtivoAsync();
        Task<IEnumerable<SessaoPlenariaJurisprudenciaResponse>> GetAllAsync();
        Task<SessaoPlenariaJurisprudenciaResponse?> GetByIdAsync(long id);
        Task<SessaoPlenariaJurisprudenciaResponse?> GetBySlugAsync(string slug);
        Task<long> CreateAsync(SessaoPlenariaJurisprudenciaCreateRequest request);
        Task<bool> UpdateAsync(long id, SessaoPlenariaJurisprudenciaUpdateRequest request);
        Task<bool> DeleteAsync(long id);
    }
}
