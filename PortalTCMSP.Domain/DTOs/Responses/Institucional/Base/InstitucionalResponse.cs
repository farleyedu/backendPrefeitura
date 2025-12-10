using PortalTCMSP.Domain.DTOs.Responses.Institucional.Relatorias;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Responses.Institucional.Base
{
    [ExcludeFromCodeCoverage]
    public class InstitucionalResponse
    {
        public long Id { get; set; }
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
        public DateTime DataCriacao { get; set; }
        public DateTime? DataAtualizacao { get; set; }
        public DateTime? DataPublicacao { get; set; }
        public string Ativo { get; set; } = "S";
        public List<InstitucionalBlocoResponse> Blocos { get; set; } = [];
    }

    [ExcludeFromCodeCoverage]
    public class InstitucionalBlocoResponse
    {
        public long Id { get; set; }
        public int Ordem { get; set; }
        public string? Html { get; set; }
        public string? Titulo { get; set; }
        public string? Subtitulo { get; set; }
        public string Ativo { get; set; } = "S";
        public List<InstitucionalBlocoDescricaoResponse> Descricoes { get; set; } = [];
        public List<InstitucionalBlocoAnexoResponse> Anexos { get; set; } = [];
    }

    [ExcludeFromCodeCoverage]
    public class InstitucionalBlocoAnexoResponse
    {
        public long Id { get; set; }
        public int Ordem { get; set; }
        public string Link { get; set; } = string.Empty;
    }

    [ExcludeFromCodeCoverage]
    public class InstitucionalBlocoDescricaoResponse
    {
        public long Id { get; set; }
        public int Ordem { get; set; }
        public string Texto { get; set; } = string.Empty;
        public List<InstitucionalBlocoSubtextoResponse>? Subtextos { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class InstitucionalBlocoSubtextoResponse
    {
        public long Id { get; set; }
        public int Ordem { get; set; }
        public string Texto { get; set; } = string.Empty;
    }
}
