using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Responses.Noticia
{
    [ExcludeFromCodeCoverage]
    public class NoticiaResponse
    {
        public long Id { get; set; }
        public string Slug { get; set; } = default!;
        public string Titulo { get; set; } = default!;
        public string? Subtitulo { get; set; }
        public string? Resumo { get; set; }
        public DateTime PublicadoQuando { get; set; }
        public string? Autor { get; set; }
        public List<CategoriaItem> Categorias { get; set; } = new();
        public int Views { get; set; }
        public string? ImageUrl { get; set; }
        public bool Ativo { get; set; }
        public bool Destaques { get; set; }
        public string ConteudoNoticia { get; set; } = string.Empty;
    }

    [ExcludeFromCodeCoverage]
    public class CategoriaItem
    {
        public long Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string? Slug { get; set; }
    }
}
