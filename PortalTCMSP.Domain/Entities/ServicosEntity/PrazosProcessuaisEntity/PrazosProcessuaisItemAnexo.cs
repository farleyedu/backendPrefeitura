using PortalTCMSP.Domain.Entities.BaseEntity;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.Entities.ServicosEntity.PrazosProcessuaisEntity
{
    [ExcludeFromCodeCoverage]
    public class PrazosProcessuaisItemAnexo : Entity
    {
        public long IdPrazosProcessuaisItem { get; set; }
        public bool Ativo { get; set; } = true;
        public int Ordem { get; set; }
        public string NomeArquivo { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;

        public PrazosProcessuaisItem PrazosProcessuaisItem { get; set; } = default!;
    }
}
