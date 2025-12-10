using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Responses.Servicos.CartaServicosUsuario
{
    [ExcludeFromCodeCoverage]
    public sealed class CartaServicosUsuarioServicoResponse
    {
        public long Id { get; set; }
        public bool Ativo { get; set; }
        public int Ordem { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public List<CartaServicosUsuarioServicoItemResponse> ServicosItens { get; set; } = [];
    }
}
