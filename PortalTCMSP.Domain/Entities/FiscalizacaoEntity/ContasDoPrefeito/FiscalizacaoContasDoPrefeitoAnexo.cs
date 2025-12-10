using PortalTCMSP.Domain.Entities.BaseEntity;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.Entities.FiscalizacaoEntity.ContasDoPrefeito
{
    [ExcludeFromCodeCoverage]
    public class FiscalizacaoContasDoPrefeitoAnexo : Entity
    {
        public long IdFiscalizacaoContasDoPrefeito { get; set; }
        public string Link { get; set; } = string.Empty;
        public string? TipoArquivo { get; set; }
        public string? NomeExibicao { get; set; }
        public int Ordem { get; set; } = 0;
        public FiscalizacaoContasDoPrefeito? ContasDoPrefeito { get; set; }
    }
}
