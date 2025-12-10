using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Requests.BannerRequest
{
    [ExcludeFromCodeCoverage]
    public class BannerCreateRequest
    {
        public string Nome { get; set; } = string.Empty;
        public string Imagem { get; set; } = string.Empty;
        public bool Ativo { get; set; } = false;
    }
}
