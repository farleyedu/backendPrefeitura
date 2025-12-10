using PortalTCMSP.Domain.Repositories.Home;

namespace PortalTCMSP.Domain.Repositories.SessaoPlenaria
{
    public interface ISessaoPlenariaRepository : IBaseRepository<Entities.SessaoPlenariaEntity.SessaoPlenaria>
    {
        Task<Entities.SessaoPlenariaEntity.SessaoPlenaria?> FindBySlugAsync(string slug);
        Task<Entities.SessaoPlenariaEntity.SessaoPlenaria?> GetWithChildrenByIdAsync(long id);
        Task<Entities.SessaoPlenariaEntity.SessaoPlenaria?> GetWithChildrenBySlugAsync(string slug);
        Task<List<Entities.SessaoPlenariaEntity.SessaoPlenaria>> AllWithChildrenAsync();
        Task DeactivateAllAsync();
        Task DeactivateAllExceptAsync(long exceptId);
        Task<List<Entities.SessaoPlenariaEntity.SessaoPlenaria>> GetAtivosAsync();
        Task<Entities.SessaoPlenariaEntity.SessaoPlenaria?> GetAtivaWithChildrenAsync();
    }
}
