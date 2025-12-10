using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Responses.Fiscalizacao.PlanoAnualFiscalizacao
{
    [ExcludeFromCodeCoverage]
    public sealed class FiscalizacaoResolucaoAtaResponse
    {
        public long Id { get; set; }
        public int Ordem { get; set; }
        public string TituloAta { get; set; } = string.Empty;
        public bool TituloAtaAEsquerda { get; set; }
        public string ConteudoAta { get; set; } = string.Empty;
    }
}
