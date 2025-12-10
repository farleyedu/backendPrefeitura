using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Requests.Servicos.CartaServicosUsuario
{
    namespace Application.DTO.Servicos.CartaServicosUsuarioDto
    {
        [ExcludeFromCodeCoverage]
        public sealed class CartaServicosUsuarioRequest
        {
            public string TituloPagina { get; set; } = string.Empty;
            public string TituloPesquisa { get; set; } = string.Empty;
            public string Slug { get; set; } = string.Empty;
            public bool Ativo { get; set; } = true;
            //public List<CartaServicosUsuarioServicoRequest> Servicos { get; set; } = [];
        }
    }
}
