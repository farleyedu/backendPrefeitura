using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Requests.Fiscalizacao.RelatorioFiscalizacao
{
    [ExcludeFromCodeCoverage]
    public sealed class FiscalizacaoRelatorioFiscalizacaoImagemAnexaRequest
    {
        public string? ImagemUrl { get; set; }
        public string? NomeExibicao { get; set; }
        public int Ordem { get; set; }
    }
}
