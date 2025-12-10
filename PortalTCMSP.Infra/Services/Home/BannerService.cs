using PortalTCMSP.Domain.DTOs.Requests.BannerRequest;
using PortalTCMSP.Domain.DTOs.Responses.Banner;
using PortalTCMSP.Domain.Mappings;
using PortalTCMSP.Domain.Mappings.Home;
using PortalTCMSP.Domain.Repositories.Home;
using PortalTCMSP.Domain.Services.Home;

namespace PortalTCMSP.Infra.Services.Home
{
    public class BannerService : IBannerService
    {
        private readonly IBannerRepository _bannerRepository;

        public BannerService(IBannerRepository bannerRepository)
        {
            _bannerRepository = bannerRepository;
        }

        public async Task<IEnumerable<BannerResponse>> ObterTodosAsync()
        {
            var banners = await _bannerRepository.AllAsync();
            return banners.Select(b => b.ToResponse());
        }

        public async Task<BannerResponse?> ObterPorIdAsync(long id) 
        {
            var banner = await _bannerRepository.FindByIdAsync(id);
            return banner?.ToResponse();
        }

        public async Task<IEnumerable<BannerResponse>> ObterAtivosAsync() 
        {
            var itens = await _bannerRepository.GetAtivosAsync();
            return itens.Select(x => x.ToResponse());
        }

        public async Task<long> CriarAsync(BannerCreateRequest request)
        {
            var entity = request.ToEntity(DateTime.UtcNow);

            if (entity.Ativo)
                await _bannerRepository.DeactivateAllAsync();

            await _bannerRepository.InsertAsync(entity);
            await _bannerRepository.CommitAsync();
            return entity.Id;
        }

        public async Task<bool> AtualizarAsync(long id, BannerUpdateRequest request)
        {
            var entity = await _bannerRepository.FindByIdAsync(id);
            if (entity is null) return false;

            entity.MapUpdate(request, DateTime.UtcNow);
            await _bannerRepository.UpdateAsync(entity);

            if (entity.Ativo)
                await _bannerRepository.DeactivateAllExceptAsync(entity.Id);

            return await _bannerRepository.CommitAsync();
        }

        public async Task<bool> RemoverAsync(long id)
        {
            await _bannerRepository.DeleteAsync(id);
            return await _bannerRepository.CommitAsync();
        }
    }
}
