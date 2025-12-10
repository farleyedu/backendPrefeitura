using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Requests.Fiscalizacao.PlanoAnualFiscalizacao
{
    [ExcludeFromCodeCoverage]
    public sealed class FiscalizacaoResolucaoDistribuicaoRequest
    {
        public string TipoFiscalizacao { get; set; } = string.Empty;
        public int TotalPAF { get; set; }
        public decimal DezPorCentoTotalPAF { get; set; }
        public int LimitePorConselheiro { get; set; }
        public int LimiteConselheiros { get; set; }
        public int LimitePlenoeCameras { get; set; }
        public int LimitePresidente { get; set; }
        public int ListaDePrioridades { get; set; }
    }
}
