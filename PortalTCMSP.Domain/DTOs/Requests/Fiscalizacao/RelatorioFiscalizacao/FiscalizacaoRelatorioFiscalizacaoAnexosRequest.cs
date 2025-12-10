using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Requests.Fiscalizacao.RelatorioFiscalizacao
{
    [ExcludeFromCodeCoverage]
    public sealed class FiscalizacaoRelatorioFiscalizacaoAnexosRequest
    {
        public List<FiscalizacaoRelatorioFiscalizacaoDocumentoAnexoRequest>? DocumentosAnexos { get; set; }
        public List<FiscalizacaoRelatorioFiscalizacaoImagemAnexaRequest>? ImagensAnexas { get; set; }
    }
}
