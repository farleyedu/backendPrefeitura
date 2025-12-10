using PortalTCMSP.Domain.DTOs.Requests.SessaoPlenaria.Pauta;
using PortalTCMSP.Domain.Entities.SessaoPlenariaEntity;
using PortalTCMSP.Domain.Repositories.Home;

namespace PortalTCMSP.Domain.Repositories.SessaoPlenaria
{
    public interface ISessaoPlenariaPautaRepository : IBaseRepository<SessaoPlenariaPauta>
    {
        Task<SessaoPlenariaPauta?> GetWithAnexosByIdAsync(long id);
        Task<List<SessaoPlenariaPauta>> AllWithAnexosAsync();
        Task ReplaceAnexosAsync(long pautaId, IEnumerable<SessaoPlenariaPautaAnexo> novos);
        Task<IEnumerable<SessaoPlenariaPauta>> Search(SessaoPlenariaPautaSearchRequest request);
    }
}
