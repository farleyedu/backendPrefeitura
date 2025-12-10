using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Requests.Institucional.Competencias
{
    [ExcludeFromCodeCoverage]
    public class UpdateCompetenciasRequest
    {
        public string Slug { get; set; } = string.Empty;
        public string Titulo { get; set; } = "Competências";
        public string? Subtitulo { get; set; }
        public string? Descricao { get; set; }
        public string? AutorNome { get; set; }
        public string? Creditos { get; set; }
        public string? SeoTitulo { get; set; }
        public string? SeoDescricao { get; set; }
        public DateTime? DataAtualizacao { get; set; } = DateTime.UtcNow;
        public string Ativo { get; set; } = "S";

        public List<UpdateCompetenciasBlocoRequest> Blocos { get; set; } = [];
    }

    [ExcludeFromCodeCoverage]
    public class UpdateCompetenciasBlocoRequest
    {
        public long? Id { get; set; } 
        public int Ordem { get; set; }
        public string? Titulo { get; set; }
        public List<UpdateCompetenciasDescricaoRequest> Descricoes { get; set; } = [];
        public List<UpdateCompetenciasAnexoRequest> Anexos { get; set; } = [];
    }

    [ExcludeFromCodeCoverage]
    public class UpdateCompetenciasDescricaoRequest
    {
        public long? Id { get; set; }
        public int Ordem { get; set; }
        public string Texto { get; set; } = string.Empty;
    }

    [ExcludeFromCodeCoverage]
    public class UpdateCompetenciasAnexoRequest
    {
        public long? Id { get; set; }
        public int Ordem { get; set; }
        public string Link { get; set; } = string.Empty;
    }
}
