using PortalTCMSP.Domain.DTOs.Requests.Base;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Requests.NoticiaRequest
{
    [ExcludeFromCodeCoverage]
    public class NoticiaListarRequest : BaseConsultaPaginada
    {
        public string? Categoria { get; set; }
        public string? Query { get; set; } 
        public bool? ApenasAtivas { get; set; }
    }
}
