using Microsoft.AspNetCore.Mvc;
using PortalTCMSP.Domain.DTOs.Requests.Fiscalizacao.SecretariaControleExterno;
using PortalTCMSP.Domain.DTOs.Responses.Fiscalizacao.SecretariaControleExterno;
using PortalTCMSP.Domain.Services.Fiscalizacao;

namespace PortalTCMSP.Api.Controllers.Fiscalizacao
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "Fiscalizacao")]
    public class SecretariaControleExternoController(IFiscalizacaoSecretariaControleExternoSeervice service) : ControllerBase
    {
        private readonly IFiscalizacaoSecretariaControleExternoSeervice _service = service;

        /// <summary>Lista todos os conteúdos (inclui títulos e carrossel).</summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<FiscalizacaoSecretariaResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<FiscalizacaoSecretariaResponse>>> GetTodos()
            => Ok(await _service.GetAllAsync());

        /// <summary>Obtém um conteúdo por ID.</summary>
        [HttpGet("{id:long}")]
        [ProducesResponseType(typeof(FiscalizacaoSecretariaResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<FiscalizacaoSecretariaResponse>> GetPorId(long id)
        {
            var item = await _service.GetByIdAsync(id);
            return item is null ? NotFound() : Ok(item);
        }

        /// <summary>Obtém um conteúdo pelo slug (único).</summary>
        [HttpGet("slug/{slug}")]
        [ProducesResponseType(typeof(FiscalizacaoSecretariaResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<FiscalizacaoSecretariaResponse>> GetPorSlug([FromRoute] string slug)
        {
            var item = await _service.GetBySlugAsync(slug);
            return item is null ? NotFound() : Ok(item);
        }

        /// <summary>Cria um novo conteúdo.</summary>
        [HttpPost]
        [ProducesResponseType(typeof(long), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<long>> Criar([FromBody] FiscalizacaoSecretariaCreateRequest request)
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

        /// <summary>Atualiza um conteúdo existente (substitui listas de títulos e carrossel).</summary>
        [HttpPut("{id:long}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Atualizar(long id, [FromBody] FiscalizacaoSecretariaUpdateRequest request)
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

        /// <summary>Exclui um conteúdo.</summary>
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
