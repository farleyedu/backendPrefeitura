using PortalTCMSP.Domain.Entities.BaseEntity;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.Entities.NoticiaEntity
{
    [ExcludeFromCodeCoverage]
    public class Categoria : Entity
    {
        public string Nome { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public ICollection<Noticia> Noticias { get; set; } = new List<Noticia>();
    }
}
