using PortalTCMSP.Domain.Entities.FiscalizacaoEntity.PlanoAnualFiscalizacao;
using PortalTCMSP.Domain.Repositories.Home;

namespace PortalTCMSP.Domain.Repositories.Fiscalizacao
{
    public interface IFiscalizacaoPlanoAnualFiscalizacaoRepository : IBaseRepository<FiscalizacaoPlanoAnualFiscalizacaoResolucao>
    {
        Task<FiscalizacaoPlanoAnualFiscalizacaoResolucao?> FindBySlugAsync(string slug);
        Task<FiscalizacaoPlanoAnualFiscalizacaoResolucao?> GetWithChildrenByIdAsync(long id);
        Task<FiscalizacaoPlanoAnualFiscalizacaoResolucao?> GetWithChildrenBySlugAsync(string slug);
        Task<List<FiscalizacaoPlanoAnualFiscalizacaoResolucao>> AllWithChildrenAsync();

        Task ReplaceDispositivosAsync(long resolucaoId, IEnumerable<FiscalizacaoResolucaoDispositivo> novos);
        Task ReplaceAnexosAsync(long resolucaoId, IEnumerable<FiscalizacaoResolucaoAnexo> novos);
        Task ReplaceAtasAsync(long resolucaoId, IEnumerable<FiscalizacaoResolucaoAta> novos);
        Task ReplaceEmentaAsync(long resolucaoId, FiscalizacaoResolucaoEmenta? nova);
    }
}
