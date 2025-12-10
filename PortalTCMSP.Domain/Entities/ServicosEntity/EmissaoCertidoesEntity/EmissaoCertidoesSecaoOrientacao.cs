using PortalTCMSP.Domain.Entities.BaseEntity;
using PortalTCMSP.Domain.Enum;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.Entities.ServicosEntity.EmissaoCertidoesEntity
{
    [ExcludeFromCodeCoverage]
    public class EmissaoCertidoesSecaoOrientacao : Entity
    {
        public long IdEmissaoCertidoes { get; set; }
        public TipoSecao TipoSecao { get; set; }
        public string TituloPagina { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;

        public ICollection<EmissaoCertidoesOrientacao>  Orientacoes { get; set; } = [];
        public EmissaoCertidoes EmissaoCertidoes { get; set; } = default!;
    }
}
