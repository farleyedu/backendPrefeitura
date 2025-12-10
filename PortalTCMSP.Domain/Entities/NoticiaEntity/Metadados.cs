using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.Entities.NoticiaEntity
{
    [ExcludeFromCodeCoverage]
    public class Metadados
    {
        public string? SeoTitle { get; set; }
        public string? SeoDescription { get; set; }
        public string? OgImageUrl { get; set; }
        public string? Canonical { get; set; }
    }
}
