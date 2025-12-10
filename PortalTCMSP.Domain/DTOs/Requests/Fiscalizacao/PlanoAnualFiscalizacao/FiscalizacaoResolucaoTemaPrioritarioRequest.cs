using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Requests.Fiscalizacao.PlanoAnualFiscalizacao
{
    [ExcludeFromCodeCoverage]
    public sealed class FiscalizacaoResolucaoTemaPrioritarioRequest
    {
        public int Ordem { get; set; }
        public string Tema { get; set; } = string.Empty;
        public string? Descricao { get; set; }
    }
}
