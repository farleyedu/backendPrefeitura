using PortalTCMSP.Domain.DTOs.Requests.Servicos.MultasProcedimentos;
using PortalTCMSP.Domain.DTOs.Responses.Servicos.MultasProcedimentos;

namespace PortalTCMSP.Domain.Services.Servicos.MultasProcedimentos
{
    public interface IMultasProcedimentosService
    {
        Task<IEnumerable<MultasProcedimentosResponse>> GetAllAsync();
        Task<MultasProcedimentosResponse?> GetByIdAsync(long id);
        Task<long> CreateAsync(MultasProcedimentosCreateRequest request);
        Task<bool> CreateProcedimentosAsync(long id, List<MultasProcedimentosProcedimentoRequest> novos);
        Task<bool> CreatePortariaRelacionadasAsync(long id, List<MultasProcedimentosPortariaRelacionadaRequest> novos);
        Task<bool> UpdateAsync(long id, MultasProcedimentosUpdateRequest request);
        Task<bool> UpdateProcedimentosAsync(long id, List<MultasProcedimentosProcedimentoUpdate> novos);
        Task<bool> UpdatePortariaRelacionadasAsync(long id, List<MultasProcedimentosPortariaRelacionadaUpdate> novos);
        Task<bool> DeleteAsync(long id);
        Task<MultasProcedimentosResponse?> GetWithChildrenBySlugAtivoAsync(string slug);
        Task<bool> DisableAsync(long id);
    }
}
