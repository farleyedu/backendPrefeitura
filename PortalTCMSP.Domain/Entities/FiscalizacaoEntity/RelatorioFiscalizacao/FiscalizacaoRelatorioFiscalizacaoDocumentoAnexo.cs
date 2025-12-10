using PortalTCMSP.Domain.Entities.BaseEntity;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.Entities.FiscalizacaoEntity.RelatorioFiscalizacao
{
    [ExcludeFromCodeCoverage]
    public class FiscalizacaoRelatorioFiscalizacaoDocumentoAnexo : Entity 
    { 
        public long IdConteudoLink { get; set; } 
        public string? Link { get; set; } 
        public string? TipoArquivo { get; set; } 
        public string? NomeExibicao { get; set; } 
        public int Ordem { get; set; } 
    }
}
