using Microsoft.AspNetCore.Mvc;
using PortalTCMSP.Domain.DTOs.Requests.BannerRequest;
using PortalTCMSP.Domain.DTOs.Responses.Banner;
using PortalTCMSP.Domain.Services.Home;

namespace PortalTCMSP.Api.Controllers.Home
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "Home")]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class BannerController : ControllerBase
    {
        private readonly IBannerService _bannerService;

        public BannerController(IBannerService bannerService)
        {
            _bannerService = bannerService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<BannerResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<BannerResponse>>> GetTodos()
        {
            var result = await _bannerService.ObterTodosAsync();
            return Ok(result);
        }

        [HttpGet("{id:long}")]
        [ProducesResponseType(typeof(BannerResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BannerResponse>> GetPorId(long id)
        {
            var result = await _bannerService.ObterPorIdAsync(id);
            if (result == null) return NotFound();
            return Ok(result);
        }

        /// <summary>Retorna o(s) banner(s) ativo(s). Pela regra, deve haver 0..1 item.</summary>
        [HttpGet("ativo")]
        [ProducesResponseType(typeof(IEnumerable<BannerResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<BannerResponse>>> GetAtivo()
        {
            var ativos = await _bannerService.ObterAtivosAsync();
            return Ok(ativos);
        }

        [HttpPost]
        [ProducesResponseType(typeof(long), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Criar([FromBody] BannerCreateRequest request)
        {
            var id = await _bannerService.CriarAsync(request);
            return CreatedAtAction(nameof(GetPorId), new { id }, id);
        }

        [HttpPut("{id:long}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Atualizar(long id, [FromBody] BannerUpdateRequest request)
        {
            var ok = await _bannerService.AtualizarAsync(id, request);
            return ok ? NoContent() : NotFound();
        }

        [HttpDelete("{id:long}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Remover(long id)
        {
            var ok = await _bannerService.RemoverAsync(id);
            return ok ? NoContent() : NotFound();
        }
    }
}
