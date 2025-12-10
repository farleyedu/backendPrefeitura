using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Requests.Fiscalizacao.PlanoAnualFiscalizacao
{
    [ExcludeFromCodeCoverage]
    public sealed class FiscalizacaoResolucaoAtividadeRequest
    {
        public string Tipo { get; set; } = string.Empty;
        public List<FiscalizacaoResolucaoAtividadeItemRequest>? AtividadeItem { get; set; }
    }
}
