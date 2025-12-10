using Microsoft.AspNetCore.Mvc;
using PortalTCMSP.Domain.Services.Fiscalizacao;

namespace PortalTCMSP.Api.Controllers.Fiscalizacao
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "Fiscalizacao")]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class FiscalizacaoController(IFiscalizacaoRelatoriosFiscalizacaoService service) : ControllerBase
    {
        private readonly IFiscalizacaoRelatoriosFiscalizacaoService _service = service;
    }
}
