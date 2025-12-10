using Microsoft.AspNetCore.Mvc;
using PortalTCMSP.Domain.DTOs.Requests.Consulta;
using PortalTCMSP.Domain.DTOs.Responses.Base;
using PortalTCMSP.Domain.DTOs.Responses.Consulta;
using PortalTCMSP.Domain.Services.Home;

namespace PortalTCMSP.Api.Controllers.Home
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "Home")]
    public class ConsultaController : ControllerBase
    {
        private readonly ILogger<ConsultaController> _logger;
        private readonly IConsultaService _consultaService;

        public ConsultaController(ILogger<ConsultaController> logger, IConsultaService consultaService)
        {
            _logger = logger;
            _consultaService = consultaService;
        }

        /// <summary>Busca consolidada de notícias (nova + old), priorizando notícias novas.</summary>
        [HttpGet("search")]
        [ProducesResponseType(typeof(ResultadoPaginadoResponse<ConsultaNoticiaItemResponse>), StatusCodes.Status200OK)]
        public ActionResult<ResultadoPaginadoResponse<ConsultaNoticiaItemResponse>> BuscarAsync([FromQuery] ConsultaNoticiasRequest request)
        {
            var result = _consultaService.BuscarNoticiasConsolidado(request);
            return Ok(result);
        }
    }
}
