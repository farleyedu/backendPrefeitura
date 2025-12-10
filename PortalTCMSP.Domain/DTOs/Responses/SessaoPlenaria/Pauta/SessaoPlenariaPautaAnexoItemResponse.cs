using PortalTCMSP.Domain.Enum;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Responses.SessaoPlenaria.Pauta
{
    [ExcludeFromCodeCoverage]
    public sealed class SessaoPlenariaPautaAnexoItemResponse
    {
        public long Id { get; set; }
        public string Link { get; set; } = string.Empty;
        public string? TipoArquivo { get; set; }
        public string? NomeExibicao { get; set; }
        public int Ordem { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public sealed class SessaoPlenariaPautaResponse
    {
        public long Id { get; set; }
        public long? IdSessaoPlenaria { get; set; }
        public string Numero { get; set; } = string.Empty;
        public PautaTipo? Tipo { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataDaSesao { get; set; }
        public DateTime? DataPublicacao { get; set; }
        public DateTime? DataAtualizacao { get; set; }
        public List<SessaoPlenariaPautaAnexoItemResponse> Anexos { get; set; } = [];
    }
}
