using Microsoft.AspNetCore.Mvc;
using PortalTCMSP.Domain.DTOs.Requests.SessaoPlenaria.Pauta;
using PortalTCMSP.Domain.DTOs.Responses.Base;
using PortalTCMSP.Domain.DTOs.Responses.SessaoPlenaria.Pauta;
using PortalTCMSP.Domain.Services.SessaoPlenaria;

namespace PortalTCMSP.Api.Controllers.SessoesPlenarias
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "SessoesPlenarias")]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class PautaController : ControllerBase
    {
        private readonly ISessaoPlenariaPautaService _service;

        public PautaController(ISessaoPlenariaPautaService service)
        {
            _service = service;
        }

        /// <summary>Busca paginada de pautas com filtros e termo livre.</summary>
        /// <remarks>
        /// Querystring de exemplo:
        /// <c>/api/pauta/search?page=1&amp;count=10&amp;q=ordinaria&amp</c>
        /// </remarks>
        [HttpGet("search")]
        [ProducesResponseType(typeof(ResultadoPaginadoResponse<SessaoPlenariaPautaResponse>), StatusCodes.Status200OK)]
        public async Task<ObjectResult> GetAll([FromQuery] SessaoPlenariaPautaSearchRequest request)
        {
            var response = await _service.GetAllPautas(request);
            if (response == null)
            {
                return new ObjectResult("Nenhuma pauta encontrada") { StatusCode = 404 };
            }
            return new ObjectResult(response) { StatusCode = 200 };
        }

        /// <summary>Obtém uma pauta por ID.</summary>
        /// <param name="id">Identificador da pauta.</param>
        /// <response code="200">Pauta encontrada.</response>
        /// <response code="404">Pauta não encontrada.</response>
        [HttpGet("{id:long}")]
        [ProducesResponseType(typeof(SessaoPlenariaPautaResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SessaoPlenariaPautaResponse>> GetPorId(long id)
        {
            var item = await _service.GetByIdAsync(id);
            return item is null ? NotFound() : Ok(item);
        }

        /// <summary>Cria uma nova pauta.</summary>
        /// <param name="request">Dados para criação da pauta.</param>
        /// <response code="201">Criada com sucesso. Retorna o ID.</response>
        /// <response code="400">Dados inválidos.</response>
        /// <response code="409">Conflito de slug (já existe pauta com o mesmo slug).</response>
        [HttpPost]
        [ProducesResponseType(typeof(long), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<long>> Criar([FromBody] SessaoPlenariaPautaCreateRequest request)
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

        /// <summary>Atualiza uma pauta existente.</summary>
        /// <param name="id">Identificador da pauta.</param>
        /// <param name="request">Dados para atualização.</param>
        /// <response code="204">Atualizada com sucesso.</response>
        /// <response code="404">Pauta não encontrada.</response>
        /// <response code="400">Dados inválidos.</response>
        /// <response code="409">Conflito de slug (já existe pauta com o mesmo slug).</response>
        [HttpPatch("{id:long}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Patch(long id, [FromBody] SessaoPlenariaPautaUpdateRequest request)
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

        /// <summary>Exclui uma pauta.</summary>
        /// <param name="id">Identificador da pauta.</param>
        /// <response code="204">Excluída com sucesso.</response>
        /// <response code="404">Pauta não encontrada.</response>
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
