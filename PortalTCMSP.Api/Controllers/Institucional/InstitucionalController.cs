using Microsoft.AspNetCore.Mvc;
using PortalTCMSP.Domain.Helper;
using PortalTCMSP.Domain.Services.Institucional;

namespace PortalTCMSP.Api.Controllers.Institucional
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "Institucional")]
    public class InstitucionalController : ControllerBase
    {
        private readonly IInstitucionalService _service;

        public InstitucionalController(IInstitucionalService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Search([FromQuery] string? Ativo, [FromQuery] DateTime? publicadoAte, CancellationToken ct)
        {
            var list = await _service.SearchAsync(Ativo, publicadoAte, ct);
            return Ok(list);
        }

        [HttpGet("slug/{slug}")]
        public async Task<IActionResult> GetBySlug(string slug, CancellationToken ct)
        {
            var ent = await _service.GetBySlugAsync(SlugHelper.Slugify(slug), ct);
            if (ent is null) return NotFound();
            return Ok(ent);
        }

    }
}
