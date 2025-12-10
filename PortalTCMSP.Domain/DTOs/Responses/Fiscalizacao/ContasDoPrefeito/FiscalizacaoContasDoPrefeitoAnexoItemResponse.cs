using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Responses.Fiscalizacao.ContasDoPrefeito
{
    [ExcludeFromCodeCoverage]
    public sealed class FiscalizacaoContasDoPrefeitoAnexoItemResponse
    {
        public long Id { get; set; }
        public string Link { get; set; } = string.Empty;
        public string? TipoArquivo { get; set; }
        public string? NomeExibicao { get; set; }
        public int Ordem { get; set; }
    }
}
