using PortalTCMSP.Domain.Entities.NoticiaEntity;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Unit.Tests.Repository.Noticia.FixFeature
{
    [ExcludeFromCodeCoverage]
    public class CategoriaRepositoryFixture
    {
        public Categoria GetCategoria(long id = 1, string nome = "Tecnologia", string slug = "tecnologia")
            => new Categoria
            {
                Id = id,
                Nome = nome,
                Slug = slug,
                Noticias = new List<PortalTCMSP.Domain.Entities.NoticiaEntity.Noticia>()
            };

        public List<Categoria> GetCategorias()
            => new List<Categoria>
            {
                GetCategoria(1, "Tecnologia", "tecnologia"),
                GetCategoria(2, "Esportes", "esportes"),
                GetCategoria(3, "Artes", "artes")
            };
    }
}
