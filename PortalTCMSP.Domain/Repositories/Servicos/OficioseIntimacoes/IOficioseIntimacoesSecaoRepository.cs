using PortalTCMSP.Domain.Entities.ServicosEntity.OficioseIntimacoesEntity;
using PortalTCMSP.Domain.Repositories.Home;

namespace PortalTCMSP.Domain.Repositories.Servicos.OficioseIntimacoes
{
    public interface IOficioseIntimacoesSecaoRepository : IBaseRepository<OficioseIntimacoesSecao>
    {
        Task CreateSecoesAsync(long id, IEnumerable<OficioseIntimacoesSecao> novas);
        Task UpdateSecoesAsync(long id, IEnumerable<OficioseIntimacoesSecao> novas);
    }
}
