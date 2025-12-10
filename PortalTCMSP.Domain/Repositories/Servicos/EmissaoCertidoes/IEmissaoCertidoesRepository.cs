using PortalTCMSP.Domain.Entities.ServicosEntity.EmissaoCertidoesEntity;
using PortalTCMSP.Domain.Repositories.Home;

namespace PortalTCMSP.Domain.Repositories.Servicos.EmissaoCertidoes
{
    public interface IEmissaoCertidoesRepository : IBaseRepository<Entities.ServicosEntity.EmissaoCertidoesEntity.EmissaoCertidoes>
    {
        Task<Entities.ServicosEntity.EmissaoCertidoesEntity.EmissaoCertidoes?> GetWithChildrenByIdAsync(long id);
        Task<List<Entities.ServicosEntity.EmissaoCertidoesEntity.EmissaoCertidoes>> AllWithChildrenAsync();
        Task<bool> DisableAsync(long id);
        Task<Entities.ServicosEntity.EmissaoCertidoesEntity.EmissaoCertidoes?> GetWithChildrenBySlugAtivoAsync(string slug);
        Task<Entities.ServicosEntity.EmissaoCertidoesEntity.EmissaoCertidoes?> GetBySlugAtivoAsync(string slug);
        Task ReplaceAcoesAsync(long id, IEnumerable<EmissaoCertidoesAcao> novos);
        Task ReplaceSecoesAsync(long id, IEnumerable<EmissaoCertidoesSecaoOrientacao> novas);
    }
}
