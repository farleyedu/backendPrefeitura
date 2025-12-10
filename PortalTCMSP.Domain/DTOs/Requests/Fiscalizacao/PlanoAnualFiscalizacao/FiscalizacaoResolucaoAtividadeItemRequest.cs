using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Requests.Fiscalizacao.PlanoAnualFiscalizacao
{
    [ExcludeFromCodeCoverage]
    public sealed class FiscalizacaoResolucaoAtividadeItemRequest
    {
        public string Descricao { get; set; } = string.Empty;
        public int Quantidade { get; set; }
        public decimal DUSFs { get; set; }
    }
}
