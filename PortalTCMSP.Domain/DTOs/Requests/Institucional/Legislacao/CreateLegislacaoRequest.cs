using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Requests.Institucional.Legislacao
{
    [ExcludeFromCodeCoverage]
    public class CreateLegislacaoRequest
    {
        public string Slug { get; set; } = string.Empty;
        public string Titulo { get; set; } = "Legislação";
        public string? Subtitulo { get; set; }
        public string? Descricao { get; set; }
        public string? AutorNome { get; set; }
        public string? Creditos { get; set; }
        public string? SeoTitulo { get; set; }
        public string? SeoDescricao { get; set; }
        public DateTime? DataCriacao { get; set; }
        public DateTime? DataAtualizacao { get; set; }
        public DateTime? DataPublicacao { get; set; }
        public string Ativo { get; set; } = "S";

        public List<CreateLegislacaoBlocoRequest> Blocos { get; set; } = [];
    }

    [ExcludeFromCodeCoverage]
    public class CreateLegislacaoBlocoRequest
    {
        public int Ordem { get; set; }
        public string? Html { get; set; }  
        public string? Titulo { get; set; }
        public List<CreateLegislacaoAnexoRequest> Anexos { get; set; } = [];
    }

    [ExcludeFromCodeCoverage]
    public class CreateLegislacaoAnexoRequest
    {
        public int Ordem { get; set; }
        public string Link { get; set; } = string.Empty;
    }
}
