using PortalTCMSP.Domain.Entities.NoticiaEntity;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Unit.Tests.Repository.Home.FixFeature
{
    [ExcludeFromCodeCoverage]
    public class BaseRepositoryFixture
    {
        public Categoria GetCategoria(long id = 1, string nome = "Teste", string slug = "teste")
            => new Categoria
            {
                Id = id,
                Nome = nome,
                Slug = slug,
                Noticias = new List<PortalTCMSP.Domain.Entities.NoticiaEntity.Noticia>()
            };

        public Categoria[] GetCategorias()
            => new[]
            {
                GetCategoria(1, "Tecnologia", "tecnologia"),
                GetCategoria(2, "Esportes", "esportes"),
                GetCategoria(3, "Cultura", "cultura")
            };

    }
}
