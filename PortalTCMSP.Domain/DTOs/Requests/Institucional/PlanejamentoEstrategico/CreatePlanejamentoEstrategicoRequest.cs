using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Requests.Institucional.PlanejamentoEstrategico
{
    [ExcludeFromCodeCoverage]
    public class CreatePlanejamentoEstrategicoRequest
    {
        public string Slug { get; set; } = string.Empty;
        public string Titulo { get; set; } = "Planejamento Estratégico";
        public string? Subtitulo { get; set; }
        public string? Descricao { get; set; }
        public string? AutorNome { get; set; }
        public string? Creditos { get; set; }
        public string? SeoTitulo { get; set; }
        public string? SeoDescricao { get; set; }
        public DateTime? DataCriacao { get; set; }
        public DateTime? DataAtualizacao { get; set; }
        public string Ativo { get; set; } = "S";

        public List<CreatePlanejamentoEstrategicoBlocoRequest> Blocos { get; set; } = [];
    }

    [ExcludeFromCodeCoverage]
    public class CreatePlanejamentoEstrategicoBlocoRequest
    {
        public int Ordem { get; set; }
        public string? Titulo { get; set; }
        public List<CreatePlanejamentoEstrategicoDescricaoRequest> Descricoes { get; set; } = [];
    }

    [ExcludeFromCodeCoverage]
    public class CreatePlanejamentoEstrategicoDescricaoRequest
    {
        public int Ordem { get; set; }
        public string Texto { get; set; } = string.Empty;
    }
}
