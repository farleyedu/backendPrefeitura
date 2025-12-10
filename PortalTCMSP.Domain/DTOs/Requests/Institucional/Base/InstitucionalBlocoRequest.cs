using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Requests.Institucional.Base
{
    [ExcludeFromCodeCoverage]
    public class InstitucionalBlocoRequest
    {
        public int Ordem { get; set; }
        public string? Html { get; set; }
        public string? Titulo { get; set; }
        public string? Subtitulo { get; set; }
        public string Ativo { get; set; } = "S";
        public List<InstitucionalBlocoDescricaoRequest> Descricoes { get; set; } = [];
        public List<InstitucionalBlocoAnexoRequest> Anexos { get; set; } = [];
    }
}
