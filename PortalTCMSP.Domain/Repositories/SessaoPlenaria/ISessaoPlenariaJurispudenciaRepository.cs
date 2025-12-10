using PortalTCMSP.Domain.Entities.SessaoPlenariaEntity;
using PortalTCMSP.Domain.Repositories.Home;

namespace PortalTCMSP.Domain.Repositories.SessaoPlenaria
{
    public interface ISessaoPlenariaJurispudenciaRepository : IBaseRepository<SessaoPlenariaJurispudencia>
    {
        Task<SessaoPlenariaJurispudencia?> FindBySlugAsync(string slug);
        Task<SessaoPlenariaJurispudencia?> GetAtivaAsync();
        Task<int> DesativarTodosExcetoAsync(long? idMantidoAtivo);
    }
}
