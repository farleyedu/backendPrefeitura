using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Extensions;
using PortalTCMSP.Domain.DTOs.Requests.SessaoPlenaria.Ata;
using PortalTCMSP.Domain.Entities.SessaoPlenariaEntity;
using PortalTCMSP.Infra.Data.Context;
using PortalTCMSP.Infra.Data.Repositories.Home;

namespace PortalTCMSP.Domain.Repositories.SessaoPlenaria
{
    public class SessaoPlenariaAtasRepository : BaseRepository<SessaoPlenariaAta>, ISessaoPlenariaAtasRepository
    {
        public SessaoPlenariaAtasRepository(PortalTCMSPContext context) : base(context) { }

        public Task<SessaoPlenariaAta?> GetWithAnexosByIdAsync(long id)
            => Set.Include(x => x.Anexos).FirstOrDefaultAsync(x => x.Id == id);

        public Task<List<SessaoPlenariaAta>> AllWithAnexosAsync()
            => Set.Include(x => x.Anexos).ToListAsync();

        public async Task ReplaceAnexosAsync(SessaoPlenariaAta parent, IEnumerable<SessaoPlenariaAtaAnexo> novos)
        {
            await Context.Entry(parent).Collection(p => p.Anexos).LoadAsync();

            if (parent.Anexos.Count > 0)
                Context.RemoveRange(parent.Anexos);

            foreach (var n in novos)
                parent.Anexos.Add(n);
        }
        public Task<bool> ExistsSessaoPlenariaAsync(long id)
                => Context.Set<PortalTCMSP.Domain.Entities.SessaoPlenariaEntity.SessaoPlenaria>().AnyAsync(s => s.Id == id);


        public async Task<IEnumerable<SessaoPlenariaAta>> Search(SessaoPlenariaAtaSearchRequest request)
        {
            var lista = await Set
                .Include(a => a.Anexos)
                .ToListAsync();

            if (request.IdSessaoPlenaria.HasValue)
                lista = lista.Where(a => a.IdSessaoPlenaria == request.IdSessaoPlenaria.Value).ToList();

            if (request.Tipo.HasValue)
            {
                lista = lista.Where(a => a.Tipo == request.Tipo.Value).ToList();
            }

            if (request.PublicadaDe.HasValue)
                lista = lista.Where(a => a.DataPublicacao >= request.PublicadaDe.Value).ToList();

            if (request.PublicadaAte.HasValue)
                lista = lista.Where(a => a.DataPublicacao <= request.PublicadaAte.Value).ToList();

            if (!string.IsNullOrWhiteSpace(request.Query))
            {
                var q = request.Query.Trim().ToLower();
                lista = lista.Where(a =>
                    (a.Numero ?? "").ToLower().Contains(q)
                ).ToList();
            }

            lista = lista
                .OrderByDescending(a => a.DataPublicacao ?? a.DataCriacao)
                .ThenByDescending(a => a.Id)
                .ToList();

            return lista;
        }
    }
}
