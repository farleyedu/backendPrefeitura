using Microsoft.AspNetCore.Mvc;
using PortalTCMSP.Domain.DTOs.Requests.Institucional.Historia;
using PortalTCMSP.Domain.DTOs.Requests.Institucional.Historia.PortalTCMSP.Domain.DTOs.Requests.Institucional.Historia;
using PortalTCMSP.Domain.Helper;
using PortalTCMSP.Domain.Services.Institucional;

namespace PortalTCMSP.Api.Controllers.Institucional
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "Institucional")]
    public class HistoriaController : ControllerBase
    {
        private readonly IHistoriaService _service;
        private readonly ILogger<HistoriaController> _logger;

        public HistoriaController(IHistoriaService service, ILogger<HistoriaController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetById(long id, CancellationToken ct)
        {
            var r = await _service.GetAsync(id, ct);
            return r is null ? NotFound() : Ok(r);
        }

        [HttpGet("slug/{slug}")]
        public async Task<IActionResult> GetBySlug(string slug, CancellationToken ct)
        {
            var r = await _service.GetBySlugAsync(SlugHelper.Slugify(slug), ct);
            return r is null ? NotFound() : Ok(r);
        }

        [HttpGet]
        public async Task<IActionResult> Search([FromQuery] string? Ativo, [FromQuery] DateTime? publicadoAte, CancellationToken ct)
        {
            var list = await _service.SearchAsync(Ativo, publicadoAte, ct);
            return Ok(list);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateHistoriaRequest request, CancellationToken ct)
        {
            var id = await _service.CreateAsync(request, ct);
            return CreatedAtAction(nameof(GetById), new { id }, new { id });
        }

        [HttpPut("{id:long}")]
        public async Task<IActionResult> Update(long id, [FromBody] UpdateHistoriaRequest request, CancellationToken ct)
        {
            await _service.UpdateAsync(id, request, ct);
            return NoContent();
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> SoftDelete(long id, CancellationToken ct)
        {
            await _service.SoftDeleteAsync(id, ct);
            return NoContent();
        }
    }
}
