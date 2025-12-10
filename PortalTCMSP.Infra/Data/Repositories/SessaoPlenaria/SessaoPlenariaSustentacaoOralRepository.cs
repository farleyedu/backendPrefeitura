using Microsoft.EntityFrameworkCore;
using PortalTCMSP.Domain.Entities.SessaoPlenariaEntity;
using PortalTCMSP.Infra.Data.Context;
using PortalTCMSP.Infra.Data.Repositories.Home;

namespace PortalTCMSP.Domain.Repositories.SessaoPlenaria
{
    public class SessaoPlenariaSustentacaoOralRepository : BaseRepository<SessaoPlenariaSustentacaoOral>, ISessaoPlenariaSustentacaoOralRepository
    {
        public SessaoPlenariaSustentacaoOralRepository(PortalTCMSPContext context) : base(context) { }

        public Task<SessaoPlenariaSustentacaoOral?> FindBySlugAsync(string slug)
           => Set.AsNoTracking().FirstOrDefaultAsync(x => x.Slug == slug);

        public Task<SessaoPlenariaSustentacaoOral?> GetAtivaWithAnexosAsync()
             => Set.AsNoTracking()
              .Include(x => x.Anexos)
              .FirstOrDefaultAsync(x => x.Ativo);

        public Task<SessaoPlenariaSustentacaoOral?> GetWithAnexosByIdAsync(long id)
            => Set.Include(x => x.Anexos).FirstOrDefaultAsync(x => x.Id == id);

        public Task<SessaoPlenariaSustentacaoOral?> GetWithAnexosBySlugAsync(string slug)
            => Set.Include(x => x.Anexos).FirstOrDefaultAsync(x => x.Slug == slug);

        public Task<List<SessaoPlenariaSustentacaoOral>> AllWithAnexosAsync()
            => Set.Include(x => x.Anexos).ToListAsync();

        public async Task ReplaceAnexosAsync(long sustentacaoId, IEnumerable<SessaoPlenariaSustentacaoOralAnexos> novos)
        {
            var parent = await Set.FirstOrDefaultAsync(x => x.Id == sustentacaoId);
            if (parent is null)
                throw new InvalidOperationException("Sustentação Oral não encontrada.");

            await Context.Entry(parent).Collection(p => p.Anexos).LoadAsync();

            if (parent.Anexos.Count > 0)
                Context.RemoveRange(parent.Anexos);

            foreach (var n in novos)
                parent.Anexos.Add(n); 
        }

        public async Task<int> DesativarTodosExcetoAsync(long? idMantidoAtivo)
        {
            var itens = await Set.Where(x => x.Ativo && (!idMantidoAtivo.HasValue || x.Id != idMantidoAtivo.Value))
                                 .ToListAsync();
            foreach (var i in itens)
            {
                i.Ativo = false;
                i.DataAtualizacao = DateTime.UtcNow;
            }
            return itens.Count;
        }
    }
}
