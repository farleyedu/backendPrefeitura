using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Requests.Institucional.Competencias
{
    [ExcludeFromCodeCoverage]
    public class CreateCompetenciasRequest
    {
        public string Slug { get; set; } = string.Empty;
        public string Titulo { get; set; } = "Competências";
        public string? Subtitulo { get; set; }
        public string? Descricao { get; set; }
        public string? AutorNome { get; set; }
        public string? Creditos { get; set; }
        public string? SeoTitulo { get; set; }
        public string? SeoDescricao { get; set; }
        public DateTime? DataCriacao { get; set; }
        public DateTime? DataAtualizacao { get; set; }
        public string Ativo { get; set; } = "S";

        public List<CreateCompetenciasBlocoRequest> Blocos { get; set; } = [];
    }

    [ExcludeFromCodeCoverage]
    public class CreateCompetenciasBlocoRequest
    {
        public int Ordem { get; set; }
        public string? Titulo { get; set; }
        public List<CreateCompetenciasDescricaoRequest> Descricoes { get; set; } = [];
        public List<CreateCompetenciasAnexoRequest> Anexos { get; set; } = [];
    }

    [ExcludeFromCodeCoverage]
    public class CreateCompetenciasDescricaoRequest
    {
        public int Ordem { get; set; }
        public string Texto { get; set; } = string.Empty;
    }

    [ExcludeFromCodeCoverage]
    public class CreateCompetenciasAnexoRequest
    {
        public int Ordem { get; set; }
        public string Link { get; set; } = string.Empty;
    }
}
