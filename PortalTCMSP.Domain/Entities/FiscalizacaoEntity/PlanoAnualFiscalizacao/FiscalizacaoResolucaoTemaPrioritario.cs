using PortalTCMSP.Domain.Entities.BaseEntity;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.Entities.FiscalizacaoEntity.PlanoAnualFiscalizacao
{
    [ExcludeFromCodeCoverage]
    public class FiscalizacaoResolucaoTemaPrioritario : Entity
    {
        public long AnexoId { get; set; }
        public int Ordem { get; set; }
        public string Tema { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public FiscalizacaoResolucaoAnexo Anexo { get; set; } = default!;
    }
}
