using Microsoft.EntityFrameworkCore;
using PortalTCMSP.Domain.DTOs.Requests.NoticiaRequest;
using PortalTCMSP.Domain.Helper;
using PortalTCMSP.Domain.Repositories.Noticia;
using PortalTCMSP.Infra.Data.Context;
using PortalTCMSP.Infra.Data.Repositories.Home;
using System;

namespace PortalTCMSP.Infra.Data.Repositories.Noticia
{
    public class NoticiaRepository : BaseRepository<PortalTCMSP.Domain.Entities.NoticiaEntity.Noticia>, INoticiaRepository
    {
        private readonly PortalTCMSPContext _context;
        public NoticiaRepository(PortalTCMSPContext context) : base(context) => _context = context;

        public IEnumerable<PortalTCMSP.Domain.Entities.NoticiaEntity.Noticia> Search(NoticiaListarRequest request)
        {
            var q = _context.Noticia
                .Include(n => n.NoticiaCategorias).ThenInclude(nc => nc.Categoria)
                .Include(n => n.Blocos)
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Categoria))
            {
                var cat = request.Categoria.Trim().ToLower();
                q = q.Where(n => n.NoticiaCategorias.Any(nc =>
                    nc.Categoria.Nome.ToLower() == cat || nc.Categoria.Slug.ToLower() == cat));
            }

            // Opcional: filtro por múltiplos IDs
            // if (request.CategoriaIds?.Any() == true)
            //     q = q.Where(n => n.NoticiaCategorias.Any(nc => request.CategoriaIds.Contains(nc.CategoriaId)));

            if (request.ApenasAtivas == true)
                q = q.Where(n => n.Ativo);

            if (!string.IsNullOrWhiteSpace(request.Query))
            {
                var term = request.Query.Trim().ToLower();
                q = q.Where(n =>
                    n.Titulo.ToLower().Contains(term) ||
                    (n.Resumo != null && n.Resumo.ToLower().Contains(term)) ||
                    (n.Autoria.AutorNome != null && n.Autoria.AutorNome.ToLower().Contains(term)));
            }

            q = q.OrderByDescending(n => n.Destaque)
                 .ThenByDescending(n => n.DataPublicacao);

            return [.. q];
        }

        public async Task<PortalTCMSP.Domain.Entities.NoticiaEntity.Noticia?> ObterPorSlugAsync(string slug)
        {
            var s = SlugHelper.Slugify(slug);
            return await _context.Noticia
                .Include(n => n.NoticiaCategorias).ThenInclude(nc => nc.Categoria)
                .Include(n => n.Blocos)
                .FirstOrDefaultAsync(n => n.Slug == s);
        }

        public async Task<PortalTCMSP.Domain.Entities.NoticiaEntity.Noticia?> ObterPorIdComBlocosAsync(int id)
        {
            return await _context.Noticia
                .Include(n => n.Blocos)
                .FirstOrDefaultAsync(n => n.Id == id);
        }
        public async Task<PortalTCMSP.Domain.Entities.NoticiaEntity.Noticia?> ObterPorIdCompletoAsync(int id)
        {
            return await _context.Noticia
                .Include(n => n.NoticiaCategorias).ThenInclude(nc => nc.Categoria)
                .Include(n => n.Blocos)
                .FirstOrDefaultAsync(n => n.Id == id);
        }

        public async Task<string> GerarSlugUnicoAsync(string slugDesejado)
        {
            var baseSlug = SlugHelper.Slugify(slugDesejado);
            var slug = baseSlug;
            int i = 2;
            while (await _context.Noticia.AnyAsync(n => n.Slug == slug))
            {
                slug = $"{baseSlug}-{i}";
                i++;
            }
            return slug;
        }

        public async Task AdicionarAsync(PortalTCMSP.Domain.Entities.NoticiaEntity.Noticia n)
        {
            _context.Noticia.Add(n);
            await _context.SaveChangesAsync();
        }

        async Task Domain.Repositories.Home.IBaseRepository<Domain.Entities.NoticiaEntity.Noticia>.UpdateAsync(PortalTCMSP.Domain.Entities.NoticiaEntity.Noticia n)
        {
            _context.Noticia.Update(n);
            await _context.SaveChangesAsync();
        }
    }
}
