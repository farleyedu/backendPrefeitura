using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Requests.Fiscalizacao.RelatorioFiscalizacao
{
    [ExcludeFromCodeCoverage]
    public sealed class FiscalizacaoRelatorioFiscalizacaoTcRequest
    {
        public string? Link { get; set; }
        public string? Descritivo { get; set; }
        public int Ordem { get; set; }
    }
}
