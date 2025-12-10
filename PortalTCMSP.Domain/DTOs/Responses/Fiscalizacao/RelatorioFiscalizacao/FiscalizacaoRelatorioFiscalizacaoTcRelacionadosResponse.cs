using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Responses.Fiscalizacao.RelatorioFiscalizacao
{
    [ExcludeFromCodeCoverage]
    public sealed class FiscalizacaoRelatorioFiscalizacaoTcRelacionadosResponse 
    { 
        public List<FiscalizacaoRelatorioFiscalizacaoTcResponse> Tc { get; set; } = []; 
    }
}
