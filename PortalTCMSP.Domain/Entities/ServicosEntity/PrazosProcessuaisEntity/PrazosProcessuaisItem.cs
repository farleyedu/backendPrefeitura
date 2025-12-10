using PortalTCMSP.Domain.Entities.BaseEntity;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.Entities.ServicosEntity.PrazosProcessuaisEntity
{
    [ExcludeFromCodeCoverage]
    public class PrazosProcessuaisItem : Entity
    {
        public long IdPrazosProcessuais { get; set; }
        public bool Ativo { get; set; } = true;
        public int Ordem { get; set; }
        public string Nome { get; set; } = string.Empty;
        public DateTime DataPublicacao { get; set; }
        public string TempoDecorrido { get; set; } = string.Empty;

        public PrazosProcessuais PrazosProcessuais { get; set; } = default!;
        public ICollection<PrazosProcessuaisItemAnexo> Anexos { get; set; } = [];
    }
}
