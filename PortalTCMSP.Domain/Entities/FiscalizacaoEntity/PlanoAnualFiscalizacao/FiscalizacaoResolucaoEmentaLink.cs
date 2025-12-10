using PortalTCMSP.Domain.Entities.BaseEntity;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.Entities.FiscalizacaoEntity.PlanoAnualFiscalizacao
{
    [ExcludeFromCodeCoverage]
    public class FiscalizacaoResolucaoEmentaLink : Entity
    {
        public string Link { get; set; } = string.Empty;
        public long EmentaId { get; set; }
        public FiscalizacaoResolucaoEmenta Ementa { get; set; } = default!;
    }
}
