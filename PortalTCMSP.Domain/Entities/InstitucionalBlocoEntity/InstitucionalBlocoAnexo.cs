using PortalTCMSP.Domain.Entities.BaseEntity;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.Entities.BlocoEntity
{
    [ExcludeFromCodeCoverage]
    public class InstitucionalBlocoAnexo : Entity
    {
        public int Ordem { get; set; }
        public string Link { get; set; } = string.Empty;
        public long IdBloco { get; set; }
        public InstitucionalBloco Bloco { get; set; } = default!;
    }
}
