using PortalTCMSP.Domain.Entities.BaseEntity;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.Entities.FiscalizacaoEntity.RelatorioFiscalizacao
{
    [ExcludeFromCodeCoverage]
    public class FiscalizacaoRelatorioFiscalizacaoCarrossel : Entity
    {
        public long IdConteudo { get; set; }
        public bool Ativo { get; set; } = true;
        public int Ordem { get; set; } = 0;
        public string? Titulo { get; set; }
        public FiscalizacaoRelatorioFiscalizacao EntityConteudo { get; set; } = default!;
        public ICollection<FiscalizacaoRelatorioFiscalizacaoConteudoCarrocel> ConteudoCarrocel { get; set; } = [];
    }
}
