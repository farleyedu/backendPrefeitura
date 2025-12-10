using Microsoft.EntityFrameworkCore;
using PortalTCMSP.Domain.Repositories.Institucional;
using PortalTCMSP.Infra.Data.Context;
using PortalTCMSP.Infra.Data.Repositories.Home;

namespace PortalTCMSP.Infra.Data.Repositories.Institucional
{
    public class InstitucionalRepository : BaseRepository<PortalTCMSP.Domain.Entities.InstitucionalEntity.Institucional>, IInstitucionalRepository
    {
        private readonly PortalTCMSPContext _context;
        public InstitucionalRepository(PortalTCMSPContext context) : base(context)
        {
            _context = context;
        }

        public async Task<PortalTCMSP.Domain.Entities.InstitucionalEntity.Institucional?> GetByIdWithTreeAsync(long id, CancellationToken ct = default)
        {
            return await _context.Set<PortalTCMSP.Domain.Entities.InstitucionalEntity.Institucional>()
                .AsNoTracking()
                .Include(i => i.Blocos.OrderBy(b => b.Ordem))
                .ThenInclude(b => b.Descricoes.OrderBy(d => d.Ordem))
                .ThenInclude(d => d.Subtextos)
                .Include(i => i.Blocos)
                .ThenInclude(b => b.Anexos.OrderBy(a => a.Ordem))
                .FirstOrDefaultAsync(i => i.Id == id, ct);
        }

        public async Task<List<PortalTCMSP.Domain.Entities.InstitucionalEntity.Institucional>> SearchAsync(string? ativo, DateTime? publicadoAte, CancellationToken ct = default)
        {
            var q = _context.Set<PortalTCMSP.Domain.Entities.InstitucionalEntity.Institucional>().AsNoTracking();

            if (!string.IsNullOrWhiteSpace(ativo)) q = q.Where(i => i.Ativo == ativo);
            if (publicadoAte.HasValue) q = q.Where(i => i.DataPublicacao != null && i.DataPublicacao <= publicadoAte);

            return await q
                .OrderByDescending(i => i.DataPublicacao ?? i.DataCriacao)
                .ToListAsync(ct);
        }

        public Task<PortalTCMSP.Domain.Entities.InstitucionalEntity.Institucional?> GetBySlugWithTreeAsync(string slug, CancellationToken ct = default)
        {
            return _context.Set<PortalTCMSP.Domain.Entities.InstitucionalEntity.Institucional>()
                .AsNoTracking()
                .Include(i => i.Blocos).ThenInclude(b => b.Descricoes).ThenInclude(d => d.Subtextos)
                .Include(i => i.Blocos).ThenInclude(b => b.Anexos)
                .FirstOrDefaultAsync(i => i.Slug == slug, ct);
        }
    }
}
