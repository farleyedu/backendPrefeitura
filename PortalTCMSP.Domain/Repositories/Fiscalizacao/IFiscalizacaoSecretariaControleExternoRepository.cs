using PortalTCMSP.Domain.Entities.FiscalizacaoEntity;
using PortalTCMSP.Domain.Entities.FiscalizacaoEntity.SecretariaControleExterno;
using PortalTCMSP.Domain.Repositories.Home;

namespace PortalTCMSP.Domain.Repositories.Fiscalizacao
{
    public interface IFiscalizacaoSecretariaControleExternoRepository : IBaseRepository<FiscalizacaoSecretariaControleExterno>
    {
        Task<FiscalizacaoSecretariaControleExterno?> FindBySlugAsync(string slug);
        Task<FiscalizacaoSecretariaControleExterno?> GetWithChildrenByIdAsync(long id);
        Task<FiscalizacaoSecretariaControleExterno?> GetWithChildrenBySlugAsync(string slug);
        Task<List<FiscalizacaoSecretariaControleExterno>> AllWithChildrenAsync();
        Task ReplaceTitulosAsync(long conteudoId, IEnumerable<FiscalizacaoSecretariaSecaoConteudoTitulo> novos);
        Task ReplaceCarrosselAsync(long conteudoId, IEnumerable<FiscalizacaoSecretariaSecaoConteudoCarrosselItem> novos);
    }
}
