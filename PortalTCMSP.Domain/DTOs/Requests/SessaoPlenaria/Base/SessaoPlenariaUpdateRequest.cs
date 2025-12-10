using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Requests.SessaoPlenaria.Base
{
    [ExcludeFromCodeCoverage]
    public sealed class SessaoPlenariaUpdateRequest
    {
        public string? Slug { get; set; } = string.Empty;
        public string? Titulo { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public string? SeoTitulo { get; set; }
        public string? SeoDescricao { get; set; }
        public string? UrlVideo { get; set; }
        public DateTime? DataPublicacao { get; set; }
        public bool? Ativo { get; set; } = false;
    }
}
