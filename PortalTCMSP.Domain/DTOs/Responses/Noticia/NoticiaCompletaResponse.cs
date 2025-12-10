using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace PortalTCMSP.Domain.DTOs.Responses.Noticia
{
    [ExcludeFromCodeCoverage]
    public class NoticiaCompletaResponse
    {
        public long Id { get; set; }
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
        public bool Ativo { get; set; }
        public bool Destaques { get; set; }
        public List<CategoriaItem> Categorias { get; set; } = new();
        public string[]? Tags { get; set; }
        public string[]? CategoriasExtras { get; set; }
        public string? ImageUrl { get; set; }
        public string? ConteudoNoticia { get; set; }

        public List<BlocoResponse> Blocos { get; set; } = [];
        public DateTime PublicadoQuando { get; set; }
        public DateTime CriadoEm { get; set; }
        public string? CriadoPor { get; set; }
        public DateTime AtualizadoEm { get; set; }
        public string? AtualizadoPor { get; set; }
        public int Versao { get; set; }
    }
    public class BlocoResponse
    {
        public int Ordem { get; set; }
        public string Tipo { get; set; } = default!;
        public JsonElement? Config { get; set; }
        public JsonElement Valor { get; set; }
    }
}
