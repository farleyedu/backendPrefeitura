using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Requests.Fiscalizacao.PlanoAnualFiscalizacao
{
    [ExcludeFromCodeCoverage]
    public sealed class FiscalizacaoResolucaoAnexoRequest
    {
        public int Numero { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string? Descritivo { get; set; }
        public List<FiscalizacaoResolucaoTemaPrioritarioRequest>? TemasPrioritarios { get; set; }
        public List<FiscalizacaoResolucaoAtividadeRequest>? Atividades { get; set; }
        public List<FiscalizacaoResolucaoDistribuicaoRequest>? Distribuicao { get; set; }
    }
}
