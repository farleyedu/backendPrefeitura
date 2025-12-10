using Microsoft.AspNetCore.Mvc;
using PortalTCMSP.Domain.DTOs.Responses.SessaoPlenaria.Base;
using PortalTCMSP.Domain.Services.SessaoPlenaria;

namespace PortalTCMSP.Api.Controllers.SessoesPlenarias
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "SessoesPlenarias")]
    [Produces("application/json")]
    public class AoVivoController : ControllerBase
    {
        private readonly ISessaoPlenariaService _sessaoPlenariaService;

        public AoVivoController(ISessaoPlenariaService sessaoPlenariaService)
        {
            _sessaoPlenariaService = sessaoPlenariaService;
        }

        /// <summary>Lista as sessões plenárias ativas (ao vivo).</summary>
        /// <response code="200">Lista retornada com sucesso (0..1 item).</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<SessaoPlenariaResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<SessaoPlenariaResponse>>> Get()
        {
            var ativos = await _sessaoPlenariaService.GetAtivosAsync();
            return Ok(ativos);
        }
    }
}
