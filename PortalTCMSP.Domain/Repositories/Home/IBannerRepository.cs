using PortalTCMSP.Domain.Entities.BannerEntity;

namespace PortalTCMSP.Domain.Repositories.Home
{
    public interface IBannerRepository : IBaseRepository<Banner> 
    {
        Task<List<Banner>> GetAtivosAsync();
        Task DeactivateAllAsync();
        Task DeactivateAllExceptAsync(long exceptId);
    }
}
