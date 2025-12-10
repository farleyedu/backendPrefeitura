using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Requests.Institucional.Colegiado
{
    [ExcludeFromCodeCoverage]
    public class CreateColegiadoRequest
    {
        public string Slug { get; set; } = string.Empty;
        public string Titulo { get; set; } = "Colegiado";
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
        public List<CreateColegiadoMembroRequest> Blocos { get; set; } = [];
    }

    [ExcludeFromCodeCoverage]
    public class CreateColegiadoMembroRequest
    {
        public int Ordem { get; set; }
        public string Cargo { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public string? ImagemUrl { get; set; }
    }
}
