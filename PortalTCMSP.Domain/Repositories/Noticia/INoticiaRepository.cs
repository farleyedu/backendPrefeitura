using PortalTCMSP.Domain.DTOs.Requests.NoticiaRequest;
using PortalTCMSP.Domain.Repositories.Home;

namespace PortalTCMSP.Domain.Repositories.Noticia
{
    public interface INoticiaRepository : IBaseRepository<PortalTCMSP.Domain.Entities.NoticiaEntity.Noticia>
    {
        IEnumerable<PortalTCMSP.Domain.Entities.NoticiaEntity.Noticia> Search(NoticiaListarRequest request);
        Task<PortalTCMSP.Domain.Entities.NoticiaEntity.Noticia?> ObterPorSlugAsync(string slug);
        Task AdicionarAsync(PortalTCMSP.Domain.Entities.NoticiaEntity.Noticia noticia);
        Task<PortalTCMSP.Domain.Entities.NoticiaEntity.Noticia?> ObterPorIdComBlocosAsync(int id);
        Task<string> GerarSlugUnicoAsync(string slugDesejado);
        Task<PortalTCMSP.Domain.Entities.NoticiaEntity.Noticia?> ObterPorIdCompletoAsync(int id);
    }
}
