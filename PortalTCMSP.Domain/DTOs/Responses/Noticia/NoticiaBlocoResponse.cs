using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Responses.Noticia
{
    [ExcludeFromCodeCoverage]
    public class NoticiaBlocoResponse
    {
        public int Order { get; set; }   
        public string Type { get; set; } = string.Empty;
        public object Configuration { get; set; } = default!;
    }
}