using Microsoft.EntityFrameworkCore;
using PortalTCMSP.Domain.Entities.FiscalizacaoEntity.RelatorioFiscalizacao;
using PortalTCMSP.Domain.Repositories.Fiscalizacao;
using PortalTCMSP.Infra.Data.Context;
using PortalTCMSP.Infra.Data.Repositories.Home;

namespace PortalTCMSP.Infra.Data.Repositories.Fiscalizacao.RelatorioFiscalizacao
{
    public class FiscalizacaoRelatoriosFiscalizacaoRepository(PortalTCMSPContext context) : BaseRepository<FiscalizacaoRelatorioFiscalizacao>(context), IFiscalizacaoRelatorioFiscalizacaoRepository
    {
        public Task<FiscalizacaoRelatorioFiscalizacao?> FindBySlugAsync(string slug)
            => Set.AsNoTracking().FirstOrDefaultAsync(x => x.Slug == slug);

        public Task<FiscalizacaoRelatorioFiscalizacao?> GetWithChildrenByIdAsync(long id)
            => Set
                .Include(x => x.Carrocel).ThenInclude(c => c.ConteudoCarrocel).ThenInclude(cc => cc.ConteudoLink).ThenInclude(cl => cl.ConteudoDestaque).ThenInclude(d => d.Descricoes)
                .Include(x => x.Carrocel).ThenInclude(c => c.ConteudoCarrocel).ThenInclude(cc => cc.ConteudoLink).ThenInclude(cl => cl.DocumentosAnexos)
                .Include(x => x.Carrocel).ThenInclude(c => c.ConteudoCarrocel).ThenInclude(cc => cc.ConteudoLink).ThenInclude(cl => cl.ImagensAnexas)
                .Include(x => x.Carrocel).ThenInclude(c => c.ConteudoCarrocel).ThenInclude(cc => cc.ConteudoLink).ThenInclude(cl => cl.TcRelacionados)
                .FirstOrDefaultAsync(x => x.Id == id);

        public Task<FiscalizacaoRelatorioFiscalizacao?> GetWithChildrenBySlugAsync(string slug)
            => Set
                .Include(x => x.Carrocel).ThenInclude(c => c.ConteudoCarrocel).ThenInclude(cc => cc.ConteudoLink).ThenInclude(cl => cl.ConteudoDestaque).ThenInclude(d => d.Descricoes)
                .Include(x => x.Carrocel).ThenInclude(c => c.ConteudoCarrocel).ThenInclude(cc => cc.ConteudoLink).ThenInclude(cl => cl.DocumentosAnexos)
                .Include(x => x.Carrocel).ThenInclude(c => c.ConteudoCarrocel).ThenInclude(cc => cc.ConteudoLink).ThenInclude(cl => cl.ImagensAnexas)
                .Include(x => x.Carrocel).ThenInclude(c => c.ConteudoCarrocel).ThenInclude(cc => cc.ConteudoLink).ThenInclude(cl => cl.TcRelacionados)
                .FirstOrDefaultAsync(x => x.Slug == slug);

        public Task<List<FiscalizacaoRelatorioFiscalizacao>> AllWithChildrenAsync()
            => Set
                .Include(x => x.Carrocel).ThenInclude(c => c.ConteudoCarrocel).ThenInclude(cc => cc.ConteudoLink).ThenInclude(cl => cl.ConteudoDestaque).ThenInclude(d => d.Descricoes)
                .Include(x => x.Carrocel).ThenInclude(c => c.ConteudoCarrocel).ThenInclude(cc => cc.ConteudoLink).ThenInclude(cl => cl.DocumentosAnexos)
                .Include(x => x.Carrocel).ThenInclude(c => c.ConteudoCarrocel).ThenInclude(cc => cc.ConteudoLink).ThenInclude(cl => cl.ImagensAnexas)
                .Include(x => x.Carrocel).ThenInclude(c => c.ConteudoCarrocel).ThenInclude(cc => cc.ConteudoLink).ThenInclude(cl => cl.TcRelacionados)
                .ToListAsync();

        public async Task ReplaceCarrosselAsync(long conteudoId, IEnumerable<FiscalizacaoRelatorioFiscalizacaoCarrossel> novos)
        {
            var set = Context.Set<FiscalizacaoRelatorioFiscalizacaoCarrossel>();
            var antigos = await set.Where(t => t.IdConteudo == conteudoId).ToListAsync();
            if (antigos.Count > 0) set.RemoveRange(antigos);

            foreach (var n in novos) n.IdConteudo = conteudoId;
            await set.AddRangeAsync(novos);
        }
    }
}
