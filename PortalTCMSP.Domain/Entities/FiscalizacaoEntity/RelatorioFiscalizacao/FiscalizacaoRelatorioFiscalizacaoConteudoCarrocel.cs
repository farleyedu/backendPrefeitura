using PortalTCMSP.Domain.Entities.BaseEntity;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.Entities.FiscalizacaoEntity.RelatorioFiscalizacao
{
    [ExcludeFromCodeCoverage]
    public class FiscalizacaoRelatorioFiscalizacaoConteudoCarrocel : Entity
    {
        public long IdCarrosselItem { get; set; }
        public int Ordem { get; set; } = 0;
        public bool Ativo { get; set; } = true;
        public string? Descricao { get; set; }
        public string? Link { get; set; }
        public FiscalizacaoRelatorioFiscalizacaoConteudoLink? ConteudoLink { get; set; }
        public FiscalizacaoRelatorioFiscalizacaoCarrossel CarrosselItem { get; set; } = default!;
    }
}
