using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Responses.Institucional.Competencias
{
    [ExcludeFromCodeCoverage]
    public class CompetenciasResponse
    {
        public long Id { get; set; }
        public string Slug { get; set; } = string.Empty;
        public string Titulo { get; set; } = string.Empty;
        public string? Subtitulo { get; set; }
        public string? Descricao { get; set; }
        public string? AutorNome { get; set; }
        public string? Creditos { get; set; }
        public string? SeoTitulo { get; set; }
        public string? SeoDescricao { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataAtualizacao { get; set; }
        public string Ativo { get; set; } = "S";
        public List<CompetenciasBlocoResponse> Blocos { get; set; } = [];
    }

    [ExcludeFromCodeCoverage]
    public class CompetenciasBlocoResponse
    {
        public long Id { get; set; }
        public int Ordem { get; set; }
        public string? Titulo { get; set; }
        public List<CompetenciasDescricaoResponse> Descricoes { get; set; } = [];
        public List<CompetenciasAnexoResponse> Anexos { get; set; } = [];
    }

    [ExcludeFromCodeCoverage]
    public class CompetenciasDescricaoResponse
    {
        public long Id { get; set; }
        public int Ordem { get; set; }
        public string Texto { get; set; } = string.Empty;
    }

    [ExcludeFromCodeCoverage]
    public class CompetenciasAnexoResponse
    {
        public long Id { get; set; }
        public int Ordem { get; set; }
        public string Link { get; set; } = string.Empty;
    }
}
