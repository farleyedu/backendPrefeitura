using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Requests.Institucional.ProtecaoDados
{
    [ExcludeFromCodeCoverage]
    public class CreateProtecaoDadosRequest
    {
        public string Slug { get; set; } = string.Empty;
        public string Titulo { get; set; } = "Proteção de Dados (LGPD)";
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

        public List<CreateProtecaoDadosBlocoRequest> Blocos { get; set; } = [];
    }

    [ExcludeFromCodeCoverage]
    public class CreateProtecaoDadosBlocoRequest
    {
        public int Ordem { get; set; }                
        public string? Titulo { get; set; }         
        public string? Html { get; set; }             
        public List<CreateProtecaoDadosDescricaoRequest> Descricoes { get; set; } = [];
        public List<CreateProtecaoDadosAnexoRequest> Anexos { get; set; } = [];
    }

    [ExcludeFromCodeCoverage]
    public class CreateProtecaoDadosDescricaoRequest
    {
        public int Ordem { get; set; }
        public string Texto { get; set; } = string.Empty;
    }

    [ExcludeFromCodeCoverage]
    public class CreateProtecaoDadosAnexoRequest
    {
        public int Ordem { get; set; }
        public string Link { get; set; } = string.Empty;
    }
}
