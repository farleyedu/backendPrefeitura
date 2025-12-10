using PortalTCMSP.Domain.Entities.BaseEntity;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.Entities.FiscalizacaoEntity.PlanoAnualFiscalizacao
{
    [ExcludeFromCodeCoverage]
    public class FiscalizacaoResolucaoAtividade : Entity
    {
        public long AnexoId { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public ICollection<FiscalizacaoResolucaoAtividadeItem> AtividadeItem { get; set; } = [];
        public FiscalizacaoResolucaoAnexo Anexo { get; set; } = default!;
    }
}
