using PortalTCMSP.Domain.DTOs.Requests.SessaoPlenaria.Ata;
using PortalTCMSP.Domain.Entities.SessaoPlenariaEntity;
using PortalTCMSP.Domain.Repositories.Home;

namespace PortalTCMSP.Domain.Repositories.SessaoPlenaria
{
    public interface ISessaoPlenariaNotasTaquigraficasRepository : IBaseRepository<SessaoPlenariaNotasTaquigraficas>
    {
        Task<SessaoPlenariaNotasTaquigraficas?> GetWithAnexosByIdAsync(long id);
        Task<List<SessaoPlenariaNotasTaquigraficas>> AllWithAnexosAsync();
        Task ReplaceAnexosAsync(long notasId, IEnumerable<SessaoPlenariaNotasTaquigraficasAnexos> novos);
        Task<IEnumerable<SessaoPlenariaNotasTaquigraficas>> Search(SessaoPlenariaNotasTaquigraficasSearchRequest request);
    }
}
