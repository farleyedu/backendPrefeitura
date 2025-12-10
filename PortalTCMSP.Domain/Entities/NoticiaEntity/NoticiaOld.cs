using PortalTCMSP.Domain.Entities.BaseEntity;

namespace PortalTCMSP.Domain.Entities.NoticiaEntity
{
    public class NoticiaOld : Entity
    {
        public string Slug { get; set; } = default!;
        public string Titulo { get; set; } = default!;
        public string? Subtitulo { get; set; }
        public string? Resumo { get; set; }
        public string[]? Tags { get; set; }
        public string[]? CategoriasExtras { get; set; }
        public string? Autor_Nome { get; set; }
        public string? Autor_Creditos { get; set; }
        public string? Seo_Title { get; set; }
        public string? Seo_Description { get; set; }
        public string? Og_Image_Url { get; set; }
        public string? Canonical { get; set; }
        public DateTime DataPublicacao { get; set; } = DateTime.UtcNow;
        public bool Ativo { get; set; } = true;
        public bool Destaque { get; set; } = false;
        public int Visualizacao { get; set; } = 0;
        public string? ImagemUrl { get; set; }
        public string? ConteudoNoticia { get; set; }
        public DateTime? Criado_Em { get; set; }
        public string? Criado_Por { get; set; }
        public DateTime? Atualizado_Em { get; set; }
        public string? Atualizado_Por { get; set; }
        public int? Versao { get; set; }
        public long? CategoriaId { get; set; }
    }
}
