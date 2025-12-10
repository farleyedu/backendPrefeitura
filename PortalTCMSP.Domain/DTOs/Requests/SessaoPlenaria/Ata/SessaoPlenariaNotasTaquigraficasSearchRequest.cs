using PortalTCMSP.Domain.DTOs.Requests.Base;
using PortalTCMSP.Domain.Enum;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Requests.SessaoPlenaria.Ata
{
    [ExcludeFromCodeCoverage]
    public sealed class SessaoPlenariaNotasTaquigraficasSearchRequest : BaseConsultaPaginada
    {
        public string? Query { get; set; }             
        public long? IdSessaoPlenaria { get; set; }
        public NotasTipo? Tipo { get; set; }          
        public DateTime? PublicadaDe { get; set; }
        public DateTime? PublicadaAte { get; set; }
    }
}
