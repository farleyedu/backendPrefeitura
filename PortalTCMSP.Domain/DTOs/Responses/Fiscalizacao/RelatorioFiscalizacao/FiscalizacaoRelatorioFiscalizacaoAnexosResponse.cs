using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Responses.Fiscalizacao.RelatorioFiscalizacao
{
    [ExcludeFromCodeCoverage]
    public sealed class FiscalizacaoRelatorioFiscalizacaoAnexosResponse
    {
        public List<FiscalizacaoRelatorioFiscalizacaoDocumentoAnexoResponse> DocumentosAnexos { get; set; } = [];
        public List<FiscalizacaoRelatorioFiscalizacaoImagemAnexaResponse> ImagensAnexas { get; set; } = [];
    }
}
