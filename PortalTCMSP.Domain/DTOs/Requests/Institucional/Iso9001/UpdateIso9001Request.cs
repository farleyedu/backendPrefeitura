using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Requests.Institucional.Iso9001
{
    [ExcludeFromCodeCoverage]
    public class UpdateIso9001Request
    {
        public string Slug { get; set; } = string.Empty;
        public string Titulo { get; set; } = "ISO 9001";
        public string? Subtitulo { get; set; }
        public string? Descricao { get; set; }
        public string? AutorNome { get; set; }
        public string? Creditos { get; set; }
        public string? SeoTitulo { get; set; }
        public string? SeoDescricao { get; set; }
        public DateTime? DataAtualizacao { get; set; } = DateTime.UtcNow;
        public DateTime? DataPublicacao { get; set; }
        public string Ativo { get; set; } = "S";

        public List<UpdateIso9001BlocoRequest> Blocos { get; set; } = [];
    }

    [ExcludeFromCodeCoverage]
    public class UpdateIso9001BlocoRequest
    {
        public long? Id { get; set; }   
        public int Ordem { get; set; }
        public string? Titulo { get; set; }
        public string? Html { get; set; }
        public List<UpdateIso9001DescricaoRequest> Descricoes { get; set; } = [];
        public List<UpdateIso9001AnexoRequest> Anexos { get; set; } = [];
    }

    [ExcludeFromCodeCoverage]
    public class UpdateIso9001DescricaoRequest
    {
        public long? Id { get; set; }
        public int Ordem { get; set; }
        public string Texto { get; set; } = string.Empty;
    }

    public class UpdateIso9001AnexoRequest
    {
        public long? Id { get; set; }
        public int Ordem { get; set; }
        public string Link { get; set; } = string.Empty;
    }
}
