using PortalTCMSP.Domain.DTOs.Requests.SessaoPlenaria.SustentacaoOral;
using PortalTCMSP.Domain.DTOs.Responses.SessaoPlenaria.SustentacaoOral;

namespace PortalTCMSP.Domain.Services.SessaoPlenaria
{
    public interface ISessaoPlenariaSustentacaoOralService
    {
        Task<IEnumerable<SessaoPlenariaSustentacaoOralResponse>> GetAllAsync();
        Task<SessaoPlenariaSustentacaoOralResponse?> GetAtivaAsync();
        Task<SessaoPlenariaSustentacaoOralResponse?> GetByIdAsync(long id);
        Task<SessaoPlenariaSustentacaoOralResponse?> GetBySlugAsync(string slug);
        Task<long> CreateAsync(SessaoPlenariaSustentacaoOralCreateRequest request);
        Task<bool> UpdateAsync(long id, SessaoPlenariaSustentacaoOralUpdateRequest request);
        Task<bool> DeleteAsync(long id);
    }
}
