using PortalTCMSP.Domain.Entities.BaseEntity;
using PortalTCMSP.Domain.Enum;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.Entities.ServicosEntity.EmissaoCertidoesEntity
{
    [ExcludeFromCodeCoverage]
    public class EmissaoCertidoesAcao : Entity
    {
        public long IdEmissaoCertidoes { get; set; }
        public bool Ativo { get; set; } = true;
        public int Ordem { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public DateTime DataPublicacao { get; set; }
        public TipoAcao TipoAcao { get; set; }
        public string UrlAcao { get; set; } = string.Empty;

        public EmissaoCertidoes EmissaoCertidoes { get; set; } = default!;
    }
}
