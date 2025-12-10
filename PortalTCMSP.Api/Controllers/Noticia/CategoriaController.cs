using Microsoft.AspNetCore.Mvc;
using PortalTCMSP.Domain.DTOs.Requests.Categoria;
using PortalTCMSP.Domain.DTOs.Responses.CategoriaResponse;
using PortalTCMSP.Domain.Services.Home;

namespace PortalTCMSP.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "Noticia")]
    public class CategoriaController : ControllerBase
    {
        private readonly ICategoriaService _categoriaService;

        public CategoriaController(ICategoriaService categoriaService)
        {
            _categoriaService = categoriaService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoriaResponse>>> Listar()
        {
            var categorias = await _categoriaService.ListarAsync();
            return Ok(categorias);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CategoriaResponse>> Obter(int id)
        {
            var categoria = await _categoriaService.ObterPorIdAsync(id);
            if (categoria == null) return NotFound();
            return Ok(categoria);
        }

        [HttpPost]
        public async Task<IActionResult> Criar([FromBody] CategoriaCreateRequest request)
        {
            var id = await _categoriaService.CriarAsync(request);
            return CreatedAtAction(nameof(Obter), new { id }, null);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Atualizar(int id, [FromBody] CategoriaUpdateRequest request)
        {
            if (id != request.Id) return BadRequest("ID do corpo e rota estão diferentes.");
            var sucesso = await _categoriaService.AtualizarAsync(request);
            return sucesso ? NoContent() : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Deletar(int id)
        {
            var sucesso = await _categoriaService.DeletarAsync(id);
            return sucesso ? NoContent() : NotFound();
        }
    }
}
