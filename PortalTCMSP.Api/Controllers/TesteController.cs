using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PortalTCMSP.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class TesteController : ControllerBase
    {
        // 1) Endpoint público (sem token)
        [HttpGet("public")]
        [AllowAnonymous]
        public IActionResult Public() => Ok(new { ok = true, message = "Público, não precisa de token." });

        // 2) Endpoint autenticado (qualquer usuário com token válido)
        [HttpGet("auth")]
        [Authorize(Roles = "WEB_GBAC_GESTOR-CONTEUDO")]
        public IActionResult Me()
        {
            var claims = User.Claims.Select(c => new { c.Type, c.Value });
            return Ok(new
            {
                message = "Autenticado com sucesso.",
                name = User.Identity?.Name,
                claims
            });
        }

        // 3) Endpoint protegido por ROLE (ex.: 'admin')
        [HttpGet("admin")]
        [Authorize(Roles = "WEB_GBAC_GESTOR-CONTEUDO")]
        public IActionResult OnlyAdmin() => Ok(new { message = "Você tem a role 'admin'." });

        // 4) Endpoint protegido por POLICY (ex.: 'api.read')
        [HttpGet("policy")]
        [Authorize(Policy = "api.read", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult PolicyProtected() => Ok(new { message = "Você possui o escopo/claim exigido pela policy 'api.read'." });
    }
}
