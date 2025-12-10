using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Responses.Servicos.CartaServicosUsuario
{
    [ExcludeFromCodeCoverage]
    public sealed class CartaServicosUsuarioDescritivoItemDetalheResponse
    {
        public long Id { get; set; }
        public int Ordem { get; set; }
        public string Descritivo { get; set; } = string.Empty;
    }
}
