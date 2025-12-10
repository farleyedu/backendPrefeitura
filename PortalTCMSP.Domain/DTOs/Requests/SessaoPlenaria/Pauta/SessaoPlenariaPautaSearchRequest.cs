using PortalTCMSP.Domain.DTOs.Requests.Base;
using PortalTCMSP.Domain.Enum;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Requests.SessaoPlenaria.Pauta
{
    [ExcludeFromCodeCoverage]
    public sealed class SessaoPlenariaPautaSearchRequest : BaseConsultaPaginada
    {
        public string? Query { get; set; }
        public long? IdSessaoPlenaria { get; set; }
        public PautaTipo? Tipo { get; set; }
        public DateTime? PublicadaDe { get; set; }
        public DateTime? PublicadaAte { get; set; }
    }
}
