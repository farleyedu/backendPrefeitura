using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Requests.Institucional.Iso9001
{
    [ExcludeFromCodeCoverage]
    public class CreateIso9001Request
    {
        public string Slug { get; set; } = string.Empty;
        public string Titulo { get; set; } = "ISO 9001";
        public string? Subtitulo { get; set; }
        public string? Descricao { get; set; }
        public string? AutorNome { get; set; }
        public string? Creditos { get; set; }
        public string? SeoTitulo { get; set; }
        public string? SeoDescricao { get; set; }
        public DateTime? DataCriacao { get; set; }
        public DateTime? DataAtualizacao { get; set; }
        public DateTime? DataPublicacao { get; set; }
        public string Ativo { get; set; } = "S";

        public List<CreateIso9001BlocoRequest> Blocos { get; set; } = [];
    }

    [ExcludeFromCodeCoverage]
    public class CreateIso9001BlocoRequest
    {
        public int Ordem { get; set; }
        public string? Titulo { get; set; }
        public string? Html { get; set; } 
        public List<CreateIso9001DescricaoRequest> Descricoes { get; set; } = []; 
        public List<CreateIso9001AnexoRequest> Anexos { get; set; } = [];  
    }

    [ExcludeFromCodeCoverage]
    public class CreateIso9001DescricaoRequest
    {
        public int Ordem { get; set; }
        public string Texto { get; set; } = string.Empty;
    }

    [ExcludeFromCodeCoverage]
    public class CreateIso9001AnexoRequest
    {
        public int Ordem { get; set; }
        public string Link { get; set; } = string.Empty;
    }
}
