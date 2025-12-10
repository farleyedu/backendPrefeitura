using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Responses.Institucional.Iso9001
{
    [ExcludeFromCodeCoverage]
    public class Iso9001Response
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

        public List<Iso9001BlocoResponse> Blocos { get; set; } = [];
    }

    [ExcludeFromCodeCoverage]
    public class Iso9001BlocoResponse
    {
        public long Id { get; set; }
        public int Ordem { get; set; }
        public string? Titulo { get; set; }
        public string? Html { get; set; }
        public List<Iso9001DescricaoResponse> Descricoes { get; set; } = [];
        public List<Iso9001AnexoResponse> Anexos { get; set; } = []; 
    }

    [ExcludeFromCodeCoverage]
    public class Iso9001DescricaoResponse
    {
        public long Id { get; set; }
        public int Ordem { get; set; }
        public string Texto { get; set; } = string.Empty;
    }

    [ExcludeFromCodeCoverage]
    public class Iso9001AnexoResponse
    {
        public long Id { get; set; }
        public int Ordem { get; set; }
        public string Link { get; set; } = string.Empty;
    }
}
