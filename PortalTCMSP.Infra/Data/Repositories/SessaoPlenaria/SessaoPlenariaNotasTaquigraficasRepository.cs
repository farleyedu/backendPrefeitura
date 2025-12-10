using Microsoft.EntityFrameworkCore;
using PortalTCMSP.Domain.DTOs.Requests.SessaoPlenaria.Ata;
using PortalTCMSP.Domain.Entities.SessaoPlenariaEntity;
using PortalTCMSP.Infra.Data.Context;
using PortalTCMSP.Infra.Data.Repositories.Home;

namespace PortalTCMSP.Domain.Repositories.SessaoPlenaria
{
    public class SessaoPlenariaNotasTaquigraficasRepository : BaseRepository<SessaoPlenariaNotasTaquigraficas>, ISessaoPlenariaNotasTaquigraficasRepository
    {
        public SessaoPlenariaNotasTaquigraficasRepository(PortalTCMSPContext context) : base(context) { }

        public Task<SessaoPlenariaNotasTaquigraficas?> GetWithAnexosByIdAsync(long id)
            => Set.Include(x => x.Anexos).FirstOrDefaultAsync(x => x.Id == id);

        public Task<List<SessaoPlenariaNotasTaquigraficas>> AllWithAnexosAsync()
            => Set.Include(x => x.Anexos).ToListAsync();

        public async Task ReplaceAnexosAsync(long notasId, IEnumerable<SessaoPlenariaNotasTaquigraficasAnexos> novos)
        {
            var parent = await Set.FirstOrDefaultAsync(x => x.Id == notasId);
            if (parent is null)
                throw new InvalidOperationException("Notas taquigráficas não encontrada.");

            await Context.Entry(parent).Collection(p => p.Anexos).LoadAsync();

            if (parent.Anexos.Count > 0)
                Context.RemoveRange(parent.Anexos);

            foreach (var n in novos)
                parent.Anexos.Add(n); 

        }

        public async Task<IEnumerable<SessaoPlenariaNotasTaquigraficas>> Search(SessaoPlenariaNotasTaquigraficasSearchRequest request)
        {
            var lista = await Set
                .Include(n => n.Anexos)
                .ToListAsync();

            if (request.IdSessaoPlenaria.HasValue)
                lista = lista.Where(n => n.IdSessaoPlenaria == request.IdSessaoPlenaria.Value).ToList();

            if (request.Tipo.HasValue)
            {
                lista = lista.Where(n => n.Tipo == request.Tipo.Value).ToList();
            }

            if (request.PublicadaDe.HasValue)
                lista = lista.Where(n => n.DataPublicacao >= request.PublicadaDe.Value).ToList();

            if (request.PublicadaAte.HasValue)
                lista = lista.Where(n => n.DataPublicacao <= request.PublicadaAte.Value).ToList();

            if (!string.IsNullOrWhiteSpace(request.Query))
            {
                var q = request.Query.Trim().ToLower();
                lista = lista.Where(n =>
                    (n.Numero ?? "").ToLower().Contains(q)
                ).ToList();
            }

            lista = lista
                .OrderByDescending(n => n.DataPublicacao ?? n.DataCriacao)
                .ThenByDescending(n => n.Id)
                .ToList();

            return lista;
        }
    }
}
