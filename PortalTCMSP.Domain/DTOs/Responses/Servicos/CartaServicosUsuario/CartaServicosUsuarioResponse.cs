using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Responses.Servicos.CartaServicosUsuario
{
    namespace Application.DTO.Servicos.CartaServicosUsuarioDto
    {
        [ExcludeFromCodeCoverage]
        public sealed class CartaServicosUsuarioResponse
        {
            public long Id { get; set; }
            public string TituloPagina { get; set; } = string.Empty;
            public string TituloPesquisa { get; set; } = string.Empty;
            public string Slug { get; set; } = string.Empty;
            public bool Ativo { get; set; }
            public List<CartaServicosUsuarioServicoResponse> Servicos { get; set; } = [];
        }
    }
}
