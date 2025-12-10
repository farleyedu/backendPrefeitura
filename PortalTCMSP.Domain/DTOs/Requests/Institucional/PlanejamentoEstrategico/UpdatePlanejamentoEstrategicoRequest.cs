using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Requests.Institucional.PlanejamentoEstrategico
{
    [ExcludeFromCodeCoverage]
    public class UpdatePlanejamentoEstrategicoRequest
    {
        public string Slug { get; set; } = string.Empty;
        public string Titulo { get; set; } = "Planejamento Estratégico";
        public string? Subtitulo { get; set; }
        public string? Descricao { get; set; }
        public string? AutorNome { get; set; }
        public string? Creditos { get; set; }
        public string? SeoTitulo { get; set; }
        public string? SeoDescricao { get; set; }
        public DateTime? DataAtualizacao { get; set; } = DateTime.UtcNow;
        public string Ativo { get; set; } = "S";

        public List<UpdatePlanejamentoEstrategicoBlocoRequest> Blocos { get; set; } = [];
    }

    [ExcludeFromCodeCoverage]
    public class UpdatePlanejamentoEstrategicoBlocoRequest
    {
        public long? Id { get; set; } 
        public int Ordem { get; set; }
        public string? Titulo { get; set; }
        public List<UpdatePlanejamentoEstrategicoDescricaoRequest> Descricoes { get; set; } = [];
    }

    [ExcludeFromCodeCoverage]
    public class UpdatePlanejamentoEstrategicoDescricaoRequest
    {
        public long? Id { get; set; }
        public int Ordem { get; set; }
        public string Texto { get; set; } = string.Empty;
    }
}
