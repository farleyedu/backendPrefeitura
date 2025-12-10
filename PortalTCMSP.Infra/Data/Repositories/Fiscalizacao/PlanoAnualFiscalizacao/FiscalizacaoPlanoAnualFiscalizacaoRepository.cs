using Microsoft.EntityFrameworkCore;
using PortalTCMSP.Domain.Entities.FiscalizacaoEntity.PlanoAnualFiscalizacao;
using PortalTCMSP.Domain.Repositories.Fiscalizacao;
using PortalTCMSP.Infra.Data.Context;
using PortalTCMSP.Infra.Data.Repositories.Home;

namespace PortalTCMSP.Infra.Data.Repositories.Fiscalizacao.PlanoAnualFiscalizacao
{
    public class FiscalizacaoPlanoAnualFiscalizacaoRepository(PortalTCMSPContext context) : BaseRepository<FiscalizacaoPlanoAnualFiscalizacaoResolucao>(context), IFiscalizacaoPlanoAnualFiscalizacaoRepository
    {
        public Task<FiscalizacaoPlanoAnualFiscalizacaoResolucao?> FindBySlugAsync(string slug)
            => Set.AsNoTracking().FirstOrDefaultAsync(x => x.Slug == slug);

        public Task<FiscalizacaoPlanoAnualFiscalizacaoResolucao?> GetWithChildrenByIdAsync(long id)
            => Set
                .Include(x => x.Ementa).ThenInclude(e => e.LinksArtigos)
                .Include(x => x.Dispositivos).ThenInclude(d => d.Paragrafo)
                .Include(x => x.Dispositivos).ThenInclude(d => d.Incisos)
                .Include(x => x.Anexos).ThenInclude(a => a.TemasPrioritarios)
                .Include(x => x.Anexos).ThenInclude(a => a.Atividades).ThenInclude(at => at.AtividadeItem)
                .Include(x => x.Anexos).ThenInclude(a => a.Distribuicao)
                .Include(x => x.Atas)
                .FirstOrDefaultAsync(x => x.Id == id);

        public Task<FiscalizacaoPlanoAnualFiscalizacaoResolucao?> GetWithChildrenBySlugAsync(string slug)
            => Set
                .Include(x => x.Ementa).ThenInclude(e => e.LinksArtigos)
                .Include(x => x.Dispositivos).ThenInclude(d => d.Paragrafo)
                .Include(x => x.Dispositivos).ThenInclude(d => d.Incisos)
                .Include(x => x.Anexos).ThenInclude(a => a.TemasPrioritarios)
                .Include(x => x.Anexos).ThenInclude(a => a.Atividades).ThenInclude(at => at.AtividadeItem)
                .Include(x => x.Anexos).ThenInclude(a => a.Distribuicao)
                .Include(x => x.Atas)
                .FirstOrDefaultAsync(x => x.Slug == slug);

        public Task<List<FiscalizacaoPlanoAnualFiscalizacaoResolucao>> AllWithChildrenAsync()
            => Set
                .Include(x => x.Ementa).ThenInclude(e => e.LinksArtigos)
                .Include(x => x.Dispositivos).ThenInclude(d => d.Paragrafo)
                .Include(x => x.Dispositivos).ThenInclude(d => d.Incisos)
                .Include(x => x.Anexos).ThenInclude(a => a.TemasPrioritarios)
                .Include(x => x.Anexos).ThenInclude(a => a.Atividades).ThenInclude(at => at.AtividadeItem)
                .Include(x => x.Anexos).ThenInclude(a => a.Distribuicao)
                .Include(x => x.Atas)
                .ToListAsync();

        public async Task ReplaceDispositivosAsync(long resolucaoId, IEnumerable<FiscalizacaoResolucaoDispositivo> novos)
        {
            var set = Context.Set<FiscalizacaoResolucaoDispositivo>();
            var antigos = await set.Where(d => d.ResolucaoId == resolucaoId).ToListAsync();
            if (antigos.Count > 0) set.RemoveRange(antigos);

            foreach (var n in novos) n.ResolucaoId = resolucaoId;
            await set.AddRangeAsync(novos);
        }

        public async Task ReplaceAnexosAsync(long resolucaoId, IEnumerable<FiscalizacaoResolucaoAnexo> novos)
        {
            var set = Context.Set<FiscalizacaoResolucaoAnexo>();
            var antigos = await set.Where(a => a.ResolucaoId == resolucaoId).ToListAsync();
            if (antigos.Count > 0) set.RemoveRange(antigos);

            foreach (var n in novos) n.ResolucaoId = resolucaoId;
            await set.AddRangeAsync(novos);
        }

        public async Task ReplaceAtasAsync(long resolucaoId, IEnumerable<FiscalizacaoResolucaoAta> novos)
        {
            var set = Context.Set<FiscalizacaoResolucaoAta>();
            var antigos = await set.Where(a => a.ResolucaoId == resolucaoId).ToListAsync();
            if (antigos.Count > 0) set.RemoveRange(antigos);

            foreach (var n in novos) n.ResolucaoId = resolucaoId;
            await set.AddRangeAsync(novos);
        }

        public async Task ReplaceEmentaAsync(long resolucaoId, FiscalizacaoResolucaoEmenta? nova)
        {
            var emSet = Context.Set<FiscalizacaoResolucaoEmenta>();
            var linksSet = Context.Set<FiscalizacaoResolucaoEmentaLink>();
            var antiga = await emSet.FirstOrDefaultAsync(e => e.ResolucaoId == resolucaoId);
            if (antiga != null)
            {
                var antigosLinks = await linksSet.Where(l => l.EmentaId == antiga.Id).ToListAsync();
                if (antigosLinks.Count > 0) linksSet.RemoveRange(antigosLinks);
                emSet.Remove(antiga);
            }

            if (nova != null)
    {
                nova.ResolucaoId = resolucaoId;
                await emSet.AddAsync(nova);
            }
        }
    }
}
