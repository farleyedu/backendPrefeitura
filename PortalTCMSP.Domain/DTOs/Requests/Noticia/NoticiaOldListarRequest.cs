using PortalTCMSP.Domain.DTOs.Requests.Base;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Requests.Noticia
{
    [ExcludeFromCodeCoverage]
    public class NoticiaOldListarRequest : BaseConsultaPaginada
    {
        public string? Categoria { get; set; }  
        public string? Query { get; set; }
        public bool? ApenasAtivas { get; set; }
    }
}
