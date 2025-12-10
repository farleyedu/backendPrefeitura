using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Requests.Base
{
    [ExcludeFromCodeCoverage]
    public class BaseConsultaPaginada
    {
        public int Page { get; set; } = 1;
        public int Count { get; set; } = 10;
    }
}
