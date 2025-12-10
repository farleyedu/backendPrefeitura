using PortalTCMSP.Domain.DTOs.Requests.BannerRequest;
using PortalTCMSP.Domain.DTOs.Responses.Banner;

namespace PortalTCMSP.Domain.Services.Home
{
    public interface IBannerService
    {
        Task<IEnumerable<BannerResponse>> ObterTodosAsync();
        Task<BannerResponse?> ObterPorIdAsync(long id);
        Task<IEnumerable<BannerResponse>> ObterAtivosAsync();
        Task<long> CriarAsync(BannerCreateRequest request);
        Task<bool> AtualizarAsync(long id, BannerUpdateRequest request);
        Task<bool> RemoverAsync(long id);
    }
}
