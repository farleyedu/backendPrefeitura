using Microsoft.AspNetCore.Mvc;
using PortalTCMSP.Domain.DTOs.Requests.Institucional.Iso9001;
using PortalTCMSP.Domain.Helper;
using PortalTCMSP.Domain.Services.Institucional;

namespace PortalTCMSP.Api.Controllers.Institucional
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "Institucional")]
    public class Iso9001Controller : ControllerBase
    {
        private readonly IIso9001Service _service;
        private readonly ILogger<Iso9001Controller> _logger;

        public Iso9001Controller(IIso9001Service service, ILogger<Iso9001Controller> logger)
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
        public async Task<IActionResult> Create([FromBody] CreateIso9001Request request, CancellationToken ct)
        {
            var id = await _service.CreateAsync(request, ct);
            return CreatedAtAction(nameof(GetById), new { id }, new { id });
        }

        [HttpPut("{id:long}")]
        public async Task<IActionResult> Update(long id, [FromBody] UpdateIso9001Request request, CancellationToken ct)
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
