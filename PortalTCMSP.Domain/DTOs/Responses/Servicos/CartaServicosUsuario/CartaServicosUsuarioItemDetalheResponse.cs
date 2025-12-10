using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Responses.Servicos.CartaServicosUsuario
{
    [ExcludeFromCodeCoverage]
    public sealed class CartaServicosUsuarioItemDetalheResponse
    {
        public long Id { get; set; }
        public bool Ativo { get; set; }
        public int Ordem { get; set; }
        public string TituloDetalhe { get; set; } = string.Empty;
        public List<CartaServicosUsuarioDescritivoItemDetalheResponse> DescritivoItemDetalhe { get; set; } = [];
    }
}
