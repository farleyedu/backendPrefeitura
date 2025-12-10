using Microsoft.AspNetCore.Mvc;
using PortalTCMSP.Domain.DTOs.Requests.Noticia;
using PortalTCMSP.Domain.DTOs.Responses.Base;
using PortalTCMSP.Domain.DTOs.Responses.Noticia;
using PortalTCMSP.Domain.Services.Noticia;

namespace PortalTCMSP.Api.Controllers.Noticia
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "Noticia")]
    public class NoticiaOldController : ControllerBase
    {
        private readonly INoticiaOldService _service;
        public NoticiaOldController(INoticiaOldService service) => _service = service;

        /// <summary>Lista notícias (modelo antigo) com paginação e filtros.</summary>
        [HttpGet]
        [ProducesResponseType(typeof(ResultadoPaginadoResponse<NoticiaOldResponse>), StatusCodes.Status200OK)]
        public ActionResult<ResultadoPaginadoResponse<NoticiaOldResponse>> GetAll([FromQuery] NoticiaOldListarRequest request)
        {
            var result = _service.ListarNoticiasOld(request);
            return Ok(result);
        }

        [HttpGet("map")]
        [ProducesResponseType(typeof(ResultadoPaginadoResponse<NoticiaOldMappedResponse>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ResultadoPaginadoResponse<NoticiaOldMappedResponse>>> GetAllMap(
     [FromQuery] NoticiaOldListarRequest request)
        {
            var result = await _service.ListarNoticiasOldMapAsync(request);
            return Ok(result);
        }

        /// <summary>Obtém uma notícia (modelo antigo) por Id.</summary>
        [HttpGet("{id:long}")]
        [ProducesResponseType(typeof(NoticiaOldResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<NoticiaOldResponse>> GetById(long id, CancellationToken ct)
        {
            var result = await _service.GetByIdAsync(id, ct);
            return result is null ? NotFound() : Ok(result);
        }

        /// <summary>Obtém uma notícia (modelo antigo) por Id — versão mapeada.</summary>
        [HttpGet("{id:long}/map")]
        [ProducesResponseType(typeof(NoticiaOldMappedResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<NoticiaOldMappedResponse>> GetByIdMap(long id, CancellationToken ct)
        {
            var result = await _service.GetByIdMapAsync(id, ct);
            return result is null ? NotFound() : Ok(result);
        }

        /// <summary>Obtém uma notícia (modelo antigo) por slug.</summary>
        [HttpGet("slug/{slug}", Name = "ObterNoticiaOldPorSlug")]
        [ProducesResponseType(typeof(NoticiaOldResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<NoticiaOldResponse>> GetBySlug(string slug, CancellationToken ct)
        {
            var result = await _service.GetBySlugAsync(slug, ct);
            return result is null ? NotFound() : Ok(result);
        }

        /// <summary>Obtém uma notícia (modelo antigo) por slug — versão mapeada (HTML limpo em blocos etc.).</summary>
        [HttpGet("slug/{slug}/map", Name = "ObterNoticiaOldPorSlugMap")]
        [ProducesResponseType(typeof(NoticiaOldMappedResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<NoticiaOldMappedResponse>> GetBySlugMap(string slug, CancellationToken ct)
        {
            var result = await _service.GetBySlugMapAsync(slug, ct);
            return result is null ? NotFound() : Ok(result);
        }
    }
}
