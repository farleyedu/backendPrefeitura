using PortalTCMSP.Domain.Entities.BaseEntity;
using PortalTCMSP.Domain.Entities.InstitucionalBlocoEntity;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.Entities.BlocoEntity
{
    [ExcludeFromCodeCoverage]
    public class InstitucionalBlocoDescricao : Entity
    {
        public int Ordem { get; set; }
        public string Texto { get; set; } = string.Empty;
        public long IdBloco { get; set; }
        public InstitucionalBloco Bloco { get; set; } = default!;
        public ICollection<InstitucionalBlocoSubtexto> Subtextos { get; set; } = [];
    }
}
