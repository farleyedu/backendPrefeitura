using PortalTCMSP.Domain.Entities.BaseEntity;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.Entities.FiscalizacaoEntity.PlanoAnualFiscalizacao
{
    [ExcludeFromCodeCoverage]
    public class FiscalizacaoResolucaoAnexo : Entity
    {
        public long ResolucaoId { get; set; }
        public int Numero { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string? Descritivo { get; set; }
        public ICollection<FiscalizacaoResolucaoTemaPrioritario> TemasPrioritarios { get; set; } = [];
        public ICollection<FiscalizacaoResolucaoAtividade> Atividades { get; set; } = [];
        public ICollection<FiscalizacaoResolucaoDistribuicao> Distribuicao { get; set; } = [];
        public FiscalizacaoPlanoAnualFiscalizacaoResolucao Resolucao { get; set; } = default!;
    }
}
