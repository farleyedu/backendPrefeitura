using Microsoft.EntityFrameworkCore;
using PortalTCMSP.Domain.DTOs.Requests.Institucional.Colegiado;
using PortalTCMSP.Domain.DTOs.Responses.Institucional.Colegiado;
using PortalTCMSP.Domain.Helper;
using PortalTCMSP.Domain.Mappings.Institucional;
using PortalTCMSP.Domain.Repositories.Institucional;
using PortalTCMSP.Domain.Services.Institucional;
using PortalTCMSP.Infra.Data.Context;

namespace PortalTCMSP.Infra.Services.Institucional
{
    public class ColegiadoService : IColegiadoService
    {
        private readonly IInstitucionalRepository _repo;
        private readonly PortalTCMSPContext _ctx;

        public ColegiadoService(IInstitucionalRepository repo, PortalTCMSPContext ctx)
        {
            _repo = repo;
            _ctx = ctx;
        }

        public async Task<ColegiadoResponse?> GetBySlugAsync(string slug, CancellationToken ct = default)
        {
            var e = await _repo.GetBySlugWithTreeAsync(slug, ct);
            return e is null ? null : e.ToResponse().ToColegiadoResponse();
        }

        private async Task<string> EnsureUniqueSlugAsync(string desired, long? excludeId, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(desired)) desired = "pagina";
            desired = SlugHelper.Slugify(desired);
            var baseSlug = desired;
            var i = 0;

            while (true)
            {
                var candidate = i == 0 ? baseSlug : $"{baseSlug}-{i}";
                var exists = await _ctx.Set<PortalTCMSP.Domain.Entities.InstitucionalEntity.Institucional>()
                    .AsNoTracking()
                    .AnyAsync(x => x.Slug == candidate && (!excludeId.HasValue || x.Id != excludeId.Value), ct);
                if (!exists) return candidate;
                i++;
            }
        }

        public async Task<ColegiadoResponse?> GetAsync(long id, CancellationToken ct = default)
        {
            var e = await _repo.GetByIdWithTreeAsync(id, ct);
            if (e is null) return null;

            var instResp = e.ToResponse();
            return instResp.ToColegiadoResponse();
        }

        public async Task<List<ColegiadoResponse>> SearchAsync(string? ativo, DateTime? publicadoAte, CancellationToken ct = default)
        {
            var entities = await _repo.SearchAsync(ativo, publicadoAte, ct);
            var instList = entities.Select(x => x.ToResponse()).ToList();
            return instList.ToColegiadoResponse();
        }

        public async Task<long> CreateAsync(CreateColegiadoRequest req, CancellationToken ct = default)
        {
            var instCreate = req.ToInstitucionalCreate();

            var desired = string.IsNullOrWhiteSpace(instCreate.Slug) ? req.Titulo : instCreate.Slug;
            instCreate.Slug = await EnsureUniqueSlugAsync(desired, null, ct);

            var ent = instCreate.ToEntity();
            await _ctx.Set<PortalTCMSP.Domain.Entities.InstitucionalEntity.Institucional>().AddAsync(ent, ct);
            await _ctx.SaveChangesAsync(ct);
            return ent.Id;
        }

        public async Task UpdateAsync(long id, UpdateColegiadoRequest req, CancellationToken ct = default)
        {
            var current = await _ctx.Set<PortalTCMSP.Domain.Entities.InstitucionalEntity.Institucional>()
                .Include(i => i.Blocos).ThenInclude(b => b.Descricoes)
                .Include(i => i.Blocos).ThenInclude(b => b.Anexos)
                .FirstOrDefaultAsync(i => i.Id == id, ct);

            if (current is null) throw new KeyNotFoundException($"Institucional {id} não encontrado.");

            var instUpdate = req.ToInstitucionalUpdate();

            if (!string.IsNullOrWhiteSpace(instUpdate.Slug))
            {
                var desired = instUpdate.Slug;
                instUpdate.Slug = await EnsureUniqueSlugAsync(desired, current.Id, ct);
            }
            else
            {
                instUpdate.Slug = current.Slug;
            }

            var diff = current.MapUpdate(instUpdate);

            foreach (var r in diff.AnexosToRemove) _ctx.Remove(r);
            foreach (var r in diff.DescricoesToRemove) _ctx.Remove(r);
            foreach (var r in diff.BlocosToRemove) _ctx.Remove(r);

            await _ctx.SaveChangesAsync(ct);
        }

        public async Task SoftDeleteAsync(long id, CancellationToken ct = default)
        {
            var current = await _ctx.Set<PortalTCMSP.Domain.Entities.InstitucionalEntity.Institucional>().FirstOrDefaultAsync(i => i.Id == id, ct);
            if (current is null) return;
            current.Ativo = "N";
            current.DataAtualizacao = DateTime.UtcNow;
            await _ctx.SaveChangesAsync(ct);
        }
    }
}
