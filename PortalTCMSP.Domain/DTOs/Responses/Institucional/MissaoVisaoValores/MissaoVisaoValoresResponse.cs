using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Responses.Institucional.MissaoVisaoValores
{
    [ExcludeFromCodeCoverage]
    public class MissaoVisaoValoresResponse
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
        public string Ativo { get; set; } = "S";
        public List<MissaoVisaoValoresBlocoResponse> Blocos { get; set; } = [];
    }

    [ExcludeFromCodeCoverage]
    public class MissaoVisaoValoresBlocoResponse
    {
        public long Id { get; set; }
        public int Ordem { get; set; }
        public string? Titulo { get; set; }
        public List<MissaoVisaoValoresDescricaoResponse> Descricoes { get; set; } = [];
        public string? ImagemUrl { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class MissaoVisaoValoresDescricaoResponse
    {
        public long Id { get; set; }
        public int Ordem { get; set; }
        public string Texto { get; set; } = string.Empty;
    }
}
