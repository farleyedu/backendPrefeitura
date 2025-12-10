using Microsoft.AspNetCore.Mvc;
using PortalTCMSP.Domain.DTOs.Requests.Servicos.MultasProcedimentos;
using PortalTCMSP.Domain.DTOs.Responses.Servicos.MultasProcedimentos;
using PortalTCMSP.Domain.Services.Servicos.MultasProcedimentos;

namespace PortalTCMSP.Api.Controllers.Servicos
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "Servicos")]
    public class MultasProcedimentosController(IMultasProcedimentosService service) : ControllerBase
    {
        private readonly IMultasProcedimentosService _service = service;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MultasProcedimentosResponse>>> GetAll()
            => Ok(await _service.GetAllAsync());

        [HttpGet("{id:long}")]
        public async Task<ActionResult<MultasProcedimentosResponse>> GetById(long id)
        {
            var item = await _service.GetByIdAsync(id);
            return item is null ? NotFound() : Ok(item);
        }

        [HttpGet("slug-ativo/{slug}")]
        [ProducesResponseType(typeof(MultasProcedimentosResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<MultasProcedimentosResponse>> GetBySlugActive(string slug)
        {
            var item = await _service.GetWithChildrenBySlugAtivoAsync(slug);
            return item is null ? NotFound() : Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult<long>> Create([FromBody] MultasProcedimentosCreateRequest request)
        {
            var id = await _service.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id }, id);
        }

        [HttpPut("{id:long}")]
        public async Task<IActionResult> Update(long id, [FromBody] MultasProcedimentosUpdateRequest request)
            => (await _service.UpdateAsync(id, request)) ? NoContent() : NotFound();

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Excluir(long id)
            => (await _service.DeleteAsync(id)) ? NoContent() : NotFound();

        [HttpPatch("{id:long}/procedimentos/create")]
        public async Task<IActionResult> CreateProcedimentos(long id, [FromBody] List<MultasProcedimentosProcedimentoRequest> req)
        {
            if (req == null || req.Count == 0) return BadRequest("Nenhum procedimento fornecido.");
            return await _service.CreateProcedimentosAsync(id, req) ? NoContent() : NotFound();
        }

        [HttpPatch("{id:long}/procedimentos/update")]
        public async Task<IActionResult> UpdateProcedimentos(long id, [FromBody] List<MultasProcedimentosProcedimentoUpdate> req)
        {
            if (req == null || req.Count == 0) return BadRequest("Nenhum procedimento fornecido.");
            return await _service.UpdateProcedimentosAsync(id, req) ? NoContent() : NotFound();
        }

        [HttpPatch("{id:long}/portarias/create")]
        public async Task<IActionResult> CreatePortarias(long id, [FromBody] List<MultasProcedimentosPortariaRelacionadaRequest> req)
        {
            if (req == null || req.Count == 0) return BadRequest("Nenhuma portaria fornecida.");
            return await _service.CreatePortariaRelacionadasAsync(id, req) ? NoContent() : NotFound();
        }

        [HttpPatch("{id:long}/portarias/update")]
        public async Task<IActionResult> UpdatePortarias(long id, [FromBody] List<MultasProcedimentosPortariaRelacionadaUpdate> req)
        {
            if (req == null || req.Count == 0) return BadRequest("Nenhuma portaria fornecida.");
            return await _service.UpdatePortariaRelacionadasAsync(id, req) ? NoContent() : NotFound();
        }

    }

}
