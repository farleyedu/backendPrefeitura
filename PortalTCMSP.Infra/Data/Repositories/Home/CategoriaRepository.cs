using Microsoft.EntityFrameworkCore;
using PortalTCMSP.Domain.Entities.NoticiaEntity;
using PortalTCMSP.Domain.Repositories.Home;
using PortalTCMSP.Infra.Data.Context;

namespace PortalTCMSP.Infra.Data.Repositories.Home
{
    public class CategoriaRepository : BaseRepository<Categoria>, ICategoriaRepository
    {
        private readonly PortalTCMSPContext _context;

        public CategoriaRepository(PortalTCMSPContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Categoria>> ListarTodasAsync()
        {
            return await _context.Categoria.OrderBy(c => c.Nome).ToListAsync();
        }

        public async Task<List<Categoria>> FindByIdsAsync(IEnumerable<int> ids)
        {
            var set = ids.Distinct().ToList();
            return await _context.Categoria.Where(c => set.Contains((int)c.Id)).ToListAsync();
        }

        public async Task<List<Categoria>> FindBySlugsAsync(IEnumerable<string> slugs)
        {
            var set = slugs.Where(s => !string.IsNullOrWhiteSpace(s))
                           .Select(s => s.Trim().ToLower())
                           .Distinct().ToList();

            return await _context.Categoria
                .Where(c => set.Contains(c.Slug.ToLower()))
                .ToListAsync();
        }
    }
}
