using PortalTCMSP.Domain.Entities.ServicosEntity.MultasProcedimentosEntity;
using PortalTCMSP.Domain.Repositories.Home;

namespace PortalTCMSP.Domain.Repositories.Servicos.MultasProcedimentos
{
    public interface IMultasProcedimentosRepository : IBaseRepository<Entities.ServicosEntity.MultasProcedimentosEntity.MultasProcedimentos>
    {
        Task<Entities.ServicosEntity.MultasProcedimentosEntity.MultasProcedimentos?> GetWithChildrenByIdAsync(long id);
        Task<List<Entities.ServicosEntity.MultasProcedimentosEntity.MultasProcedimentos>> AllWithChildrenAsync();
        Task<bool> DisableAsync(long id);
        Task<Entities.ServicosEntity.MultasProcedimentosEntity.MultasProcedimentos?> GetWithChildrenBySlugAtivoAsync(string slug);
        Task<Entities.ServicosEntity.MultasProcedimentosEntity.MultasProcedimentos?> GetBySlugAtivoAsync(string slug);
        Task ReplaceProcedimentosAsync(long id, IEnumerable<MultasProcedimentosProcedimento> novos);
        Task ReplacePortariasAsync(long id, IEnumerable<MultasProcedimentosPortariaRelacionada> novas);
    }
}
