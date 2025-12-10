using PortalTCMSP.Domain.Entities.BaseEntity;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.Entities.FiscalizacaoEntity.SecretariaControleExterno
{
    [ExcludeFromCodeCoverage]
    public class FiscalizacaoSecretariaSecaoConteudoTitulo : Entity
    {
        public long IdSecaoConteudo { get; set; }
        public int Ordem { get; set; } = 0;
        public string Titulo { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public string? ImagemUrl { get; set; } = string.Empty;
        public FiscalizacaoSecretariaControleExterno Conteudo { get; set; } = default!;
    }
}
