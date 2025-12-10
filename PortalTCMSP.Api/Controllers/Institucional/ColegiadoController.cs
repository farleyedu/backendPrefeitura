using Microsoft.AspNetCore.Mvc;
using PortalTCMSP.Domain.DTOs.Requests.Institucional.Colegiado;
using PortalTCMSP.Domain.Helper;
using PortalTCMSP.Domain.Services.Institucional;

namespace PortalTCMSP.Api.Controllers.Institucional
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "Institucional")]
    public class ColegiadoController : ControllerBase
    {
        private readonly IColegiadoService _colegiadoService;
        private readonly ILogger<ColegiadoController> _logger;

        public ColegiadoController(IColegiadoService colegiadoService, ILogger<ColegiadoController> logger)
        {
            _colegiadoService = colegiadoService;
            _logger = logger;
        }
        [HttpGet("slug/{slug}")]
        public async Task<IActionResult> GetBySlug(string slug, CancellationToken ct)
        {
            var ent = await _colegiadoService.GetBySlugAsync(SlugHelper.Slugify(slug), ct);
            if (ent is null) return NotFound();
            return Ok(ent);
        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetById(long id, CancellationToken ct)
        {
            var colegiado = await _colegiadoService.GetAsync(id, ct);
            return colegiado is null ? NotFound() : Ok(colegiado);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateColegiadoRequest request, CancellationToken ct)
        { 
            var id = await _colegiadoService.CreateAsync(request, ct);
            return CreatedAtAction(nameof(GetById), new { id }, new { id });
        }

        [HttpPut("{id:long}")]
        public async Task<IActionResult> Update(long id, [FromBody] UpdateColegiadoRequest request, CancellationToken ct)
        {// <— extensão
            await _colegiadoService.UpdateAsync(id, request, ct);
            return NoContent();
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> SoftDelete(long id, CancellationToken ct)
        {
            await _colegiadoService.SoftDeleteAsync(id, ct);
            return NoContent();
        }
    }
}
