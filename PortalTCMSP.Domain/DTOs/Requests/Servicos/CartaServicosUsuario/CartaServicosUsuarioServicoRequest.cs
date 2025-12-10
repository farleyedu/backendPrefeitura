using PortalTCMSP.Domain.DTOs.Requests.Servicos.CartaServicosUsuario.Application.DTO.Servicos.CartaServicosUsuarioDto;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Requests.Servicos.CartaServicosUsuario
{
    [ExcludeFromCodeCoverage]
    public sealed class CartaServicosUsuarioServicoRequest
    {
        public bool Ativo { get; set; } = true;
        public int Ordem { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public List<CartaServicosUsuarioServicoItemRequest> ServicosItens { get; set; } = [];
    }
}
