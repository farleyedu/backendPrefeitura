using Microsoft.EntityFrameworkCore;
using PortalTCMSP.Domain.DTOs.Requests.Noticia;
using PortalTCMSP.Domain.Entities.NoticiaEntity;
using PortalTCMSP.Domain.Helper;
using PortalTCMSP.Domain.Repositories.Noticia;
using PortalTCMSP.Infra.Data.Context;
using PortalTCMSP.Infra.Data.Repositories.Home;

namespace PortalTCMSP.Infra.Data.Repositories.Noticia
{
    public class NoticiaOldRepository : BaseRepository<NoticiaOld>, INoticiaOldRepository
    {
        private readonly PortalTCMSPContext _context;
        public NoticiaOldRepository(PortalTCMSPContext context) : base(context) => _context = context;

        public IEnumerable<NoticiaOld> Search(NoticiaOldListarRequest request)
        {
            var q = _context.NoticiaOld
                .AsNoTracking()
                .AsQueryable();

            return [.. q
        .OrderByDescending(n => n.Destaque)
        .ThenByDescending(n => n.DataPublicacao)];
        }

        public async Task<NoticiaOld?> GetByIdAsync(long id, CancellationToken ct = default)
        {
            return await _context.NoticiaOld
                .AsNoTracking()
                .FirstOrDefaultAsync(n => n.Id == id, ct);
        }
        public async Task<NoticiaOld?> GetBySlugAsync(string slug, CancellationToken ct = default)
        {
            return await _context.NoticiaOld
                .AsNoTracking()
                .FirstOrDefaultAsync(n => n.Slug == slug, ct);
        }
    }
}
