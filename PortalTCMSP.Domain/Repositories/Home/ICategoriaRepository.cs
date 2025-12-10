using PortalTCMSP.Domain.Entities.NoticiaEntity;

namespace PortalTCMSP.Domain.Repositories.Home
{
    public interface ICategoriaRepository : IBaseRepository<Categoria>
    {
        Task<IEnumerable<Categoria>> ListarTodasAsync();
        Task<List<Categoria>> FindByIdsAsync(IEnumerable<int> ids);
        Task<List<Categoria>> FindBySlugsAsync(IEnumerable<string> slugs);
    }
}
