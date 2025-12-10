using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Requests.NoticiaRequest
{
    [ExcludeFromCodeCoverage]
    public class NoticiaCreateRequest
    {
        public string Slug { get; set; } = default!;
        public string Titulo { get; set; } = default!;
        public string? Subtitulo { get; set; }
        public string? Resumo { get; set; }
        public string? AutorNome { get; set; }
        public string? Creditos { get; set; }
        public string? SeoTitulo { get; set; }
        public string? SeoDescricao { get; set; }
        public string? OgImageUrl { get; set; }
        public string? Canonical { get; set; }
        public DateTime? DataPublicacao { get; set; }
        public bool Ativo { get; set; } = true;
        public bool Destaque { get; set; } = false;

        [Required, MinLength(1)]
        public List<long> CategoriaIds { get; set; } = new();
        public string[]? Tags { get; set; }
        public string[]? CategoriasExtras { get; set; }
        public string? ImagemUrl { get; set; }
        public string? ConteudoNoticia { get; set; }
        public List<NoticiaBlocoCreateRequest>? Blocos { get; set; }
    }
}
