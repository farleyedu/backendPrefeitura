using PortalTCMSP.Domain.Entities.BaseEntity;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.Entities.FiscalizacaoEntity.PlanoAnualFiscalizacao
{
    [ExcludeFromCodeCoverage]
    public class FiscalizacaoResolucaoEmenta : Entity
    {
        public string Descritivo { get; set; } = string.Empty;
        public ICollection<FiscalizacaoResolucaoEmentaLink> LinksArtigos { get; set; } = [];
        public long ResolucaoId { get; set; }
        public FiscalizacaoPlanoAnualFiscalizacaoResolucao Resolucao { get; set; } = default!;
    }
}
