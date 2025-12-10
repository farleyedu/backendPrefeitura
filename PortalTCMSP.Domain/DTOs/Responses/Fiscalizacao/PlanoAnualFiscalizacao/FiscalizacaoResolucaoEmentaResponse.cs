using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Responses.Fiscalizacao.PlanoAnualFiscalizacao
{
    [ExcludeFromCodeCoverage]
    public sealed class FiscalizacaoResolucaoEmentaResponse
    {
        public long Id { get; set; }
        public string Descritivo { get; set; } = string.Empty;
        public List<string> LinksArtigos { get; set; } = [];
    }
}
