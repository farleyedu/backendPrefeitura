using PortalTCMSP.Domain.Entities.BaseEntity;
using PortalTCMSP.Domain.Enum;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.Entities.ServicosEntity.ProtocoloEletronicoEntity
{
    [ExcludeFromCodeCoverage]
    public class ProtocoloEletronicoAcao : Entity
    {
        public long IdProtocoloEletronico { get; set; }
        public bool Ativo { get; set; } = true;
        public int Ordem { get; set; } = 0;
        public string Titulo { get; set; } = string.Empty;
        public DateTime DataPublicacao { get; set; }
        public TipoAcao TipoAcao { get; set; }
        public string UrlAcao { get; set; } = string.Empty;

        public ProtocoloEletronico ProtocoloEletronico { get; set; } = default!;
    }
}
