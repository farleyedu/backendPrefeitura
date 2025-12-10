using PortalTCMSP.Domain.Entities.ServicosEntity.OficioseIntimacoesEntity;
using PortalTCMSP.Domain.Repositories.Home;

namespace PortalTCMSP.Domain.Repositories.Servicos.OficioseIntimacoes
{
    public interface IOficioseIntimacoesSecaoItemRepository : IBaseRepository<OficioseIntimacoesSecaoItem>
    {
        Task CreateSecaoItensAsync(long id, IEnumerable<OficioseIntimacoesSecaoItem> novas);
        Task UpdateSecaoItensAsync(long id, IEnumerable<OficioseIntimacoesSecaoItem> novas);
    }
}
