using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Responses.Fiscalizacao.PlanoAnualFiscalizacao
{
    [ExcludeFromCodeCoverage]
    public sealed class FiscalizacaoResolucaoTemaPrioritarioResponse
    {
        public long Id { get; set; }
        public int Ordem { get; set; }
        public string Tema { get; set; } = string.Empty;
        public string? Descricao { get; set; }
    }
}
