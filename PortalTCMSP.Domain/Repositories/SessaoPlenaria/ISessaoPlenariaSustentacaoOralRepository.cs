using PortalTCMSP.Domain.Entities.SessaoPlenariaEntity;
using PortalTCMSP.Domain.Repositories.Home;

namespace PortalTCMSP.Domain.Repositories.SessaoPlenaria
{
    public interface ISessaoPlenariaSustentacaoOralRepository : IBaseRepository<SessaoPlenariaSustentacaoOral>
    {
        Task<SessaoPlenariaSustentacaoOral?> FindBySlugAsync(string slug);
        Task<SessaoPlenariaSustentacaoOral?> GetAtivaWithAnexosAsync();
        Task<SessaoPlenariaSustentacaoOral?> GetWithAnexosByIdAsync(long id);
        Task<SessaoPlenariaSustentacaoOral?> GetWithAnexosBySlugAsync(string slug);
        Task<List<SessaoPlenariaSustentacaoOral>> AllWithAnexosAsync();
        Task<int> DesativarTodosExcetoAsync(long? idMantidoAtivo);
        Task ReplaceAnexosAsync(long sustentacaoId, IEnumerable<SessaoPlenariaSustentacaoOralAnexos> novos);
    }
}
