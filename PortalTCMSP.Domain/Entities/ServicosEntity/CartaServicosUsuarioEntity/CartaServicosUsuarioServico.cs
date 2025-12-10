using PortalTCMSP.Domain.Entities.BaseEntity;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.Entities.ServicosEntity.CartaServicosUsuarioEntity
{
    [ExcludeFromCodeCoverage]
    public class CartaServicosUsuarioServico : Entity
    {
        public long IdCartaServicosUsuario { get; set; }
        public bool Ativo { get; set; } = true;
        public int Ordem { get; set; }
        public string Titulo { get; set; } = string.Empty;

        public CartaServicosUsuario CartaServicosUsuario { get; set; } = default!;
        public ICollection<CartaServicosUsuarioServicoItem> ServicosItens { get; set; } = [];
    }
}
