using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Responses.CategoriaResponse
{
    [ExcludeFromCodeCoverage]
    public class CategoriaResponse
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
    }
}
