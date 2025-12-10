using PortalTCMSP.Domain.Entities.ServicosEntity.OficioseIntimacoesEntity;
using PortalTCMSP.Domain.Repositories.Home;

namespace PortalTCMSP.Domain.Repositories.Servicos.OficioseIntimacoes
{
    public interface IOficioseIntimacoesRepository : IBaseRepository<Entities.ServicosEntity.OficioseIntimacoesEntity.OficioseIntimacoes>
    {
        Task<Entities.ServicosEntity.OficioseIntimacoesEntity.OficioseIntimacoes?> GetWithChildrenByIdAsync(long id);
        Task<List<Entities.ServicosEntity.OficioseIntimacoesEntity.OficioseIntimacoes>> AllWithChildrenAsync();
        Task<bool> DisableAsync(long id);
        Task<Entities.ServicosEntity.OficioseIntimacoesEntity.OficioseIntimacoes?> GetWithChildrenBySlugAtivoAsync(string slug);
        Task<Entities.ServicosEntity.OficioseIntimacoesEntity.OficioseIntimacoes?> GetBySlugAtivoAsync(string slug);
        Task ReplaceSecoesAsync(long id, IEnumerable<OficioseIntimacoesSecao> novas);
    }
}
