using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Responses.Fiscalizacao.RelatorioFiscalizacao
{
    [ExcludeFromCodeCoverage]
    public sealed class FiscalizacaoRelatorioFiscalizacaoImagemAnexaResponse
    {
        public long Id { get; set; }
        public string? ImagemUrl { get; set; }
        public string? NomeExibicao { get; set; }
        public int Ordem { get; set; }
    }
}
