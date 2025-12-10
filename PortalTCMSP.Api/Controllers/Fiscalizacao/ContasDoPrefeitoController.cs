using Microsoft.AspNetCore.Mvc;
using PortalTCMSP.Domain.DTOs.Requests.Fiscalizacao.ContasDoPrefeito;
using PortalTCMSP.Domain.DTOs.Responses.Base;
using PortalTCMSP.Domain.DTOs.Responses.Fiscalizacao.ContasDoPrefeito;
using PortalTCMSP.Domain.Services.Fiscalizacao;

namespace PortalTCMSP.Api.Controllers.Fiscalizacao
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "Fiscalizacao")]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class ContasDoPrefeitoController(IFiscalizacaoContasDoPrefeitoService service) : ControllerBase
    {
        private readonly IFiscalizacaoContasDoPrefeitoService _service = service;

        /// <summary>Busca paginada com filtros (seção, sessão, número, ano, período) e termo livre.</summary>
        /// <remarks>Ex.: <c>/api/contasdoprefeito/search?page=1&amp;count=10&amp;ano=2024&amp;q=3.304</c></remarks>
        [HttpGet("search")]
        [ProducesResponseType(typeof(ResultadoPaginadoResponse<FiscalizacaoContasDoPrefeitoResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ResultadoPaginadoResponse<FiscalizacaoContasDoPrefeitoResponse>>> Search(
            [FromQuery] FiscalizacaoContasDoPrefeitoSearchRequest request)
        {
            var result = await _service.GetListAsync(request);
            return Ok(result);
        }

        /// <summary>Obtém um item por ID.</summary>
        [HttpGet("{id:long}")]
        [ProducesResponseType(typeof(FiscalizacaoContasDoPrefeitoResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<FiscalizacaoContasDoPrefeitoResponse>> GetById(long id)
        {
            var item = await _service.GetByIdAsync(id);
            return item is null ? NotFound() : Ok(item);
        }

        /// <summary>Cria novo registro (único por Seção + Número + Ano).</summary>
        [HttpPost]
        [ProducesResponseType(typeof(long), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<ActionResult<long>> Create([FromBody] FiscalizacaoContasDoPrefeitoCreateRequest request)
        {
            try
            {
                var id = await _service.CreateAsync(request);
                return CreatedAtAction(nameof(GetById), new { id }, id);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        /// <summary>Atualiza um registro existente.</summary>
        [HttpPut("{id:long}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> Update(long id, [FromBody] FiscalizacaoContasDoPrefeitoUpdateRequest request)
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

        /// <summary>Exclui um registro.</summary>
        [HttpDelete("{id:long}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(long id)
        {
            var ok = await _service.DeleteAsync(id);
            return ok ? NoContent() : NotFound();
        }
    }
}
