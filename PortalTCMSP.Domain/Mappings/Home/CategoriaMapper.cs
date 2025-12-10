using PortalTCMSP.Domain.DTOs.Requests.Categoria;
using PortalTCMSP.Domain.DTOs.Responses.CategoriaResponse;
using PortalTCMSP.Domain.Entities.NoticiaEntity;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.Mappings.Home
{
    [ExcludeFromCodeCoverage]
    public static class CategoriaMapper
    {
        public static Categoria ToEntity(CategoriaCreateRequest request)
        {
            return new Categoria
            {
                Nome = request.Nome,
                Slug = request.Slug
            };
        }

        public static void ApplyUpdate(Categoria categoria, CategoriaUpdateRequest request)
        {
            categoria.Nome = request.Nome;
            categoria.Slug = request.Slug;
        }

        public static CategoriaResponse ToResponse(Categoria categoria)
        {
            return new CategoriaResponse
            {
                Id = (int)categoria.Id,
                Nome = categoria.Nome,
                Slug = categoria.Slug
            };
        }
    }
}
