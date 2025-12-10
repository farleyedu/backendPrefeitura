using PortalTCMSP.Domain.DTOs.Requests.Base;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Requests.Consulta
{
    [ExcludeFromCodeCoverage]
    public class ConsultaNoticiasRequest : BaseConsultaPaginada
    {
        public string? Query { get; set; }
        public string? Categoria { get; set; }
        public bool? ApenasAtivas { get; set; }
    }
}
