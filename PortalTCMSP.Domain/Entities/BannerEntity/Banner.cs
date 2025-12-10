using PortalTCMSP.Domain.Entities.BaseEntity;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.Entities.BannerEntity
{
    [ExcludeFromCodeCoverage]
    public class Banner : Entity
    {
        public string Nome { get; set; } = string.Empty;
        public string Imagem { get; set; } = string.Empty;
        public bool Ativo { get; set; } = false;  
        public DateTime DataCriacao { get; set; }    
        public DateTime? DataAtualizacao { get; set; }
    }
}
