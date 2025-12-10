using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Requests.Categoria
{
    [ExcludeFromCodeCoverage]
    public class CategoriaCreateRequest
    {
        public string Nome { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
    }
}