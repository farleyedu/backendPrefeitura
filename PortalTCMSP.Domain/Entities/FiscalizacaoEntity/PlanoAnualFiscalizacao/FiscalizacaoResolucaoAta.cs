using PortalTCMSP.Domain.Entities.BaseEntity;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.Entities.FiscalizacaoEntity.PlanoAnualFiscalizacao
{
    [ExcludeFromCodeCoverage]
    public class FiscalizacaoResolucaoAta : Entity
    {
        public long ResolucaoId { get; set; }
        public int Ordem { get; set; }
        public string TituloAta { get; set; } = string.Empty;
        public bool TituloAtaAEsquerda { get; set; }
        public string ConteudoAta { get; set; } = string.Empty;
        public FiscalizacaoPlanoAnualFiscalizacaoResolucao Resolucao { get; set; } = default!;
    }
}
