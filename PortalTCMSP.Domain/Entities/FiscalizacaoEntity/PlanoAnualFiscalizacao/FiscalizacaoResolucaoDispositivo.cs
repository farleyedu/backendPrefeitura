using PortalTCMSP.Domain.Entities.BaseEntity;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.Entities.FiscalizacaoEntity.PlanoAnualFiscalizacao
{
    [ExcludeFromCodeCoverage]
    public class FiscalizacaoResolucaoDispositivo : Entity
    {
        public long ResolucaoId { get; set; }
        public int Ordem { get; set; }
        public string Artigo { get; set; } = string.Empty;
        public ICollection<FiscalizacaoResolucaoParagrafo> Paragrafo { get; set; } = [];
        public ICollection<FiscalizacaoResolucaoInciso> Incisos { get; set; } = [];
        public FiscalizacaoPlanoAnualFiscalizacaoResolucao Resolucao { get; set; } = default!;
    }
}
