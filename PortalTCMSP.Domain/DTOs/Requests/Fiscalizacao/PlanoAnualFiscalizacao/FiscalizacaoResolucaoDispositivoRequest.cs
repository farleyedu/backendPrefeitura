using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Requests.Fiscalizacao.PlanoAnualFiscalizacao
{
    [ExcludeFromCodeCoverage]
    public sealed class FiscalizacaoResolucaoDispositivoRequest
    {
        public int Ordem { get; set; }
        public string Artigo { get; set; } = string.Empty;
        public List<FiscalizacaoResolucaoParagrafoRequest>? Paragrafo { get; set; }
        public List<FiscalizacaoResolucaoIncisoRequest>? Incisos { get; set; }
    }
}
