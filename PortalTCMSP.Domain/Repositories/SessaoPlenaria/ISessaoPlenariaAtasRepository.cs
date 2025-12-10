using PortalTCMSP.Domain.DTOs.Requests.SessaoPlenaria.Ata;
using PortalTCMSP.Domain.Entities.SessaoPlenariaEntity;
using PortalTCMSP.Domain.Repositories.Home;

namespace PortalTCMSP.Domain.Repositories.SessaoPlenaria
{
    public interface ISessaoPlenariaAtasRepository : IBaseRepository<SessaoPlenariaAta>
    {
        Task<SessaoPlenariaAta?> GetWithAnexosByIdAsync(long id);
        Task<List<SessaoPlenariaAta>> AllWithAnexosAsync();
        Task ReplaceAnexosAsync(SessaoPlenariaAta parent, IEnumerable<SessaoPlenariaAtaAnexo> novos);
        Task<IEnumerable<SessaoPlenariaAta>> Search(SessaoPlenariaAtaSearchRequest request);
        Task<bool> ExistsSessaoPlenariaAsync(long id);
    }
}
