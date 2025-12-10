using Microsoft.EntityFrameworkCore;
using PortalTCMSP.Domain.DTOs.Requests.SessaoPlenaria.Pauta;
using PortalTCMSP.Domain.Entities.SessaoPlenariaEntity;
using PortalTCMSP.Infra.Data.Context;
using PortalTCMSP.Infra.Data.Repositories.Home;

namespace PortalTCMSP.Domain.Repositories.SessaoPlenaria
{
    public class SessaoPlenariaPautaRepository : BaseRepository<SessaoPlenariaPauta>, ISessaoPlenariaPautaRepository
    {
        public SessaoPlenariaPautaRepository(PortalTCMSPContext context) : base(context) { }

        public Task<SessaoPlenariaPauta?> GetWithAnexosByIdAsync(long id)
            => Set.Include(x => x.Anexos).FirstOrDefaultAsync(x => x.Id == id);

        public Task<List<SessaoPlenariaPauta>> AllWithAnexosAsync()
            => Set.Include(x => x.Anexos).ToListAsync();

        public async Task ReplaceAnexosAsync(long pautaId, IEnumerable<SessaoPlenariaPautaAnexo> novos)
        {
            var parent = await Set.FirstOrDefaultAsync(x => x.Id == pautaId);
            if (parent is null)
                throw new InvalidOperationException("Pauta não encontrada.");

            await Context.Entry(parent).Collection(p => p.Anexos).LoadAsync();

            if (parent.Anexos.Count > 0)
                Context.RemoveRange(parent.Anexos);

            foreach (var n in novos)
                parent.Anexos.Add(n);  

        }

        public async Task<IEnumerable<SessaoPlenariaPauta>> Search(SessaoPlenariaPautaSearchRequest request)
        {
            // segue o padrão da sua Campanha: carrega e filtra em memóri
            var lista = await Set
                .Include(p => p.Anexos)
                .ToListAsync();

            if (request.IdSessaoPlenaria.HasValue)
                lista = lista.Where(p => p.IdSessaoPlenaria == request.IdSessaoPlenaria.Value).ToList();

            if (request.Tipo.HasValue)
                lista = lista.Where(p => p.Tipo == request.Tipo.Value).ToList();

            if (request.PublicadaDe.HasValue)
                lista = lista.Where(p => p.DataPublicacao >= request.PublicadaDe.Value).ToList();

            if (request.PublicadaAte.HasValue)
                lista = lista.Where(p => p.DataPublicacao <= request.PublicadaAte.Value).ToList();

            if (!string.IsNullOrWhiteSpace(request.Query))
            {
                var q = request.Query.Trim().ToLower();
                lista = lista.Where(p =>
                    (p.Numero ?? "").ToLower().Contains(q)
                ).ToList();
            }

            return lista;
        }
    }
}
