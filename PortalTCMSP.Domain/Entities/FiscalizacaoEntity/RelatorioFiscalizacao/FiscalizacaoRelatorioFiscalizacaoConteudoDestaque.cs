using PortalTCMSP.Domain.Entities.BaseEntity;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.Entities.FiscalizacaoEntity.RelatorioFiscalizacao
{
    [ExcludeFromCodeCoverage]
    public class FiscalizacaoRelatorioFiscalizacaoConteudoDestaque : Entity
    {
        public long IdConteudoLink { get; set; }
        public string? Titulo { get; set; }
        public ICollection<FiscalizacaoRelatorioFiscalizacaoDescricao> Descricoes { get; set; } = [];
        public string? ImagemUrl { get; set; }
    }
}
