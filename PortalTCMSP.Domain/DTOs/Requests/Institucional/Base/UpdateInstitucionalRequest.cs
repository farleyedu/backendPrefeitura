using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Requests.Institucional.Base
{
    [ExcludeFromCodeCoverage]
    public class UpdateInstitucionalRequest
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
        public DateTime? DataAtualizacao { get; set; } = DateTime.UtcNow;
        public DateTime? DataPublicacao { get; set; }
        public string Ativo { get; set; } = "S";
        public List<UpdateInstitucionalBlocoRequest> Blocos { get; set; } = [];
    }

    [ExcludeFromCodeCoverage]
    public class UpdateInstitucionalBlocoRequest
    {
        public long? Id { get; set; } 
        public int Ordem { get; set; }
        public string? Html { get; set; }
        public string? Titulo { get; set; }
        public string? Subtitulo { get; set; }
        public string Ativo { get; set; } = "S";
        public List<UpdateInstitucionalBlocoDescricaoRequest> Descricoes { get; set; } = [];
        public List<UpdateInstitucionalBlocoAnexoRequest> Anexos { get; set; } = [];
    }

    [ExcludeFromCodeCoverage]
    public class UpdateInstitucionalBlocoDescricaoRequest
    {
        public long? Id { get; set; } 
        public int Ordem { get; set; }
        public string Texto { get; set; } = string.Empty;
        public List<UpdateInstitucionalBlocoSubtextoRequest> Subtextos { get; set; } = [];
    }

    [ExcludeFromCodeCoverage]
    public class UpdateInstitucionalBlocoSubtextoRequest
    {
        public long? Id { get; set; }
        public int Ordem { get; set; }
        public string Texto { get; set; } = string.Empty;
    }

    [ExcludeFromCodeCoverage]
    public class UpdateInstitucionalBlocoAnexoRequest
    {
        public long? Id { get; set; } 
        public int Ordem { get; set; }
        public string Link { get; set; } = string.Empty;
    }
}
