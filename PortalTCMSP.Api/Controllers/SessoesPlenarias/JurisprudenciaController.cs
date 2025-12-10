using Microsoft.AspNetCore.Mvc;
using PortalTCMSP.Domain.DTOs.Requests.SessaoPlenaria.Jurispudencia;
using PortalTCMSP.Domain.DTOs.Responses.SessaoPlenaria.Jurispudencia;
using PortalTCMSP.Domain.Services.SessaoPlenaria;

namespace PortalTCMSP.Api.Controllers.SessoesPlenarias
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "SessoesPlenarias")]
    [Produces("application/json")]
    public class JurisprudenciaController : ControllerBase
    {
        private readonly ISessaoPlenariaJurispudenciaService _service;

        public JurisprudenciaController(ISessaoPlenariaJurispudenciaService service)
        {
            _service = service;
        }

        [HttpGet("ativo")]
        [ProducesResponseType(typeof(SessaoPlenariaJurisprudenciaResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SessaoPlenariaJurisprudenciaResponse>> GetAtivo()
        {
            var item = await _service.GetAtivoAsync();
            return item is null ? NotFound() : Ok(item);
        }

        /// <summary>Lista todos os registros de Jurisprudência.</summary>
        /// <response code="200">Lista retornada com sucesso.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<SessaoPlenariaJurisprudenciaResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<SessaoPlenariaJurisprudenciaResponse>>> GetTodas()
            => Ok(await _service.GetAllAsync());

        /// <summary>Obtém um registro de Jurisprudência pelo <paramref name="id"/>.</summary>
        /// <param name="id">Identificador do registro.</param>
        /// <response code="200">Registro encontrado.</response>
        /// <response code="404">Registro não encontrado.</response>
        [HttpGet("{id:long}")]
        [ProducesResponseType(typeof(SessaoPlenariaJurisprudenciaResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SessaoPlenariaJurisprudenciaResponse>> GetPorId(long id)
        {
            var item = await _service.GetByIdAsync(id);
            return item is null ? NotFound() : Ok(item);
        }

        /// <summary>Obtém um registro de Jurisprudência pelo <paramref name="slug"/>.</summary>
        /// <param name="slug">Slug único do registro.</param>
        /// <response code="200">Registro encontrado.</response>
        /// <response code="404">Registro não encontrado.</response>
        [HttpGet("slug/{slug}")]
        [ProducesResponseType(typeof(SessaoPlenariaJurisprudenciaResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<SessaoPlenariaJurisprudenciaResponse>> GetPorSlug([FromRoute] string slug)
        {
            var item = await _service.GetBySlugAsync(slug);
            return item is null ? NotFound() : Ok(item);
        }

        /// <summary>Cria um novo registro de Jurisprudência.</summary>
        /// <param name="request">Dados para criação.</param>
        /// <response code="201">Criado com sucesso. Retorna o ID.</response>
        /// <response code="400">Dados inválidos.</response>
        /// <response code="409">Conflito de slug (já existe).</response>
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(long), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<long>> Criar([FromBody] SessaoPlenariaJurisprudenciaCreateRequest request)
        {
            try
            {
                var id = await _service.CreateAsync(request);
                return CreatedAtAction(nameof(GetPorId), new { id }, id);
            }
            catch (InvalidOperationException ex)
            {
                // Ex.: slug duplicado
                return Conflict(new { message = ex.Message });
            }
        }

        /// <summary>Atualiza um registro de Jurisprudência existente.</summary>
        /// <param name="id">Identificador do registro.</param>
        /// <param name="request">Dados para atualização.</param>
        /// <response code="204">Atualizado com sucesso.</response>
        /// <response code="404">Registro não encontrado.</response>
        /// <response code="400">Dados inválidos.</response>
        /// <response code="409">Conflito de slug (já existe).</response>
        [HttpPatch("{id:long}")]
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Patch(long id, [FromBody] SessaoPlenariaJurisprudenciaUpdateRequest request)
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

        /// <summary>Exclui um registro de Jurisprudência.</summary>
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
