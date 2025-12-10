using Microsoft.AspNetCore.Mvc;
using PortalTCMSP.Domain.DTOs.Requests.Servicos.ProtocoloEletronico;
using PortalTCMSP.Domain.DTOs.Responses.Servicos.ProtocoloEletronico;
using PortalTCMSP.Domain.Services.Servicos.ProtocoloEletronico;

namespace PortalTCMSP.Api.Controllers.Servicos
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "Servicos")]
    public class ProtocoloEletronicoController : ControllerBase
    {
        private readonly IProtocoloEletronicoService _service;

        public ProtocoloEletronicoController(IProtocoloEletronicoService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id:long}")]
        public async Task<IActionResult> GetByIdAsync(long id)
        {
            var result = await _service.GetByIdAsync(id);
            if (result is null) return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromBody] ProtocoloEletronicoCreateRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var id = await _service.CreateAsync(request);
            return Created($"/api/protocoloeletronico/{id}", new { id });
        }

        [HttpPut("{id:long}")]
        public async Task<IActionResult> UpdateAsync(long id, [FromBody] ProtocoloEletronicoUpdateRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var updated = await _service.UpdateAsync(id, request);
            return updated ? NoContent() : NotFound();
        }

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> DeleteAsync(long id)
        {
            var deleted = await _service.DeleteAsync(id);
            return deleted ? NoContent() : NotFound();
        }

        [HttpGet("slug-ativo/{slug}")]
        [ProducesResponseType(typeof(ProtocoloEletronicoResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProtocoloEletronicoResponse>> GetBySlugActive(string slug)
        {
            var item = await _service.GetWithChildrenBySlugAtivoAsync(slug);
            return item is null ? NotFound() : Ok(item);
        }

        [HttpPatch("{id:long}/acoes/create")]
        public async Task<IActionResult> CreateAcoesAsync(long id, [FromBody] List<ProtocoloEletronicoAcaoRequest> acoes)
        {
            if (acoes == null || acoes.Count == 0) return BadRequest("Nenhuma ação fornecida.");
            var created = await _service.CreateAcoesAsync(id, acoes);
            return created ? NoContent() : NotFound();
        }

        [HttpPatch("{id:long}/acoes/update")]
        public async Task<IActionResult> UpdateAcoesAsync(long id, [FromBody] List<ProtocoloEletronicoAcaoUpdate> acoes)
        {
            if (acoes == null || acoes.Count == 0) return BadRequest("Nenhuma ação fornecida.");
            var updated = await _service.UpdateAcoesAsync(id, acoes);
            return updated ? NoContent() : NotFound();
        }
    }
}
