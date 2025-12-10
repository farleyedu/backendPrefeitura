using PortalTCMSP.Domain.Entities.BaseEntity;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.Entities.ServicosEntity.CartaServicosUsuarioEntity
{
    [ExcludeFromCodeCoverage]
    public class CartaServicosUsuario : Entity
    {
        public string TituloPagina { get; set; } = string.Empty;
        public string TituloPesquisa { get; set; } = string.Empty;
        public bool Ativo { get; set; } = true;
        public string Slug { get; set; } = string.Empty;
        public DateTime DataCriacao { get; set; }
        public DateTime? DataAtualizacao { get; set; }

        public ICollection<CartaServicosUsuarioServico> Servicos { get; set; } = [];
    }
}
