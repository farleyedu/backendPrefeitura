using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Requests.Institucional.MissaoVisaoValores
{
    [ExcludeFromCodeCoverage]
    public class CreateMissaoVisaoValoresRequest
    {
        public string Slug { get; set; } = string.Empty;
        public string Titulo { get; set; } = "Missão, Visão e Valores";
        public string? Subtitulo { get; set; }
        public string? Descricao { get; set; }
        public string? AutorNome { get; set; }
        public string? Creditos { get; set; }
        public string? SeoTitulo { get; set; }
        public string? SeoDescricao { get; set; }
        public DateTime? DataCriacao { get; set; }
        public DateTime? DataAtualizacao { get; set; }
        public string Ativo { get; set; } = "S";
        public List<CreateMissaoVisaoValoresBlocoRequest> Blocos { get; set; } = [];
    }

    [ExcludeFromCodeCoverage]
    public class CreateMissaoVisaoValoresBlocoRequest
    {
        public int Ordem { get; set; }
        public string? Titulo { get; set; }
        public List<CreateMissaoVisaoValoresDescricaoRequest> Descricoes { get; set; } = [];
        public string? ImagemUrl { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class CreateMissaoVisaoValoresDescricaoRequest
    {
        public int Ordem { get; set; }
        public string Texto { get; set; } = string.Empty;
    }
}
