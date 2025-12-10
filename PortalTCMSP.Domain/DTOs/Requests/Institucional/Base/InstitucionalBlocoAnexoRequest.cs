using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Requests.Institucional.Base
{
    [ExcludeFromCodeCoverage]
    public class InstitucionalBlocoAnexoRequest
    {
        public int Ordem { get; set; }
        public string Link { get; set; } = string.Empty;
    }
}
