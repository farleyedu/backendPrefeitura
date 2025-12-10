using PortalTCMSP.Domain.DTOs.Requests.Servicos.CartaServicosUsuario.Application.DTO.Servicos.CartaServicosUsuarioDto;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Requests.Servicos.CartaServicosUsuario
{
    [ExcludeFromCodeCoverage]
    public sealed class CartaServicosUsuarioItemDetalheUpdate
    {
        public long Id { get; set; }
        public bool Ativo { get; set; } = true;
        public int Ordem { get; set; }
        public string TituloDetalhe { get; set; } = string.Empty;
        //public List<CartaServicosUsuarioDescritivoItemDetalheRequest> DescritivoItemDetalhe { get; set; } = [];
    }
}
