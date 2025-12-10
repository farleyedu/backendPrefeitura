using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Responses.Fiscalizacao.RelatorioFiscalizacao
{
    [ExcludeFromCodeCoverage]
    public sealed class FiscalizacaoRelatorioFiscalizacaoTcResponse 
    { 
        public long Id { get; set; } 
        public string? Link { get; set; } 
        public string? Descritivo { get; set; } 
        public int Ordem { get; set; } 
    }
}
