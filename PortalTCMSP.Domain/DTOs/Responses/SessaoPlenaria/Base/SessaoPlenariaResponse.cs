using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Responses.SessaoPlenaria.Base
{
    [ExcludeFromCodeCoverage]
    public sealed class SessaoPlenariaResponse
    {
        public long Id { get; set; }
        public string Slug { get; set; } = string.Empty;
        public string Titulo { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public string? SeoTitulo { get; set; }
        public string? SeoDescricao { get; set; }
        public string? UrlVideo { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataAtualizacao { get; set; }
        public DateTime? DataPublicacao { get; set; }
        public bool Ativo { get; set; }  
        public int QtdePautas { get; set; }
        public int QtdeAtas { get; set; }
        public int QtdeNotas { get; set; }
    }
}
