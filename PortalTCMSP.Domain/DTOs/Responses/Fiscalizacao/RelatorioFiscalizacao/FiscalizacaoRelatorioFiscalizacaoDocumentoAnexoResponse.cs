using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Responses.Fiscalizacao.RelatorioFiscalizacao
{
    [ExcludeFromCodeCoverage]
    public sealed class FiscalizacaoRelatorioFiscalizacaoDocumentoAnexoResponse
    {
        public long Id { get; set; }
        public string? Link { get; set; }
        public string? TipoArquivo { get; set; }
        public string? NomeExibicao { get; set; }
        public int Ordem { get; set; }
    }
}
