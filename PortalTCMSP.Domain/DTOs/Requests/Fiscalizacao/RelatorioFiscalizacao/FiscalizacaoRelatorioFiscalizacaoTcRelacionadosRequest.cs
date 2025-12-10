using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Requests.Fiscalizacao.RelatorioFiscalizacao
{
    [ExcludeFromCodeCoverage]
    public sealed class FiscalizacaoRelatorioFiscalizacaoTcRelacionadosRequest
    {
        public List<FiscalizacaoRelatorioFiscalizacaoTcRequest>? Tc { get; set; }
    }
}
