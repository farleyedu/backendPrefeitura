using PortalTCMSP.Domain.Entities.ServicosEntity.ProtocoloEletronicoEntity;
using PortalTCMSP.Domain.Repositories.Home;

namespace PortalTCMSP.Domain.Repositories.Servicos.ProtocoloEletronico
{
    public interface IProtocoloEletronicoAcaoRepository : IBaseRepository<ProtocoloEletronicoAcao>
    {
        Task CreateAcoesAsync(long id, IEnumerable<ProtocoloEletronicoAcao> novas);
        Task UpdateAcoesAsync(long id, IEnumerable<ProtocoloEletronicoAcao> novas);
    }
}
