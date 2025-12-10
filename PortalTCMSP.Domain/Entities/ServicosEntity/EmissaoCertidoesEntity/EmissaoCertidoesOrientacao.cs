using PortalTCMSP.Domain.Entities.BaseEntity;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.Entities.ServicosEntity.EmissaoCertidoesEntity
{
    [ExcludeFromCodeCoverage]
    public class EmissaoCertidoesOrientacao : Entity
    {
        public long IdEmissaoCertidoesSecaoOrientacao { get; set; }
        public int Ordem { get; set; }
        public ICollection<EmissaoCertidoesDescritivo> Descritivos { get; set; } = [];
        public EmissaoCertidoesSecaoOrientacao SecaoOrientacao { get; set; } = default!;
    }
}
