using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Requests.Institucional.Relatorias
{
    [ExcludeFromCodeCoverage]
    public class UpdateRelatoriasRequest
    {
        public string Slug { get; set; } = string.Empty;
        public string Titulo { get; set; } = "Relatorias";
        public string? Subtitulo { get; set; }
        public string? Descricao { get; set; }
        public string? AutorNome { get; set; }
        public string? Creditos { get; set; }
        public string? SeoTitulo { get; set; }
        public string? SeoDescricao { get; set; }
        public DateTime? DataAtualizacao { get; set; } = DateTime.UtcNow;
        public string Ativo { get; set; } = "S";
        public List<UpdateRelatoriasBlocoRequest> Blocos { get; set; } = [];
    }

    [ExcludeFromCodeCoverage]
    public class UpdateRelatoriasBlocoRequest
    {
        public long? Id { get; set; } 
        public int Ordem { get; set; }
        public string? Titulo { get; set; }
        public string? Subtitulo { get; set; }
        public List<UpdateRelatoriasDescricaoRequest> Descricoes { get; set; } = [];
        public string? ImagemUrl { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class UpdateRelatoriasDescricaoRequest
    {
        public long? Id { get; set; }
        public int Ordem { get; set; }
        public string Texto { get; set; } = string.Empty;
        public List<UpdateRelatoriasSubtextoRequest>? Subtextos { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class UpdateRelatoriasSubtextoRequest
    {
        public long? Id { get; set; }
        public int Ordem { get; set; }
        public string Texto { get; set; } = string.Empty;
    }
}
