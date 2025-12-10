using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Responses.Fiscalizacao.PlanoAnualFiscalizacao
{
    [ExcludeFromCodeCoverage]
    public sealed class FiscalizacaoResolucaoAtividadeResponse
    {
        public long Id { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public List<FiscalizacaoResolucaoAtividadeItemResponse> AtividadeItem { get; set; } = new();
    }
}
