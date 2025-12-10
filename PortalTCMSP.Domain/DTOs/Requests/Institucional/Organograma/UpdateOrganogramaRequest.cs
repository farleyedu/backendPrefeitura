using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Requests.Institucional.Organograma
{
    [ExcludeFromCodeCoverage]
    public class UpdateOrganogramaRequest
    {
        public string Slug { get; set; } = string.Empty;
        public string Titulo { get; set; } = "Organograma";
        public string? Subtitulo { get; set; }
        public string? Descricao { get; set; }
        public string? AutorNome { get; set; }
        public string? Creditos { get; set; }
        public string? SeoTitulo { get; set; }
        public string? SeoDescricao { get; set; }
        public DateTime? DataAtualizacao { get; set; } = DateTime.UtcNow;
        public string Ativo { get; set; } = "S";

        public List<UpdateOrganogramaBlocoRequest> Blocos { get; set; } = [];
    }

    [ExcludeFromCodeCoverage]
    public class UpdateOrganogramaBlocoRequest
    {
        public long? Id { get; set; }
        public int Ordem { get; set; }
        public string? Titulo { get; set; }
        public string? ImagemUrl { get; set; }
    }
}
