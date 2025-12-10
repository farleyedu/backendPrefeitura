using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Requests.Institucional.Base
{
    [ExcludeFromCodeCoverage]
    public class CreateInstitucionalRequest
    {
        public string Slug { get; set; } = string.Empty; 
        public string Titulo { get; set; } = string.Empty;
        public string? Subtitulo { get; set; }
        public string? Descricao { get; set; }
        public string? Resumo { get; set; }
        public string? AutorNome { get; set; }
        public string? Creditos { get; set; }
        public string? SeoTitulo { get; set; }
        public string? SeoDescricao { get; set; }
        public string? ImagemUrlPrincipal { get; set; }
        public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
        public DateTime? DataAtualizacao { get; set; }
        public DateTime? DataPublicacao { get; set; }
        public string Ativo { get; set; } = "S";
        public List<InstitucionalBlocoRequest> Blocos { get; set; } = new();
    }
}
