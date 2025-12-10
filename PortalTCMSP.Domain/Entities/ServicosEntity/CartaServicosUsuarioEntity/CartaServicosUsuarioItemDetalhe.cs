using PortalTCMSP.Domain.Entities.BaseEntity;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.Entities.ServicosEntity.CartaServicosUsuarioEntity
{
    [ExcludeFromCodeCoverage]
    public class CartaServicosUsuarioItemDetalhe : Entity
    {
        public long IdCartaServicosUsuarioServicoItem { get; set; }
        public bool Ativo { get; set; } = true;
        public int Ordem { get; set; }
        public string TituloDetalhe { get; set; } = string.Empty;

        public CartaServicosUsuarioServicoItem ServicoItem { get; set; } = default!;
        public ICollection<CartaServicosUsuarioDescritivoItemDetalhe> DescritivoItemDetalhe { get; set; } = [];
    }
}
