using Microsoft.AspNetCore.Mvc;
using PortalTCMSP.Domain.DTOs.Requests.SessaoPlenaria.SustentacaoOral;
using PortalTCMSP.Domain.DTOs.Responses.SessaoPlenaria.SustentacaoOral;
using PortalTCMSP.Domain.Services.SessaoPlenaria;

namespace PortalTCMSP.Api.Controllers.SessoesPlenarias
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "SessoesPlenarias")]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class SustentacaoOralController : ControllerBase
    {
        private readonly ISessaoPlenariaSustentacaoOralService _service;

        public SustentacaoOralController(ISessaoPlenariaSustentacaoOralService service)
        {
            _service = service;
        }

        /// <summary>Lista todas as Sustentações Orais (inclui anexos).</summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<SessaoPlenariaSustentacaoOralResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<SessaoPlenariaSustentacaoOralResponse>>> GetTodas()
            => Ok(await _service.GetAllAsync());

        /// <summary>Obtém a Sustentação Oral ativa (única) — inclui anexos.</summary>
        [HttpGet("ativa")]
        [ProducesResponseType(typeof(SessaoPlenariaSustentacaoOralResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SessaoPlenariaSustentacaoOralResponse>> GetAtiva()
        {
            var item = await _service.GetAtivaAsync();
            return item is null ? NotFound() : Ok(item);
        }

        /// <summary>Obtém uma Sustentação Oral por ID.</summary>
        [HttpGet("{id:long}")]
        [ProducesResponseType(typeof(SessaoPlenariaSustentacaoOralResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SessaoPlenariaSustentacaoOralResponse>> GetPorId(long id)
        {
            var item = await _service.GetByIdAsync(id);
            return item is null ? NotFound() : Ok(item);
        }

        /// <summary>Obtém uma Sustentação Oral pelo slug (slug único global).</summary>
        [HttpGet("slug/{slug}")]
        [ProducesResponseType(typeof(SessaoPlenariaSustentacaoOralResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SessaoPlenariaSustentacaoOralResponse>> GetPorSlug(string slug)
        {
            var item = await _service.GetBySlugAsync(slug);
            return item is null ? NotFound() : Ok(item);
        }

        /// <summary>Cria uma nova Sustentação Oral.</summary>
        [HttpPost]
        [ProducesResponseType(typeof(long), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<long>> Criar([FromBody] SessaoPlenariaSustentacaoOralCreateRequest request)
        {
            try
            {
                var id = await _service.CreateAsync(request);
                return CreatedAtAction(nameof(GetPorId), new { id }, id);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        /// <summary>Atualiza parcialmente uma Sustentação Oral existente.</summary>
        [HttpPatch("{id:long}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Patch(long id, [FromBody] SessaoPlenariaSustentacaoOralUpdateRequest request)
        {
            try
            {
                var ok = await _service.UpdateAsync(id, request); 
                return ok ? NoContent() : NotFound();
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        /// <summary>Exclui uma Sustentação Oral.</summary>
        [HttpDelete("{id:long}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Excluir(long id)
        {
            var ok = await _service.DeleteAsync(id);
            return ok ? NoContent() : NotFound();
        }
    }
}
