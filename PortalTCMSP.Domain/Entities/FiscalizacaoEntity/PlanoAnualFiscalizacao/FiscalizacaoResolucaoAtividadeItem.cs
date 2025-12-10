using PortalTCMSP.Domain.Entities.BaseEntity;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.Entities.FiscalizacaoEntity.PlanoAnualFiscalizacao
{
    [ExcludeFromCodeCoverage]
    public class FiscalizacaoResolucaoAtividadeItem : Entity
    {
        public long AtividadeId { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public int Quantidade { get; set; }
        public decimal DUSFs { get; set; }
        public FiscalizacaoResolucaoAtividade Atividade { get; set; } = default!;
    }
}
