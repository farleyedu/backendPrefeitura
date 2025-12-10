using Microsoft.AspNetCore.Mvc;
using PortalTCMSP.Domain.DTOs.Requests.Servicos.OficioseIntimacoes;
using PortalTCMSP.Domain.DTOs.Responses.Servicos.OficioseIntimacoes;
using PortalTCMSP.Domain.Services.Servicos.OficioseIntimacoes;

namespace PortalTCMSP.Api.Controllers.Servicos
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "Servicos")]
    public class OficioseIntimacoesController(IOficioseIntimacoesService service) : ControllerBase
    {
        private readonly IOficioseIntimacoesService _service = service;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OficioseIntimacoesResponse>>> GetAll()
            => Ok(await _service.GetAllAsync());

        [HttpGet("{id:long}")]
        public async Task<ActionResult<OficioseIntimacoesResponse>> GetById(long id)
        {
            var item = await _service.GetByIdAsync(id);
            return item is null ? NotFound() : Ok(item);
        }

        [HttpGet("slug-ativo/{slug}")]
        [ProducesResponseType(typeof(OficioseIntimacoesResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OficioseIntimacoesResponse>> GetBySlugActive(string slug)
        {
            var item = await _service.GetWithChildrenBySlugAtivoAsync(slug);
            return item is null ? NotFound() : Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult<long>> Create([FromBody] OficioseIntimacoesCreateRequest request)
        {
            var id = await _service.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id }, id);
        }

        [HttpPut("{id:long}")]
        public async Task<IActionResult> Update(long id, [FromBody] OficioseIntimacoesUpdateRequest request)
            => (await _service.UpdateAsync(id, request)) ? NoContent() : NotFound();

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Excluir(long id)
            => (await _service.DeleteAsync(id)) ? NoContent() : NotFound();

        [HttpPatch("{id:long}/secoes/create")]
        public async Task<IActionResult> CreateSecoes(long id, [FromBody] List<OficioseIntimacoesSecaoRequest> req)
        {
            if (req == null || req.Count == 0) return BadRequest("Nenhuma seção fornecida.");
            return await _service.CreateSecoesAsync(id, req) ? NoContent() : NotFound();
        }

        [HttpPatch("{id:long}/secoes/update")]
        public async Task<IActionResult> UpdateSecoes(long id, [FromBody] List<OficioseIntimacoesSecaoUpdate> req)
        {
            if (req == null || req.Count == 0) return BadRequest("Nenhuma seção fornecida.");
            return await _service.UpdateSecoesAsync(id, req) ? NoContent() : NotFound();
        }

        [HttpPatch("secao/{idSecao:long}/secoes-itens/create")]
        public async Task<IActionResult> CreateSecaoItens(long idSecao, [FromBody] List<OficioseIntimacoesSecaoItemRequest> req)
        {
            if (req == null || req.Count == 0) return BadRequest("Nenhum ítem fornecido.");
            return await _service.CreateSecaoItensAsync(idSecao, req) ? NoContent() : NotFound();
        }

        [HttpPatch("secao/{idSecao:long}/secoes-itens/update")]
        public async Task<IActionResult> UpdateSecaoItens(long idSecao, [FromBody] List<OficioseIntimacoesSecaoItemUpdate> req)
        {
            if (req == null || req.Count == 0) return BadRequest("Nenhum ítem fornecido.");
            return await _service.UpdateSecaoItensAsync(idSecao, req) ? NoContent() : NotFound();
        }
    }
}
