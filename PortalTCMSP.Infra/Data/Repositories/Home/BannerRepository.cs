using Microsoft.EntityFrameworkCore;
using PortalTCMSP.Domain.Entities.BannerEntity;
using PortalTCMSP.Domain.Repositories.Home;
using PortalTCMSP.Infra.Data.Context;

namespace PortalTCMSP.Infra.Data.Repositories.Home
{
    public class BannerRepository : BaseRepository<Banner>, IBannerRepository
    {
        public BannerRepository(PortalTCMSPContext context) : base(context) { }

        public Task<List<Banner>> GetAtivosAsync()
            => Set.AsNoTracking().Where(b => b.Ativo).ToListAsync();

        public async Task DeactivateAllAsync()
        {
            var ativos = await Set.Where(b => b.Ativo).ToListAsync();
            if (ativos.Count == 0) return;
            foreach (var b in ativos) b.Ativo = false;
            Set.UpdateRange(ativos);
        }

        public async Task DeactivateAllExceptAsync(long exceptId)
        {
            var ativos = await Set.Where(b => b.Ativo && b.Id != exceptId).ToListAsync();
            if (ativos.Count == 0) return;
            foreach (var b in ativos) b.Ativo = false;
            Set.UpdateRange(ativos);
        }
    }
}
