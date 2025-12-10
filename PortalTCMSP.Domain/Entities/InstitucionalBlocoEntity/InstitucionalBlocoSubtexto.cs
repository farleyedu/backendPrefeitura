using PortalTCMSP.Domain.Entities.BaseEntity;
using PortalTCMSP.Domain.Entities.BlocoEntity;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.Entities.InstitucionalBlocoEntity
{
    [ExcludeFromCodeCoverage]
    public class InstitucionalBlocoSubtexto : Entity
    {
        public int Ordem { get; set; }
        public string Texto { get; set; } = string.Empty;
        public long IdDescricao { get; set; }
        public InstitucionalBlocoDescricao Descricao { get; set; } = default!;
    }
}
