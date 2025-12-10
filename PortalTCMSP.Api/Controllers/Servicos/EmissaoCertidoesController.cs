using Microsoft.AspNetCore.Mvc;
using PortalTCMSP.Domain.DTOs.Requests.Servicos.EmissaoCertidoes;
using PortalTCMSP.Domain.DTOs.Responses.Servicos.EmissaoCertidoes;
using PortalTCMSP.Domain.Services.Servicos.EmissaoCertidoes;

namespace PortalTCMSP.Api.Controllers.Servicos
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "Servicos")]
    public class EmissaoCertidoesController(IEmissaoCertidoesService service) : ControllerBase
    {
        private readonly IEmissaoCertidoesService _service = service;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmissaoCertidoesResponse>>> GetAll() => Ok(await _service.GetAllAsync());

        [HttpGet("{id:long}")]
        public async Task<ActionResult<EmissaoCertidoesResponse>> GetById(long id)
        {
            var item = await _service.GetByIdAsync(id);
            return item is null ? NotFound() : Ok(item);
        }

        [HttpGet("slug-ativo/{slug}")]
        public async Task<ActionResult<EmissaoCertidoesResponse>> GetBySlugActive(string slug)
        {
            var item = await _service.GetWithChildrenBySlugAtivoAsync(slug);
            return item is null ? NotFound() : Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult<long>> Create([FromBody] EmissaoCertidoesCreateRequest request)
        {
            var id = await _service.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id }, id);
        }

        [HttpPut("{id:long}")]
        public async Task<IActionResult> Update(long id, [FromBody] EmissaoCertidoesUpdateRequest request)
            => (await _service.UpdateAsync(id, request)) ? NoContent() : NotFound();

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id) => (await _service.DeleteAsync(id)) ? NoContent() : NotFound();

        [HttpPatch("{id:long}/acoes/create")]
        public async Task<IActionResult> CreateAcoes(long id, [FromBody] List<EmissaoCertidoesAcaoRequest> req)
        {
            if (req == null || req.Count == 0) return BadRequest("Nenhuma ação fornecida.");
            return await _service.CreateAcoesAsync(id, req) ? NoContent() : NotFound();
        }

        [HttpPatch("{id:long}/acoes/update")]
        public async Task<IActionResult> UpdateAcoes(long id, [FromBody] List<EmissaoCertidoesAcaoUpdate> req)
        {
            if (req == null || req.Count == 0) return BadRequest("Nenhuma ação fornecida.");
            return await _service.UpdateAcoesAsync(id, req) ? NoContent() : NotFound();
        }

        [HttpPatch("{id:long}/secoes/create")]
        public async Task<IActionResult> CreateSecoes(long id, [FromBody] List<EmissaoCertidoesSecaoOrientacaoRequest> req)
        {
            if (req == null || req.Count == 0) return BadRequest("Nenhuma seção fornecida.");
            return await _service.CreateSecoesAsync(id, req) ? NoContent() : NotFound();
        }

        [HttpPatch("{id:long}/secoes/update")]
        public async Task<IActionResult> UpdateSecoes(long id, [FromBody] List<EmissaoCertidoesSecaoOrientacaoUpdate> req)
        {
            if (req == null || req.Count == 0) return BadRequest("Nenhuma seção fornecida.");
            return await _service.UpdateSecoesAsync(id, req) ? NoContent() : NotFound();
        }
    }
}
