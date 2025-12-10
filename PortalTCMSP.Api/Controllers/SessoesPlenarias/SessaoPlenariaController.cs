using Microsoft.AspNetCore.Mvc;
using PortalTCMSP.Domain.DTOs.Requests.SessaoPlenaria.Base;
using PortalTCMSP.Domain.DTOs.Responses.SessaoPlenaria.Base;
using PortalTCMSP.Domain.Services.SessaoPlenaria;

namespace PortalTCMSP.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "SessoesPlenarias")]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class SessaoPlenariaController : ControllerBase
    {
        private readonly ISessaoPlenariaService _service;

        public SessaoPlenariaController(ISessaoPlenariaService service)
        {
            _service = service;
        }

        /// <summary>Lista todas as sessões (inclui contagens de pautas/atas/notas).</summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<SessaoPlenariaResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<SessaoPlenariaResponse>>> GetTodas()
            => Ok(await _service.GetAllAsync());

        [HttpGet("ativa")]
        [ProducesResponseType(typeof(SessaoPlenariaResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SessaoPlenariaResponse>> GetAtiva()
        {
            var item = await _service.GetAtivaAsync();
            return item is null ? NotFound() : Ok(item);
        }

        /// <summary>Obtém uma sessão por ID.</summary>
        [HttpGet("{id:long}")]
        [ProducesResponseType(typeof(SessaoPlenariaResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SessaoPlenariaResponse>> GetPorId(long id)
        {
            var item = await _service.GetByIdAsync(id);
            return item is null ? NotFound() : Ok(item);
        }

        /// <summary>Obtém uma sessão pelo slug (slug único global).</summary>
        [HttpGet("slug/{slug}")]
        [ProducesResponseType(typeof(SessaoPlenariaResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SessaoPlenariaResponse>> GetPorSlug(string slug)
        {
            var item = await _service.GetBySlugAsync(slug);
            return item is null ? NotFound() : Ok(item);
        }

        /// <summary>Cria uma nova sessão plenária.</summary>
        /// <response code="201">Criada com sucesso. Retorna o ID.</response>
        /// <response code="400">Dados inválidos.</response>
        /// <response code="409">Conflito de slug (já existe).</response>
        [HttpPost]
        [ProducesResponseType(typeof(long), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<long>> Criar([FromBody] SessaoPlenariaCreateRequest request)
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

        /// <summary>Atualiza uma sessão plenária.</summary>
        /// <response code="204">Atualizada com sucesso.</response>
        /// <response code="404">Sessão não encontrada.</response>
        /// <response code="400">Dados inválidos.</response>
        /// <response code="409">Conflito de slug (já existe).</response>
        [HttpPatch("{id:long}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Patch(long id, [FromBody] SessaoPlenariaUpdateRequest request)
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

        /// <summary>Exclui uma sessão plenária.</summary>
        /// <response code="204">Excluída com sucesso.</response>
        /// <response code="404">Sessão não encontrada.</response>
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
