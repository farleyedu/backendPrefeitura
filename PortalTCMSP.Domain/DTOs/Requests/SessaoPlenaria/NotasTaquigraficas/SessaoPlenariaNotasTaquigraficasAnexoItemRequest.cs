using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Requests.SessaoPlenaria.NotasTaquigraficas
{
    [ExcludeFromCodeCoverage]
    public sealed class SessaoPlenariaNotasTaquigraficasAnexoItemRequest
    {
        public string Link { get; set; } = string.Empty;
        public string? TipoArquivo { get; set; }
        public string? NomeExibicao { get; set; }
        public int Ordem { get; set; } = 0;
    }
}
