using PortalTCMSP.Domain.Enum;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Responses.SessaoPlenaria.NotasTaquigraficas
{
    [ExcludeFromCodeCoverage]
    public sealed class SessaoPlenariaNotasTaquigraficasAnexoItemResponse
    {
        public long Id { get; set; }
        public string Link { get; set; } = string.Empty;
        public string? TipoArquivo { get; set; }
        public string? NomeExibicao { get; set; }
        public int Ordem { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public sealed class SessaoPlenariaNotasTaquigraficasResponse
    {
        public long Id { get; set; }
        public long? IdSessaoPlenaria { get; set; }
        public string Numero { get; set; } = string.Empty;
        public NotasTipo Tipo { get; set; }
        public DateTime? DataPublicacao { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataAtualizacao { get; set; }
        public List<SessaoPlenariaNotasTaquigraficasAnexoItemResponse> Anexos { get; set; } = [];
    }
}
