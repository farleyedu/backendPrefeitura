using Microsoft.EntityFrameworkCore;
using PortalTCMSP.Domain.DTOs.Requests.Fiscalizacao.ContasDoPrefeito;
using PortalTCMSP.Domain.Entities.FiscalizacaoEntity.ContasDoPrefeito;
using PortalTCMSP.Domain.Repositories.Fiscalizacao;
using PortalTCMSP.Infra.Data.Context;
using PortalTCMSP.Infra.Data.Repositories.Home;

namespace PortalTCMSP.Infra.Data.Repositories.Fiscalizacao.ContasDoPrefeito
{
    public class FiscalizacaoContasDoPrefeitoRepository(PortalTCMSPContext context) : BaseRepository<FiscalizacaoContasDoPrefeito>(context), IFiscalizacaoContasDoPrefeitoRepository
    {
        public Task<FiscalizacaoContasDoPrefeito?> GetWithAnexosByIdAsync(long id)
            => Set.Include(x => x.Anexos).FirstOrDefaultAsync(x => x.Id == id);

        public Task<List<FiscalizacaoContasDoPrefeito>> AllWithAnexosAsync()
            => Set.Include(x => x.Anexos).ToListAsync();

        public async Task ReplaceAnexosAsync(long id, IEnumerable<FiscalizacaoContasDoPrefeitoAnexo> novos)
        {
            var set = Context.Set<FiscalizacaoContasDoPrefeitoAnexo>();
            var antigos = await set.Where(a => a.IdFiscalizacaoContasDoPrefeito == id).ToListAsync();
            if (antigos.Count > 0) set.RemoveRange(antigos);

            foreach (var n in novos)
                n.IdFiscalizacaoContasDoPrefeito = id;

            await set.AddRangeAsync(novos);
        }

        public IQueryable<FiscalizacaoContasDoPrefeito> Search(FiscalizacaoContasDoPrefeitoSearchRequest r)
        {
            var query = Set.Include(x => x.Anexos).AsQueryable();

            if (!string.IsNullOrWhiteSpace(r.Ano))
                query = query.Where(x => x.Ano == r.Ano);

            if (!string.IsNullOrWhiteSpace(r.Pauta))
                query = query.Where(x => x.Pauta == r.Pauta);

            if (r.SessaoDe.HasValue)
                query = query.Where(x => x.DataSessao >= r.SessaoDe.Value);

            if (r.SessaoAte.HasValue)
                query = query.Where(x => x.DataSessao <= r.SessaoAte.Value);

            if (r.PublicadaDe.HasValue)
                query = query.Where(x => x.DataPublicacao >= r.PublicadaDe.Value);

            if (r.PublicadaAte.HasValue)
                query = query.Where(x => x.DataPublicacao <= r.PublicadaAte.Value);

            if (!string.IsNullOrWhiteSpace(r.Query))
            {
                var q = r.Query.Trim().ToLower();
                var like = $"%{q}%";
                query = query.Where(x =>
                    EF.Functions.Like((x.Ano ?? "").ToLower(), like) ||
                    EF.Functions.Like((x.Pauta ?? "").ToLower(), like)
                );
            }

            return query
                .OrderByDescending(x => x.DataPublicacao ?? x.DataCriacao)
                .ThenByDescending(x => x.Id);
        }
    }
}
