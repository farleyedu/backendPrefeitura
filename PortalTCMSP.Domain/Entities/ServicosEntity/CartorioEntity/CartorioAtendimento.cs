using PortalTCMSP.Domain.Entities.BaseEntity;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.Entities.ServicosEntity.CartorioEntity
{
    [ExcludeFromCodeCoverage]
    public class CartorioAtendimento : Entity
    {
        public long IdCartorio { get; set; }
        public int Ordem { get; set; } = 0;
        public string Titulo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;

        public Cartorio Cartorio { get; set; } = default!;
    }
}
