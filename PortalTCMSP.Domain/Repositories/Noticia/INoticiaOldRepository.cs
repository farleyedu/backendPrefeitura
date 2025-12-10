using PortalTCMSP.Domain.DTOs.Requests.Noticia;
using PortalTCMSP.Domain.Entities.NoticiaEntity;
using PortalTCMSP.Domain.Repositories.Home;

namespace PortalTCMSP.Domain.Repositories.Noticia
{
    public interface INoticiaOldRepository : IBaseRepository<NoticiaOld>
    {
        IEnumerable<NoticiaOld> Search(NoticiaOldListarRequest request);
        Task<NoticiaOld?> GetByIdAsync(long id, CancellationToken ct = default);
        Task<NoticiaOld?> GetBySlugAsync(string slug, CancellationToken ct = default);
    }
}
