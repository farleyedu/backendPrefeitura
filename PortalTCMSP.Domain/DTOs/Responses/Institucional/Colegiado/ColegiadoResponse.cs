using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Responses.Institucional.Colegiado
{
    [ExcludeFromCodeCoverage]
    public class ColegiadoResponse
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
        public List<ColegiadoMembroResponse> Blocos { get; set; } = [];
    }
}
