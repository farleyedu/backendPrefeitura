using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Responses.Institucional.ProtecaoDados
{
    [ExcludeFromCodeCoverage]
    public class ProtecaoDadosResponse
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
        public DateTime? DataPublicacao { get; set; }
        public string Ativo { get; set; } = "S";

        public List<ProtecaoDadosBlocoResponse> Blocos { get; set; } = [];
    }

    [ExcludeFromCodeCoverage]
    public class ProtecaoDadosBlocoResponse
    {
        public long Id { get; set; }
        public int Ordem { get; set; }
        public string? Titulo { get; set; }
        public string? Html { get; set; }
        public List<ProtecaoDadosDescricaoResponse> Descricoes { get; set; } = [];
        public List<ProtecaoDadosAnexoResponse> Anexos { get; set; } = [];
    }

    [ExcludeFromCodeCoverage]
    public class ProtecaoDadosDescricaoResponse
    {
        public long Id { get; set; }
        public int Ordem { get; set; }
        public string Texto { get; set; } = string.Empty;
    }

    [ExcludeFromCodeCoverage]
    public class ProtecaoDadosAnexoResponse
    {
        public long Id { get; set; }
        public int Ordem { get; set; }
        public string Link { get; set; } = string.Empty;
    }
}
