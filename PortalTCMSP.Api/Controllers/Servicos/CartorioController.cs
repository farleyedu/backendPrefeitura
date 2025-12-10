using Microsoft.AspNetCore.Mvc;
using PortalTCMSP.Domain.DTOs.Requests.Servicos.Cartorio;
using PortalTCMSP.Domain.DTOs.Responses.Servicos.Cartorio;
using PortalTCMSP.Domain.Services.Servicos.Cartorio;

namespace PortalTCMSP.Api.Controllers.Servicos
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "Servicos")]
    public class CartorioController(ICartorioService service) : ControllerBase
    {
        private readonly ICartorioService _service = service;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CartorioResponse>>> GetAll()
            => Ok(await _service.GetAllAsync());

        [HttpGet("{id:long}")]
        public async Task<ActionResult<CartorioResponse>> GetById(long id)
        {
            var item = await _service.GetByIdAsync(id);
            return item is null ? NotFound() : Ok(item);
        }

        [HttpGet("slug-ativo/{slug}")]
        public async Task<ActionResult<CartorioResponse>> GetBySlugActive(string slug)
        {
            var item = await _service.GetWithChildrenBySlugAtivoAsync(slug);
            return item is null ? NotFound() : Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult<long>> Create([FromBody] CartorioCreateRequest request)
        {
            var id = await _service.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id }, id);
        }

        [HttpPut("{id:long}")]
        public async Task<IActionResult> Update(long id, [FromBody] CartorioUpdateRequest request)
            => (await _service.UpdateAsync(id, request)) ? NoContent() : NotFound();

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id)
            => (await _service.DeleteAsync(id)) ? NoContent() : NotFound();

        [HttpPatch("{id:long}/atendimentos/create")]
        public async Task<IActionResult> CreateAtendimentos(long id, [FromBody] List<CartorioAtendimentoRequest> req)
        {
            if (req == null || req.Count == 0) return BadRequest("Nenhum atendimento fornecido.");
            return await _service.CreateAtendimentosAsync(id, req) ? NoContent() : NotFound();
        }

        [HttpPatch("{id:long}/atendimentos/update")]
        public async Task<IActionResult> UpdateAtendimentos(long id, [FromBody] List<CartorioAtendimentoUpdate> req)
        {
            if (req == null || req.Count == 0) return BadRequest("Nenhum atendimento fornecido.");
            return await _service.UpdateAtendimentosAsync(id, req) ? NoContent() : NotFound();
        }
    }
}
