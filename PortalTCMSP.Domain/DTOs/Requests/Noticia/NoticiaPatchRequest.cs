using PortalTCMSP.Domain.DTOs.Requests.NoticiaRequest;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Requests.Noticia
{
    [ExcludeFromCodeCoverage]
    public class NoticiaPatchRequest
    {
        public string? Slug { get; set; }
        public string? Titulo { get; set; }
        public string? Subtitulo { get; set; }
        public string? Resumo { get; set; }
        public string? AutorNome { get; set; }
        public string? Creditos { get; set; }
        public string? SeoTitulo { get; set; }
        public string? SeoDescricao { get; set; }
        public string? OgImageUrl { get; set; }
        public string? Canonical { get; set; }
        public DateTime? DataPublicacao { get; set; }
        public bool? Ativo { get; set; }
        public bool? Destaque { get; set; }
        public List<long>? CategoriaIds { get; set; }
        public string[]? Tags { get; set; }
        public string[]? CategoriasExtras { get; set; }
        public string? ImagemUrl { get; set; }
        public string? ConteudoNoticia { get; set; }
        public List<NoticiaBlocoCreateRequest>? Blocos { get; set; }
    }
}
