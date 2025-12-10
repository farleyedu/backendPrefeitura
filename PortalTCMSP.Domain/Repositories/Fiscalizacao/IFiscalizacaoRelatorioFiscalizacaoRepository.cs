using PortalTCMSP.Domain.Entities.FiscalizacaoEntity.RelatorioFiscalizacao;
using PortalTCMSP.Domain.Repositories.Home;

namespace PortalTCMSP.Domain.Repositories.Fiscalizacao
{
    public interface IFiscalizacaoRelatorioFiscalizacaoRepository : IBaseRepository<FiscalizacaoRelatorioFiscalizacao>
    {
        Task<FiscalizacaoRelatorioFiscalizacao?> FindBySlugAsync(string slug);
        Task<FiscalizacaoRelatorioFiscalizacao?> GetWithChildrenByIdAsync(long id);
        Task<FiscalizacaoRelatorioFiscalizacao?> GetWithChildrenBySlugAsync(string slug);
        Task<List<FiscalizacaoRelatorioFiscalizacao>> AllWithChildrenAsync();
        Task ReplaceCarrosselAsync(long id, IEnumerable<FiscalizacaoRelatorioFiscalizacaoCarrossel> novosCarrossel);
    }
}
