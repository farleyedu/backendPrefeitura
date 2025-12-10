using PortalTCMSP.Domain.Entities.BaseEntity;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.Entities.ServicosEntity.CartaServicosUsuarioEntity
{
    [ExcludeFromCodeCoverage]
    public class CartaServicosUsuarioDescritivoItemDetalhe : Entity
    {
        public long IdCartaServicosUsuarioItemDetalhe { get; set; }
        public int Ordem { get; set; }
        public string Descritivo { get; set; } = string.Empty;

        public CartaServicosUsuarioItemDetalhe ItemDetalhe { get; set; } = default!;
    }
}
