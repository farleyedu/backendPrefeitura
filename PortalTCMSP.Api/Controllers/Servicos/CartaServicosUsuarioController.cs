using Microsoft.AspNetCore.Mvc;
using PortalTCMSP.Domain.DTOs.Requests.Servicos.CartaServicosUsuario;
using PortalTCMSP.Domain.DTOs.Requests.Servicos.CartaServicosUsuario.Application.DTO.Servicos.CartaServicosUsuarioDto;
using PortalTCMSP.Domain.DTOs.Responses.Servicos.CartaServicosUsuario.Application.DTO.Servicos.CartaServicosUsuarioDto;
using PortalTCMSP.Domain.Services.Servicos.CartaServicosUsuario;

namespace PortalTCMSP.Api.Controllers.Servicos
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "Servicos")]
    public class CartaServicosUsuarioController(ICartaServicosUsuarioService service) : ControllerBase
    {
        private readonly ICartaServicosUsuarioService _service = service;

        [HttpGet("search")]
        [ProducesResponseType(typeof(CartaServicosUsuarioResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CartaServicosUsuarioResponse>> GetSearchAsync(
    [FromQuery] CartaServicosUsuarioDescritivoItemDetalheSearchRequest request)
        {
            var response = await _service.GetSearchAsync(request);

            if (response is null)
                return NotFound("Nenhuma carta encontrada.");

            return Ok(response);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CartaServicosUsuarioResponse>>> GetAll()
            => Ok(await _service.GetAllAsync());

        [HttpGet("{id:long}")]
        public async Task<ActionResult<CartaServicosUsuarioResponse>> GetById(long id)
        {
            var item = await _service.GetByIdAsync(id);
            return item is null ? NotFound() : Ok(item);
        }

        [HttpGet("slug-ativo/{slug}")]
        public async Task<ActionResult<CartaServicosUsuarioResponse>> GetBySlugActive(string slug)
        {
            var item = await _service.GetWithChildrenBySlugAtivoAsync(slug);
            return item is null ? NotFound() : Ok(item);
        }

        [HttpPost]
        [ProducesResponseType(typeof(CartaServicosUsuarioResponse), StatusCodes.Status201Created)]
        public async Task<ActionResult<CartaServicosUsuarioResponse>> Create(
     [FromBody] CartaServicosUsuarioRequest request)
        {
            var response = await _service.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = response.Id }, response);
        }

        [HttpPut("{id:long}")]
        public async Task<IActionResult> Update(long id, [FromBody] CartaServicosUsuarioRequest request)
            => (await _service.UpdateAsync(id, request)) ? NoContent() : NotFound();

        [HttpDelete("{id:long}")]
        public async Task<IActionResult> Delete(long id)
            => (await _service.DeleteAsync(id)) ? NoContent() : NotFound();

        [HttpPatch("{id:long}/servicos/create")]
        [ProducesResponseType(typeof(CartaServicosUsuarioResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CartaServicosUsuarioResponse>> CreateServicos(
     long id,
     [FromBody] List<CartaServicosUsuarioServicoRequest> req)
        {
            if (req == null || req.Count == 0)
                return BadRequest("Nenhum servico fornecido.");

            var response = await _service.CreateServicosAsync(id, req);
            if (response is null)
                return NotFound();

            return Ok(response);
        }

        [HttpPatch("{id:long}/servicos/update")]
        [ProducesResponseType(typeof(CartaServicosUsuarioResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CartaServicosUsuarioResponse>> UpdateAtendimentos(
     long id,
     [FromBody] List<CartaServicosUsuarioServicoUpdate> req)
        {
            if (req == null || req.Count == 0)
                return BadRequest("Nenhum servico fornecido.");

            var response = await _service.UpdateServicosAsync(id, req);
            if (response is null)
                return NotFound();

            return Ok(response);
        }

        [HttpPatch("{id:long}/servicos-item/create")]
        [ProducesResponseType(typeof(CartaServicosUsuarioResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CartaServicosUsuarioResponse>> CreateServicosItens(
    long id,
    [FromBody] List<CartaServicosUsuarioServicoItemRequest> req)
        {
            if (req == null || req.Count == 0)
                return BadRequest("Nenhum servico item fornecido.");

            var response = await _service.CreateServicosItensAsync(id, req);
            if (response is null)
                return NotFound();

            return Ok(response);
        }

        [HttpPatch("{id:long}/servicos-item/update")]
        [ProducesResponseType(typeof(CartaServicosUsuarioResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CartaServicosUsuarioResponse>> UpdateServicosItens(
     long id,
     [FromBody] List<CartaServicosUsuarioServicoItemUpdate> req)
        {
            if (req == null || req.Count == 0)
                return BadRequest("Nenhum servico item fornecido.");

            var response = await _service.UpdateServicosItensAsync(id, req);
            if (response is null)
                return NotFound();

            return Ok(response);
        }

        [HttpPatch("{id:long}/item-detalhe/create")]
        [ProducesResponseType(typeof(CartaServicosUsuarioResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CartaServicosUsuarioResponse>> CreateItemDetalhe(
    long id,
    [FromBody] List<CartaServicosUsuarioItemDetalheRequest> req)
        {
            if (req == null || req.Count == 0)
                return BadRequest("Nenhum item detalhe fornecido.");

            var response = await _service.CreateServicosItensDetalhesAsync(id, req);
            if (response is null)
                return NotFound();

            return Ok(response);
        }

        [HttpPatch("{id:long}/item-detalhe/update")]
        [ProducesResponseType(typeof(CartaServicosUsuarioResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CartaServicosUsuarioResponse>> UpdateItemDetalhe(
    long id,
    [FromBody] List<CartaServicosUsuarioItemDetalheUpdate> req)
        {
            if (req == null || req.Count == 0)
                return BadRequest("Nenhum item detalhe fornecido.");

            var response = await _service.UpdateServicosItensDetalhesAsync(id, req);
            if (response is null)
                return NotFound();

            return Ok(response);
        }

        [HttpPatch("{id:long}/descritivo-item-detalhe/create")]
        [ProducesResponseType(typeof(CartaServicosUsuarioResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CartaServicosUsuarioResponse>> CreateDescritivoItemDetalhe(
     long id,
     [FromBody] List<CartaServicosUsuarioDescritivoItemDetalheRequest> req)
        {
            if (req == null || req.Count == 0)
                return BadRequest("Nenhum descritivo item detalhe fornecido.");

            var response = await _service.CreateDescritivoItemDetalheAsync(id, req);
            if (response is null)
                return NotFound();

            return Ok(response);
        }

        [HttpPatch("{id:long}/descritivo-item-detalhe/update")]
        [ProducesResponseType(typeof(CartaServicosUsuarioResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CartaServicosUsuarioResponse>> UpdateDescritivoItemDetalhe(
    long id,
    [FromBody] List<CartaServicosUsuarioDescritivoItemDetalheUpdate> req)
        {
            if (req == null || req.Count == 0)
                return BadRequest("Nenhum descritivo item detalhe fornecido.");

            var response = await _service.UpdateDescritivoItemDetalheAsync(id, req);
            if (response is null)
                return NotFound();

            return Ok(response);
        }
    }
}