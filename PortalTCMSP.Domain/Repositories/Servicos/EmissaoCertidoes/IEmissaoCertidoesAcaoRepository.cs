using PortalTCMSP.Domain.Entities.ServicosEntity.EmissaoCertidoesEntity;
using PortalTCMSP.Domain.Repositories.Home;

namespace PortalTCMSP.Domain.Repositories.Servicos.EmissaoCertidoes
{
    public interface IEmissaoCertidoesAcaoRepository : IBaseRepository<EmissaoCertidoesAcao>
    {
        Task CreateAcoesAsync(long id, IEnumerable<EmissaoCertidoesAcao> novos);
        Task UpdateAcoesAsync(long id, IEnumerable<EmissaoCertidoesAcao> novos);
    }
}
