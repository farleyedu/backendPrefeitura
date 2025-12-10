using Microsoft.EntityFrameworkCore;
using PortalTCMSP.Domain.Entities.FiscalizacaoEntity.SecretariaControleExterno;
using PortalTCMSP.Domain.Repositories.Fiscalizacao;
using PortalTCMSP.Infra.Data.Context;
using PortalTCMSP.Infra.Data.Repositories.Home;

namespace PortalTCMSP.Infra.Data.Repositories.Fiscalizacao
{
    public class FiscalizacaoSecretariaControleExternoRepository(PortalTCMSPContext context) : BaseRepository<FiscalizacaoSecretariaControleExterno>(context), IFiscalizacaoSecretariaControleExternoRepository
    {
        public Task<FiscalizacaoSecretariaControleExterno?> FindBySlugAsync(string slug)
            => Set.AsNoTracking().FirstOrDefaultAsync(x => x.Slug == slug);

        public Task<FiscalizacaoSecretariaControleExterno?> GetWithChildrenByIdAsync(long id)
            => Set
                .Include(x => x.Titulos)
                .Include(x => x.Carrossel)
                .FirstOrDefaultAsync(x => x.Id == id);

        public Task<FiscalizacaoSecretariaControleExterno?> GetWithChildrenBySlugAsync(string slug)
            => Set
                .Include(x => x.Titulos)
                .Include(x => x.Carrossel)
                .FirstOrDefaultAsync(x => x.Slug == slug);

        public Task<List<FiscalizacaoSecretariaControleExterno>> AllWithChildrenAsync()
            => Set
                .Include(x => x.Titulos)
                .Include(x => x.Carrossel)
                .ToListAsync();

        public async Task ReplaceTitulosAsync(long conteudoId, IEnumerable<FiscalizacaoSecretariaSecaoConteudoTitulo> novos)
        {
            var set = Context.Set<FiscalizacaoSecretariaSecaoConteudoTitulo>();
            var antigos = await set.Where(t => t.IdSecaoConteudo == conteudoId).ToListAsync();
            if (antigos.Count > 0) set.RemoveRange(antigos);

            foreach (var n in novos) n.IdSecaoConteudo = conteudoId;
            await set.AddRangeAsync(novos);
        }

        public async Task ReplaceCarrosselAsync(long conteudoId, IEnumerable<FiscalizacaoSecretariaSecaoConteudoCarrosselItem> novos)
        {
            var set = Context.Set<FiscalizacaoSecretariaSecaoConteudoCarrosselItem>();
            var antigos = await set.Where(c => c.IdSecaoConteudo == conteudoId).ToListAsync();
            if (antigos.Count > 0) set.RemoveRange(antigos);

            foreach (var n in novos) n.IdSecaoConteudo = conteudoId;
            await set.AddRangeAsync(novos);
        }
    }
}
