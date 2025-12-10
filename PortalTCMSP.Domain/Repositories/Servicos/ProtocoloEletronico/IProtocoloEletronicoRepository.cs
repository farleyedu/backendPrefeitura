using PortalTCMSP.Domain.Entities.ServicosEntity.ProtocoloEletronicoEntity;
using PortalTCMSP.Domain.Repositories.Home;

namespace PortalTCMSP.Domain.Repositories.Servicos.ProtocoloEletronico
{
    public interface IProtocoloEletronicoRepository : IBaseRepository<Entities.ServicosEntity.ProtocoloEletronicoEntity.ProtocoloEletronico>
    {
        Task<Entities.ServicosEntity.ProtocoloEletronicoEntity.ProtocoloEletronico?> GetWithChildrenByIdAsync(long id);
        Task<List<Entities.ServicosEntity.ProtocoloEletronicoEntity.ProtocoloEletronico>> AllWithChildrenAsync();
        Task<bool> DisableAsync(long id);
        Task<Entities.ServicosEntity.ProtocoloEletronicoEntity.ProtocoloEletronico?> GetBySlugAtivoAsync(string slug);
        Task<Entities.ServicosEntity.ProtocoloEletronicoEntity.ProtocoloEletronico?> GetWithChildrenBySlugAtivoAsync(string slug);
        Task ReplaceAcoesAsync(long id, IEnumerable<ProtocoloEletronicoAcao> novas);
    }
}
