using PortalTCMSP.Domain.Entities.BaseEntity;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.Entities.ServicosEntity.EmissaoCertidoesEntity
{
    [ExcludeFromCodeCoverage]
    public class EmissaoCertidoesDescritivo : Entity
    {
        public long IdEmissaoCertidoesOrientacao { get; set; }
        public string Texto { get; set; } = string.Empty;
        public EmissaoCertidoesOrientacao Orientacao { get; set; } = default!;
    }
}
