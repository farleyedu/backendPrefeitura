using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Responses.Fiscalizacao.PlanoAnualFiscalizacao
{
    [ExcludeFromCodeCoverage]
    public sealed class FiscalizacaoResolucaoAnexoResponse
    {
        public long Id { get; set; }
        public int Numero { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string? Descritivo { get; set; }
        public List<FiscalizacaoResolucaoTemaPrioritarioResponse> TemasPrioritarios { get; set; } = [];
        public List<FiscalizacaoResolucaoAtividadeResponse> Atividades { get; set; } = [];
        public List<FiscalizacaoResolucaoDistribuicaoResponse> Distribuicao { get; set; } = [];
    }
}
