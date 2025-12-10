using PortalTCMSP.Domain.Entities.BaseEntity;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.Entities.FiscalizacaoEntity.RelatorioFiscalizacao
{
    [ExcludeFromCodeCoverage]
    public class FiscalizacaoRelatorioFiscalizacaoTcRelacionado : Entity 
    { 
        public long IdConteudoLink { get; set; } 
        public string? Link { get; set; } 
        public string? Descritivo { get; set; } 
        public int Ordem { get; set; } 
    }
}
