using PortalTCMSP.Domain.Entities.BaseEntity;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.Entities.FiscalizacaoEntity.PlanoAnualFiscalizacao
{
    [ExcludeFromCodeCoverage]
    public class FiscalizacaoResolucaoInciso : Entity
    {
        public long DispositivoId { get; set; }
        public int Ordem { get; set; }
        public string Texto { get; set; } = string.Empty;
        public FiscalizacaoResolucaoDispositivo Dispositivo { get; set; } = default!;
    }
}
