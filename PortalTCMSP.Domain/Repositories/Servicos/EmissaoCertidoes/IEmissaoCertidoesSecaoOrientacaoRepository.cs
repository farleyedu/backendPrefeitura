using PortalTCMSP.Domain.Entities.ServicosEntity.EmissaoCertidoesEntity;
using PortalTCMSP.Domain.Repositories.Home;

namespace PortalTCMSP.Domain.Repositories.Servicos.EmissaoCertidoes
{
    public interface IEmissaoCertidoesSecaoOrientacaoRepository : IBaseRepository<EmissaoCertidoesSecaoOrientacao>
    {
        Task CreateScoesAsync(long id, IEnumerable<EmissaoCertidoesSecaoOrientacao> novos);
        Task UpdateSecoesAsync(long id, IEnumerable<EmissaoCertidoesSecaoOrientacao> novos);
    }
}
