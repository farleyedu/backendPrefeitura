using PortalTCMSP.Domain.Entities.BaseEntity;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.Entities.FiscalizacaoEntity.RelatorioFiscalizacao
{
    [ExcludeFromCodeCoverage]
    public class FiscalizacaoRelatorioFiscalizacaoImagemAnexa : Entity 
    { 
        public long IdConteudoLink { get; set; } 
        public string? ImagemUrl { get; set; } 
        public string? NomeExibicao { get; set; } 
        public int Ordem { get; set; } 
    }
}
