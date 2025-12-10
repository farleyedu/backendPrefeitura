using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Responses.Fiscalizacao.PlanoAnualFiscalizacao
{
    [ExcludeFromCodeCoverage]
    public sealed class FiscalizacaoResolucaoAtividadeItemResponse
    {
        public string Descricao { get; set; } = string.Empty;
        public int Quantidade { get; set; }
        public decimal DUSFs { get; set; }
    }
}
