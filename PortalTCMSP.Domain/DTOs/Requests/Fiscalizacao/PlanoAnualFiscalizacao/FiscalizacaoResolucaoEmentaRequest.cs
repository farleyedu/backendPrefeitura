using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Requests.Fiscalizacao.PlanoAnualFiscalizacao
{
    [ExcludeFromCodeCoverage]
    public sealed class FiscalizacaoResolucaoEmentaRequest
    {
        public string Descritivo { get; set; } = string.Empty;
        public List<string>? LinksArtigos { get; set; }
    }
}
