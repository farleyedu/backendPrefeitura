using PortalTCMSP.Domain.Entities.BaseEntity;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.Entities.NoticiaEntity
{
    [ExcludeFromCodeCoverage]
    public class Noticia : Entity
    {
        public string Slug { get; set; } = default!;
        public string Titulo { get; set; } = default!;
        public string? Subtitulo { get; set; }
        public string? Resumo { get; set; }
        public ICollection<NoticiaCategoria> NoticiaCategorias { get; set; } = new List<NoticiaCategoria>();
        public string[]? Tags { get; set; }
        public string[]? CategoriasExtras { get; set; }
        public Autoria? Autoria { get; set; } = new();
        public Metadados? Metadados { get; set; } = new();
        public DateTime DataPublicacao { get; set; } = DateTime.UtcNow;
        public bool Ativo { get; set; } = true;
        public bool Destaque { get; set; } = false;
        public int Visualizacao { get; set; } = 0;
        public string? ImagemUrl { get; set; }
        public string? ConteudoNoticia { get; set; }
        public Auditoria? Auditoria { get; set; } = new();
        public ICollection<NoticiaBloco> Blocos { get; set; } = new List<NoticiaBloco>();
    }
}
