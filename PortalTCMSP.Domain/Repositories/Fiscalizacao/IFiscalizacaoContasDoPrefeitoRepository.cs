using PortalTCMSP.Domain.DTOs.Requests.Fiscalizacao.ContasDoPrefeito;
using PortalTCMSP.Domain.Entities.FiscalizacaoEntity.ContasDoPrefeito;
using PortalTCMSP.Domain.Repositories.Home;

namespace PortalTCMSP.Domain.Repositories.Fiscalizacao
{
    public interface IFiscalizacaoContasDoPrefeitoRepository : IBaseRepository<FiscalizacaoContasDoPrefeito>
    {
        Task<FiscalizacaoContasDoPrefeito?> GetWithAnexosByIdAsync(long id);
        Task<List<FiscalizacaoContasDoPrefeito>> AllWithAnexosAsync();
        Task ReplaceAnexosAsync(long id, IEnumerable<FiscalizacaoContasDoPrefeitoAnexo> novos);
        IQueryable<FiscalizacaoContasDoPrefeito> Search(FiscalizacaoContasDoPrefeitoSearchRequest request);
    }
}
