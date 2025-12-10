using PortalTCMSP.Domain.DTOs.Requests.Institucional.Competencias;
using PortalTCMSP.Domain.DTOs.Responses.Institucional.Competencias;
using PortalTCMSP.Domain.Helper;
using PortalTCMSP.Infra.Data.Context;
using Microsoft.EntityFrameworkCore;
using PortalTCMSP.Domain.Services.Institucional;
using PortalTCMSP.Domain.Repositories.Institucional;
using PortalTCMSP.Domain.Mappings.Institucional;

namespace PortalTCMSP.Infra.Services.Institucional
{
    public class CompetenciasService : ICompetenciasService
    {
        private readonly IInstitucionalRepository _repo;
        private readonly PortalTCMSPContext _ctx;

        public CompetenciasService(IInstitucionalRepository repo, PortalTCMSPContext ctx)
        {
            _repo = repo;
            _ctx = ctx;
        }

        public async Task<CompetenciasResponse?> GetAsync(long id, CancellationToken ct = default)
        {
            var e = await _repo.GetByIdWithTreeAsync(id, ct);
            return e is null ? null : e.ToResponse().ToCompetenciasResponse();
        }

        public async Task<CompetenciasResponse?> GetBySlugAsync(string slug, CancellationToken ct = default)
        {
            var e = await _repo.GetBySlugWithTreeAsync(slug, ct);
            return e is null ? null : e.ToResponse().ToCompetenciasResponse();
        }

        public async Task<List<CompetenciasResponse>> SearchAsync(string? ativo, DateTime? publicadoAte, CancellationToken ct = default)
        {
            var list = await _repo.SearchAsync(ativo, publicadoAte, ct);
            return list.Select(x => x.ToResponse()).ToCompetenciasResponse();
        }

        public async Task<long> CreateAsync(CreateCompetenciasRequest req, CancellationToken ct = default)
        {
            var instCreate = req.ToInstitucionalCreate();

            // slug único (gera do título se vier vazio)
            var desired = string.IsNullOrWhiteSpace(instCreate.Slug) ? req.Titulo : instCreate.Slug;
            instCreate.Slug = await EnsureUniqueSlugAsync(desired, null, ct);

            var ent = instCreate.ToEntity();
            await _ctx.Set<PortalTCMSP.Domain.Entities.InstitucionalEntity.Institucional>().AddAsync(ent, ct);
            await _ctx.SaveChangesAsync(ct);
            return ent.Id;
        }

        public async Task UpdateAsync(long id, UpdateCompetenciasRequest req, CancellationToken ct = default)
        {
            var current = await _ctx.Set<PortalTCMSP.Domain.Entities.InstitucionalEntity.Institucional>()
                .Include(i => i.Blocos).ThenInclude(b => b.Descricoes)
                .Include(i => i.Blocos).ThenInclude(b => b.Anexos)
                .FirstOrDefaultAsync(i => i.Id == id, ct);

            if (current is null) throw new KeyNotFoundException($"Institucional {id} não encontrado.");

            var instUpdate = req.ToInstitucionalUpdate();

            if (!string.IsNullOrWhiteSpace(instUpdate.Slug))
                instUpdate.Slug = await EnsureUniqueSlugAsync(instUpdate.Slug, current.Id, ct);
            else
                instUpdate.Slug = current.Slug;

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
    }
}
