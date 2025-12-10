using Microsoft.AspNetCore.Mvc;
using PortalTCMSP.Domain.DTOs.Requests.SessaoPlenaria.Ata;
using PortalTCMSP.Domain.DTOs.Responses.Base;
using PortalTCMSP.Domain.DTOs.Responses.SessaoPlenaria.Ata;
using PortalTCMSP.Domain.Services.SessaoPlenaria;

namespace PortalTCMSP.Api.Controllers.SessoesPlenarias
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "SessoesPlenarias")]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class AtasController : ControllerBase
    {
        private readonly ISessaoPlenariaAtaService _service;

        public AtasController(ISessaoPlenariaAtaService service)
        {
            _service = service;
        }

        /// <summary>Lista todas as atas (inclui anexos).</summary>
        /// <response code="200">Lista retornada com sucesso.</response>
        [HttpGet]
        [ProducesResponseType(typeof(ResultadoPaginadoResponse<SessaoPlenariaAtaResponse>), StatusCodes.Status200OK)]
        public async Task<ObjectResult> List([FromQuery] SessaoPlenariaAtaSearchRequest request)
        {
            var response = await _service.GetAllAtas(request);
            if (response == null)
                return new ObjectResult("Nenhuma ata encontrada") { StatusCode = 404 };

            return new ObjectResult(response) { StatusCode = 200 };
        }

        /// <summary>Obtém uma ata por ID.</summary>
        /// <param name="id">Identificador da ata.</param>
        /// <response code="200">Ata encontrada.</response>
        /// <response code="404">Ata não encontrada.</response>
        [HttpGet("{id:long}")]
        [ProducesResponseType(typeof(SessaoPlenariaAtaResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SessaoPlenariaAtaResponse>> GetPorId(long id)
        {
            var item = await _service.GetByIdAsync(id);
            return item is null ? NotFound() : Ok(item);
        }

        /// <summary>Cria uma nova ata.</summary>
        /// <param name="request">Dados para criação da ata.</param>
        /// <response code="201">Criada com sucesso. Retorna o ID.</response>
        /// <response code="400">Dados inválidos.</response>
        /// <response code="409">Conflito de slug (já existe ata com o mesmo slug).</response>
        [HttpPost]
        [ProducesResponseType(typeof(long), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<long>> Criar([FromBody] SessaoPlenariaAtaCreateRequest request)
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

        /// <summary>Atualiza uma ata existente.</summary>
        /// <param name="id">Identificador da ata.</param>
        /// <param name="request">Dados para atualização.</param>
        /// <response code="204">Atualizada com sucesso.</response>
        /// <response code="404">Ata não encontrada.</response>
        /// <response code="400">Dados inválidos.</response>
        /// <response code="409">Conflito de slug (já existe ata com o mesmo slug).</response>
        [HttpPatch("{id:long}")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Patch(long id, [FromBody] SessaoPlenariaAtaUpdateRequest request)
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

        /// <summary>Exclui uma ata.</summary>
        /// <param name="id">Identificador da ata.</param>
        /// <response code="204">Excluída com sucesso.</response>
        /// <response code="404">Ata não encontrada.</response>
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
