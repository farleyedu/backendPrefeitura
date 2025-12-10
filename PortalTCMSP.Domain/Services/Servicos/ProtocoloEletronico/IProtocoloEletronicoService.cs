using PortalTCMSP.Domain.DTOs.Requests.Servicos.ProtocoloEletronico;
using PortalTCMSP.Domain.DTOs.Responses.Servicos.ProtocoloEletronico;

namespace PortalTCMSP.Domain.Services.Servicos.ProtocoloEletronico
{
    public interface IProtocoloEletronicoService
    {
        Task<IEnumerable<ProtocoloEletronicoResponse>> GetAllAsync();
        Task<ProtocoloEletronicoResponse?> GetByIdAsync(long id);
        Task<long> CreateAsync(ProtocoloEletronicoCreateRequest request);
        Task<bool> UpdateAsync(long id, ProtocoloEletronicoUpdateRequest request);
        Task<bool> DeleteAsync(long id);
        Task<ProtocoloEletronicoResponse?> GetWithChildrenBySlugAtivoAsync(string slug);
        Task<bool> CreateAcoesAsync(long id, List<ProtocoloEletronicoAcaoRequest> novas);
        Task<bool> UpdateAcoesAsync(long id, List<ProtocoloEletronicoAcaoUpdate> novas);
    }
}
