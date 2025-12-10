using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Requests.Institucional.Base
{
    [ExcludeFromCodeCoverage]
    public class InstitucionalBlocoDescricaoRequest
    {
        public int Ordem { get; set; }
        public string Texto { get; set; } = string.Empty;
        public List<CreateInstitucionalBlocoSubtextoRequest> Subtextos { get; set; } = [];
    }
    public class CreateInstitucionalBlocoSubtextoRequest
    {
        public int Ordem { get; set; }
        public string Texto { get; set; } = string.Empty;
    }
}
