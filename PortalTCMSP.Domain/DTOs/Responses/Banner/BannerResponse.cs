using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Responses.Banner
{
    [ExcludeFromCodeCoverage]
    public class BannerResponse
    {
        public long Id { get; set; }        
        public string Nome { get; set; } = string.Empty;
        public string ImagemUrl { get; set; } = string.Empty;
        public bool Ativo { get; set; }  
        public DateTime DataCriacao { get; set; }
        public DateTime? DataAtualizacao { get; set; }
    }
}
