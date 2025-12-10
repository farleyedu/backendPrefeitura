using Microsoft.AspNetCore.Mvc;
using PortalTCMSP.Domain.DTOs.Requests.SessaoPlenaria.Ata;
using PortalTCMSP.Domain.DTOs.Requests.SessaoPlenaria.NotasTaquigraficas;
using PortalTCMSP.Domain.DTOs.Responses.Base;
using PortalTCMSP.Domain.DTOs.Responses.SessaoPlenaria.NotasTaquigraficas;
using PortalTCMSP.Domain.Services.SessaoPlenaria;

namespace PortalTCMSP.Api.Controllers.SessoesPlenarias
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "SessoesPlenarias")]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class NotasTaquigraficasController : ControllerBase
    {
        private readonly ISessaoPlenariaNotasTaquigraficasService _service;

        public NotasTaquigraficasController(ISessaoPlenariaNotasTaquigraficasService service)
        {
            _service = service;
        }

        /// <summary>Lista todas as notas taquigráficas (inclui anexos).</summary>
        /// <response code="200">Lista retornada com sucesso.</response>
        [HttpGet]
        [ProducesResponseType(typeof(ResultadoPaginadoResponse<SessaoPlenariaNotasTaquigraficasResponse>), StatusCodes.Status200OK)]
        public async Task<ObjectResult> List([FromQuery] SessaoPlenariaNotasTaquigraficasSearchRequest request)
        {
            var response = await _service.GetAllNotas(request);
            if (response == null)
                return new ObjectResult("Nenhuma nota taquigráfica encontrada") { StatusCode = 404 };

            return new ObjectResult(response) { StatusCode = 200 };
        }

        /// <summary>Obtém uma nota taquigráfica por ID.</summary>
        /// <param name="id">Identificador do registro.</param>
        /// <response code="200">Registro encontrado.</response>
        /// <response code="404">Registro não encontrado.</response>
        [HttpGet("{id:long}")]
        [ProducesResponseType(typeof(SessaoPlenariaNotasTaquigraficasResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SessaoPlenariaNotasTaquigraficasResponse>> GetPorId(long id)
        {
            var item = await _service.GetByIdAsync(id);
            return item is null ? NotFound() : Ok(item);
        }

        /// <summary>Cria um novo registro de notas taquigráficas.</summary>
        /// <param name="request">Dados para criação.</param>
        /// <response code="201">Criado com sucesso. Retorna o ID.</response>
        /// <response code="400">Dados inválidos.</response>
        /// <response code="409">Conflito de slug (já existe registro com o mesmo slug).</response>
        [HttpPost]
        [ProducesResponseType(typeof(long), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<long>> Criar([FromBody] SessaoPlenariaNotasTaquigraficasCreateRequest request)
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

        /// <summary>Atualiza um registro de notas taquigráficas.</summary>
        /// <param name="id">Identificador do registro.</param>
        /// <param name="request">Dados para atualização.</param>
        /// <response code="204">Atualizado com sucesso.</response>
        /// <response code="404">Registro não encontrado.</response>
        /// <response code="400">Dados inválidos.</response>
        /// <response code="409">Conflito de slug (já existe registro com o mesmo slug).</response>
        [HttpPatch("{id:long}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Patch(long id, [FromBody] SessaoPlenariaNotasTaquigraficasUpdateRequest request)
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

        /// <summary>Exclui um registro de notas taquigráficas.</summary>
        /// <param name="id">Identificador do registro.</param>
        /// <response code="204">Excluído com sucesso.</response>
        /// <response code="404">Registro não encontrado.</response>
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
