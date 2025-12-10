using PortalTCMSP.Domain.DTOs.Requests.Categoria;
using PortalTCMSP.Domain.DTOs.Responses.CategoriaResponse;
using PortalTCMSP.Domain.Entities.NoticiaEntity;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Unit.Tests.Services.Noticia.FixFeature
{
    [ExcludeFromCodeCoverage]
    public class CategoriaServiceFixture
    {
        public CategoriaCreateRequest GetCreateRequest() => new()
        {
            Nome = "Tecnologia",
            Slug = "tecnologia"
        };

        public CategoriaUpdateRequest GetUpdateRequest(int id = 1) => new()
        {
            Id = id,
            Nome = "Atualizada",
            Slug = "atualizada"
        };

        public Categoria GetCategoriaEntity(long id = 1) => new()
        {
            Id = id,
            Nome = "Tecnologia",
            Slug = "tecnologia",
            Noticias = new List<PortalTCMSP.Domain.Entities.NoticiaEntity.Noticia>()
        };

        public CategoriaResponse GetCategoriaResponse(int id = 1) => new()
        {
            Id = id,
            Nome = "Tecnologia",
            Slug = "tecnologia"
        };

        public List<Categoria> GetCategoriaList()
        {
            return new List<Categoria>
            {
                GetCategoriaEntity(1),
                new Categoria { Id = 2, Nome = "Esportes", Slug = "esportes", Noticias = new List<PortalTCMSP.Domain.Entities.NoticiaEntity.Noticia>() }
            };
        }
    }
}
