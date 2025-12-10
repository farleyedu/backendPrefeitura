using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Responses.Fiscalizacao.PlanoAnualFiscalizacao
{
    [ExcludeFromCodeCoverage]
    public sealed class FiscalizacaoResolucaoDispositivoResponse
    {
        public long Id { get; set; }
        public int Ordem { get; set; }
        public string Artigo { get; set; } = string.Empty;
        public List<FiscalizacaoResolucaoParagrafoResponse> Paragrafo { get; set; } = [];
        public List<FiscalizacaoResolucaoIncisoResponse> Incisos { get; set; } = [];
    }
}
