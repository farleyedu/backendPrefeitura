using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Requests.Fiscalizacao.PlanoAnualFiscalizacao
{
    [ExcludeFromCodeCoverage]
    public sealed class FiscalizacaoResolucaoAtaRequest
    {
        public int Ordem { get; set; }
        public string TituloAta { get; set; } = string.Empty;
        public bool TituloAtaAEsquerda { get; set; }
        public string ConteudoAta { get; set; } = string.Empty;
    }
}
