using PortalTCMSP.Domain.Entities.BaseEntity;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.Entities.ServicosEntity.CartaServicosUsuarioEntity
{
    [ExcludeFromCodeCoverage]
    public class CartaServicosUsuarioServicoItem : Entity
    {
        public long IdCartaServicosUsuarioServico { get; set; }
        public bool Ativo { get; set; } = true;
        public int Ordem { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Acao { get; set; } = string.Empty;
        public string LinkItem { get; set; } = string.Empty;

        public CartaServicosUsuarioServico Servico { get; set; } = default!;
        public ICollection<CartaServicosUsuarioItemDetalhe> ItemDetalhe { get; set; } = [];
    }
}
