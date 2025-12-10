using PortalTCMSP.Domain.DTOs.Responses.Institucional.Base;
using PortalTCMSP.Domain.DTOs.Responses.Institucional.Competencias;
using PortalTCMSP.Domain.Mappings.Institucional;
using PortalTCMSP.Domain.Repositories.Institucional;
using PortalTCMSP.Domain.Services.Institucional;
using PortalTCMSP.Infra.Data.Context;

namespace PortalTCMSP.Infra.Services.Institucional
{
    public class InstitucionalService : IInstitucionalService
    {
        private readonly IInstitucionalRepository _repo;
        private readonly PortalTCMSPContext _ctx;

        public InstitucionalService(IInstitucionalRepository repo, PortalTCMSPContext ctx)
        {
            _repo = repo;
            _ctx = ctx;
        }

        public async Task<List<InstitucionalResponse>> SearchAsync(string? ativo, DateTime? publicadoAte, CancellationToken ct = default)
        {
            var list = await _repo.SearchAsync(ativo, publicadoAte, ct);
            return list.ToResponse(); 
        }

        public async Task<CompetenciasResponse?> GetBySlugAsync(string slug, CancellationToken ct = default)
        {
            var e = await _repo.GetBySlugWithTreeAsync(slug, ct);
            return e is null ? null : e.ToResponse().ToCompetenciasResponse();
        }
    }
}
