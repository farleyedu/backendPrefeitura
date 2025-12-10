using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PortalTCMSP.Domain.DTOs.Requests.Noticia;
using PortalTCMSP.Domain.DTOs.Requests.NoticiaRequest;
using PortalTCMSP.Domain.DTOs.Responses.Base;
using PortalTCMSP.Domain.DTOs.Responses.Noticia;
using PortalTCMSP.Domain.Services.Noticia;

namespace PortalTCMSP.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "Noticia")]
    public class NoticiaController : ControllerBase
    {
        private readonly INoticiaService _noticiaService;

        public NoticiaController(INoticiaService noticiaService) => _noticiaService = noticiaService;

        [HttpGet]
        [ProducesResponseType(typeof(ResultadoPaginadoResponse<NoticiaResponse>), StatusCodes.Status200OK)]
        public ActionResult<ResultadoPaginadoResponse<NoticiaResponse>> GetAll([FromQuery] NoticiaListarRequest request)
        {
            var result = _noticiaService.ListarNoticias(request);
            return Ok(result);
        }

        [HttpGet("id/{id:long}", Name = "ObterNoticiaPorId")]
        [ProducesResponseType(typeof(NoticiaCompletaResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObterPorIdAsync([FromRoute] long id)
        {
            if (id <= 0) return BadRequest("ID inválido.");

            var response = await _noticiaService.ObterNoticiaPorIdAsync((int)id);
            if (response == null) return NotFound("Notícia não encontrada.");
            return Ok(response);
        }

        [HttpGet("{slug}", Name = "ObterNoticiaPorSlug")]
        [ProducesResponseType(typeof(NoticiaCompletaResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ObterPorSlugAsync(string slug)
        {
            var response = await _noticiaService.ObterNoticiaPorSlugAsync(slug);
            if (response == null) return NotFound("Notícia não encontrada");
            return Ok(response);
        }

        [HttpPost]
       // [Authorize(Roles = "WEB_GBAC_GESTOR-CONTEUDO")]
        [ProducesResponseType(typeof(NoticiaCompletaResponse), StatusCodes.Status201Created)]
        public async Task<IActionResult> CriarAsync([FromBody] NoticiaCreateRequest request)
        {
            var slug = await _noticiaService.AdicionarAsync(request);
            // Opcional: retornar o recurso
            var recurso = await _noticiaService.ObterNoticiaPorSlugAsync(slug);
            return CreatedAtRoute("ObterNoticiaPorSlug", new { slug }, recurso);
        }

        [HttpPut("{id}")]
        //[Authorize(Roles = "WEB_GBAC_GESTOR-CONTEUDO")]
        [ProducesResponseType(typeof(NoticiaCompletaResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AtualizarAsync(long id, [FromBody] NoticiaUpdateRequest request)
        {
            if (id != request.Id) return BadRequest("ID da rota difere do corpo da requisição.");

            var sucesso = await _noticiaService.AtualizarAsync(request);
            if (!sucesso) return NotFound("Notícia não encontrada.");

            var recurso = await _noticiaService.ObterNoticiaPorSlugAsync(request.Slug);
            return Ok(recurso);
        }

        [HttpPatch("{id:long}")]
        //[Authorize(Roles = "WEB_GBAC_GESTOR-CONTEUDO")]
        [ProducesResponseType(typeof(NoticiaCompletaResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PatchAsync([FromRoute] long id, [FromBody] NoticiaPatchRequest request)
        {
            var sucesso = await _noticiaService.PatchAsync(id, request);
            if (!sucesso) return NotFound("Notícia não encontrada.");

            var recurso = await _noticiaService.ObterNoticiaPorIdAsync((int)id);
            return Ok(recurso);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "WEB_GBAC_GESTOR-CONTEUDO")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeletarAsync(long id)
        {
            var sucesso = await _noticiaService.DeletarAsync((int)id);
            if (!sucesso) return NotFound("Notícia não encontrada.");
            return NoContent();
        }
    }
}