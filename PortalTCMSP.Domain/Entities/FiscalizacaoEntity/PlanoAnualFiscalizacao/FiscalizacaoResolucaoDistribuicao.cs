using PortalTCMSP.Domain.Entities.BaseEntity;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.Entities.FiscalizacaoEntity.PlanoAnualFiscalizacao
{

    [ExcludeFromCodeCoverage]
    public class FiscalizacaoResolucaoDistribuicao : Entity
    {
        public long AnexoId { get; set; }
        public string TipoFiscalizacao { get; set; } = string.Empty;
        public int TotalPAF { get; set; }
        public decimal DezPorCentoTotalPAF { get; set; }
        public int LimitePorConselheiro { get; set; }
        public int LimiteConselheiros { get; set; }
        public int LimitePlenoeCameras { get; set; }
        public int LimitePresidente { get; set; }
        public int ListaDePrioridades { get; set; }
        public FiscalizacaoResolucaoAnexo Anexo { get; set; } = default!;
    }
}
