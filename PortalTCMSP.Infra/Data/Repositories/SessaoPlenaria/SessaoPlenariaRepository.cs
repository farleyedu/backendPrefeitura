using Microsoft.EntityFrameworkCore;
using PortalTCMSP.Infra.Data.Context;
using PortalTCMSP.Infra.Data.Repositories.Home;

namespace PortalTCMSP.Domain.Repositories.SessaoPlenaria
{
    public class SessaoPlenariaRepository : BaseRepository<Entities.SessaoPlenariaEntity.SessaoPlenaria>, ISessaoPlenariaRepository
    {
        public SessaoPlenariaRepository(PortalTCMSPContext context) : base(context) { }

        public Task<Entities.SessaoPlenariaEntity.SessaoPlenaria?> FindBySlugAsync(string slug)
            => Set.AsNoTracking().FirstOrDefaultAsync(x => x.Slug == slug);

        public Task<Entities.SessaoPlenariaEntity.SessaoPlenaria?> GetWithChildrenByIdAsync(long id)
            => Set.Include(x => x.Pautas)
                  .Include(x => x.Atas)
                  .Include(x => x.NotasTaquigraficas)
                  .FirstOrDefaultAsync(x => x.Id == id);

        public Task<Entities.SessaoPlenariaEntity.SessaoPlenaria?> GetWithChildrenBySlugAsync(string slug)
            => Set.Include(x => x.Pautas)
                  .Include(x => x.Atas)
                  .Include(x => x.NotasTaquigraficas)
                  .FirstOrDefaultAsync(x => x.Slug == slug);

        public Task<List<Entities.SessaoPlenariaEntity.SessaoPlenaria>> AllWithChildrenAsync()
            => Set.Include(x => x.Pautas)
                  .Include(x => x.Atas)
                  .Include(x => x.NotasTaquigraficas)
                  .ToListAsync();

        public async Task DeactivateAllAsync()
        {
            var rows = await Set.Where(s => s.Ativo == "S").ToListAsync();
            if (rows.Count == 0) return;
            foreach (var s in rows) s.Ativo = "N";
            Set.UpdateRange(rows);
        }

        public async Task DeactivateAllExceptAsync(long exceptId)
        {
            var rows = await Set.Where(s => s.Id != exceptId && s.Ativo == "S").ToListAsync();
            if (rows.Count == 0) return;
            foreach (var s in rows) s.Ativo = "N";
            Set.UpdateRange(rows);
        }

        public Task<List<Entities.SessaoPlenariaEntity.SessaoPlenaria>> GetAtivosAsync()
            => Set.AsNoTracking()
                  .Where(s => s.Ativo == "S")
                  .OrderByDescending(s => s.DataPublicacao ?? s.DataCriacao)
                  .ToListAsync();

        public Task<Entities.SessaoPlenariaEntity.SessaoPlenaria?> GetAtivaWithChildrenAsync()
            => Set.AsNoTracking()
                  .Where(s => s.Ativo == "S")
                  .OrderByDescending(s => s.DataPublicacao ?? s.DataCriacao)
                  .Include(x => x.Pautas)
                  .Include(x => x.Atas)
                  .Include(x => x.NotasTaquigraficas)
                  .FirstOrDefaultAsync();
    }
}
