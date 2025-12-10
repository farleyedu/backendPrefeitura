using Microsoft.AspNetCore.Mvc;
using PortalTCMSP.Domain.DTOs.Requests.Servicos.PrazosProcessuais;
using PortalTCMSP.Domain.DTOs.Responses.Servicos.PrazosProcessuais;
using PortalTCMSP.Domain.Services.Servicos.PrazosProcessuais;

namespace PortalTCMSP.Api.Controllers.Servicos
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "Servicos")]
    public class PrazosProcessuaisController(IPrazosProcessuaisService service) : ControllerBase
    {
        private readonly IPrazosProcessuaisService _service = service;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PrazosProcessuaisResponse>>> GetAll()
            => Ok(await _service.GetAllAsync());

        [HttpGet("{id:long}")]
        public async Task<ActionResult<PrazosProcessuaisResponse>> GetById(long id)
        {
            var item = await _service.GetByIdAsync(id);
            return item is null ? NotFound() : Ok(item);
        }

        [HttpGet("slug-ativo/{slug}")]
        public async Task<ActionResult<PrazosProcessuaisResponse>> GetBySlugActive(string slug)
        {
            var item = await _service.GetWithChildrenBySlugAtivoAsync(slug);
            return item is null ? NotFound() : Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult<long>> Create([FromBody] PrazosProcessuaisCreateRequest request)
        {
            var id = await _service.CreateAsync(request);
            return Created($"/api/prazosprocessuais/{id}", new { id });
        }

        [HttpPut("{id:long}")]
        public async Task<IActionResult> Update(long id, [FromBody] PrazosProcessuaisCreateRequest request)
            => (await _service.UpdateAsync(id, request)) ? NoContent() : NotFound();

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id)
            => (await _service.DeleteAsync(id)) ? NoContent() : NotFound();

        [HttpPatch("{id:long}/prazos-processuais-itens/create")]
        public async Task<IActionResult> CreatePrazosProcessuaisItens(long id, [FromBody] List<PrazosProcessuaisItemRequest> req)
        {
            if (req == null || req.Count == 0) return BadRequest("Nenhum item fornecido.");
            return await _service.CreatePrazosProcessuaisItemAsync(id, req) ? NoContent() : NotFound();
        }

        [HttpPatch("{id:long}/prazos-processuais-itens/update")]
        public async Task<IActionResult> UpdatePrazosProcessuaisItens(long id, [FromBody] List<PrazosProcessuaisItemUpdate> req)
        {
            if (req == null || req.Count == 0) return BadRequest("Nenhum item fornecido.");
            return await _service.UpdatePrazosProcessuaisItemAsync(id, req) ? NoContent() : NotFound();
        }

        [HttpPatch("{id:long}/anexos/create")]
        public async Task<IActionResult> CreateAnexos(long id, [FromBody] List<PrazosProcessuaisItemAnexoRequest> req)
        {
            if (req == null || req.Count == 0) return BadRequest("Nenhum item fornecido.");
            return await _service.CreateAnexoAsync(id, req) ? NoContent() : NotFound();
        }

        [HttpPatch("{id:long}/anexos/update")]
        public async Task<IActionResult> UpdateAnexos(long id, [FromBody] List<PrazosProcessuaisItemAnexoUpdate> req)
        {
            if (req == null || req.Count == 0) return BadRequest("Nenhum item fornecido.");
            return await _service.UpdateAnexoAsync(id, req) ? NoContent() : NotFound();
        }
    }
}
