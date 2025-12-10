using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Requests.Fiscalizacao.PlanoAnualFiscalizacao
{
    [ExcludeFromCodeCoverage]
    public sealed class FiscalizacaoResolucaoParagrafoRequest
    {
        public int Ordem { get; set; }
        public string Texto { get; set; } = string.Empty;
    }
}
