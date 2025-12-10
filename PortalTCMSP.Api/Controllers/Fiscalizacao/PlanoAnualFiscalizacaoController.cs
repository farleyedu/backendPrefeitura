using Microsoft.AspNetCore.Mvc;
using PortalTCMSP.Domain.DTOs.Requests.Fiscalizacao.PlanoAnualFiscalizacao;
using PortalTCMSP.Domain.DTOs.Responses.Fiscalizacao.PlanoAnualFiscalizacao;
using PortalTCMSP.Domain.Services.Fiscalizacao;

namespace PortalTCMSP.Api.Controllers.Fiscalizacao
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "Fiscalizacao")]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class PlanoAnualFiscalizacaoController : ControllerBase
    {
        private readonly IFiscalizacaoPlanoAnualFiscalizacaoService _service;

        public PlanoAnualFiscalizacaoController(IFiscalizacaoPlanoAnualFiscalizacaoService service)
        {
            _service = service;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<FiscalizacaoPlanoAnualFiscalizacaoResolucaoResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<FiscalizacaoPlanoAnualFiscalizacaoResolucaoResponse>>> GetTodos()
        => Ok(await _service.GetAllAsync());

        [HttpGet("{id:long}")]
        [ProducesResponseType(typeof(FiscalizacaoPlanoAnualFiscalizacaoResolucaoResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<FiscalizacaoPlanoAnualFiscalizacaoResolucaoResponse>> GetPorId(long id)
        {
            var item = await _service.GetByIdAsync(id);
            return item is null ? NotFound() : Ok(item);
        }

        [HttpGet("slug/{slug}")]
        [ProducesResponseType(typeof(FiscalizacaoPlanoAnualFiscalizacaoResolucaoResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<FiscalizacaoPlanoAnualFiscalizacaoResolucaoResponse>> GetPorSlug([FromRoute] string slug)
        {
            var item = await _service.GetBySlugAsync(slug);
            return item is null ? NotFound() : Ok(item);
        }

        [HttpPost]
        [ProducesResponseType(typeof(long), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<long>> Criar([FromBody] FiscalizacaoPlanoAnualFiscalizacaoResolucaoCreateRequest request)
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

        [HttpPut("{id:long}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Atualizar(long id, [FromBody] FiscalizacaoPlanoAnualFiscalizacaoResolucaoUpdateRequest request)
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
