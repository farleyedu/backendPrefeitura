using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Requests.Servicos.CartaServicosUsuario
{
    [ExcludeFromCodeCoverage]
    public sealed class CartaServicosUsuarioDescritivoItemDetalheUpdate
    {
        public long Id { get; set; }
        public int Ordem { get; set; }
        public string Descritivo { get; set; } = string.Empty;
    }
}
