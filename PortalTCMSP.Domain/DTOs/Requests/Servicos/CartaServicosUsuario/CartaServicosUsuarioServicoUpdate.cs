using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Requests.Servicos.CartaServicosUsuario
{
    [ExcludeFromCodeCoverage]
    public sealed class CartaServicosUsuarioServicoUpdate
    {
        public long Id { get; set; }
        public bool Ativo { get; set; } = true;
        public int Ordem { get; set; }
        public string Titulo { get; set; } = string.Empty;
        //public List<CartaServicosUsuarioServicoItemRequest> ServicosItens { get; set; } = [];
    }
}
