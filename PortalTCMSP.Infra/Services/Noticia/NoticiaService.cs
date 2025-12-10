using PortalTCMSP.Domain.DTOs.Requests.Noticia;
using PortalTCMSP.Domain.DTOs.Requests.NoticiaRequest;
using PortalTCMSP.Domain.DTOs.Responses.Base;
using PortalTCMSP.Domain.DTOs.Responses.Noticia;
using PortalTCMSP.Domain.Entities.NoticiaEntity;
using PortalTCMSP.Domain.Helper;
using PortalTCMSP.Domain.Mappings.Noticia;
using PortalTCMSP.Domain.Repositories.Home;
using PortalTCMSP.Domain.Repositories.Noticia;
using PortalTCMSP.Domain.Services.Noticia;

namespace PortalTCMSP.Infra.Services.Noticia
{
    public class NoticiaService : INoticiaService
    {
        private readonly INoticiaRepository _noticiaRepository;
        private readonly ICategoriaRepository _categoriaRepository;

        public NoticiaService(INoticiaRepository noticiaRepository, ICategoriaRepository categoriaRepository)
        {
            _noticiaRepository = noticiaRepository;
            _categoriaRepository = categoriaRepository;
        }

        public ResultadoPaginadoResponse<NoticiaResponse> ListarNoticias(NoticiaListarRequest request)
        {
            var all = _noticiaRepository.Search(request).ToList();
            var total = all.Count;
            var page = request.Page < 1 ? 1 : request.Page;
            var count = request.Count <= 0 ? 10 : request.Count;

            var items = all.Skip((page - 1) * count).Take(count).ToList();
            return NoticiaMapper.BuildList(page, count, total, items);
        }

        public async Task<NoticiaCompletaResponse?> ObterNoticiaPorIdAsync(int id)
        {
            var n = await _noticiaRepository.ObterPorIdCompletoAsync(id);
            return n == null ? null : NoticiaMapper.BuildCompleteResponse(n);
        }

        public async Task<NoticiaCompletaResponse?> ObterNoticiaPorSlugAsync(string slug)
        {
            var noticia = await _noticiaRepository.ObterPorSlugAsync(slug);
            return noticia == null ? null : NoticiaMapper.BuildCompleteResponse(noticia);
        }

        public async Task<string> AdicionarAsync(NoticiaCreateRequest request)
        {
            if (request.CategoriaIds is null || request.CategoriaIds.Count == 0)
                throw new ArgumentException("Informe ao menos uma categoria.");

            var ids = request.CategoriaIds.Distinct().Select(i => (int)i).ToList();
            var categorias = await _categoriaRepository.FindByIdsAsync(ids);
            if (categorias.Count == 0)
                throw new ArgumentException("Categorias informadas não existem.");
            if (categorias.Count != ids.Count)
                throw new ArgumentException("Uma ou mais categorias informadas não existem.");

            var entity = NoticiaMapper.ToEntity(request);

            var desired = string.IsNullOrWhiteSpace(request.Slug) ? request.Titulo : request.Slug!;
            entity.Slug = await _noticiaRepository.GerarSlugUnicoAsync(desired);

            foreach (var c in categorias)
                entity.NoticiaCategorias.Add(new NoticiaCategoria { CategoriaId = c.Id });

            await _noticiaRepository.AdicionarAsync(entity);
            return entity.Slug;
        }

        public async Task<bool> AtualizarAsync(NoticiaUpdateRequest request)
        {
            var noticia = await _noticiaRepository.ObterPorIdComBlocosAsync((int)request.Id);
            if (noticia == null) return false;

            if (request.CategoriaIds is null || request.CategoriaIds.Count == 0)
                throw new ArgumentException("Informe ao menos uma categoria.");

            var ids = request.CategoriaIds.Distinct().Select(i => (int)i).ToList();
            var categorias = await _categoriaRepository.FindByIdsAsync(ids);
            if (categorias.Count == 0)
                throw new ArgumentException("Categorias informadas não existem.");
            if (categorias.Count != ids.Count)
                throw new ArgumentException("Uma ou mais categorias informadas não existem.");

            var novoSlug = SlugHelper.Slugify(request.Slug);
            if (!string.Equals(noticia.Slug, novoSlug, StringComparison.OrdinalIgnoreCase))
                request.Slug = await _noticiaRepository.GerarSlugUnicoAsync(novoSlug);

            NoticiaMapper.ApplyUpdate(noticia, request);

            noticia.NoticiaCategorias.Clear();
            foreach (var c in categorias)
                noticia.NoticiaCategorias.Add(new NoticiaCategoria { CategoriaId = c.Id });

            await _noticiaRepository.UpdateAsync(noticia);
            return true;
        }

        public async Task<bool> PatchAsync(long id, NoticiaPatchRequest request)
        {
            var noticia = await _noticiaRepository.ObterPorIdCompletoAsync((int)id);
            if (noticia == null) return false;

            if (request.Slug != null)
            {
                var novoSlug = SlugHelper.Slugify(request.Slug);
                if (!string.Equals(noticia.Slug, novoSlug, StringComparison.OrdinalIgnoreCase))
                    noticia.Slug = await _noticiaRepository.GerarSlugUnicoAsync(novoSlug);
            }
            if (request.Titulo != null) noticia.Titulo = request.Titulo;
            if (request.Subtitulo != null) noticia.Subtitulo = request.Subtitulo;
            if (request.Resumo != null) noticia.Resumo = request.Resumo;

            if (request.DataPublicacao.HasValue) noticia.DataPublicacao = request.DataPublicacao.Value;
            if (request.Ativo.HasValue) noticia.Ativo = request.Ativo.Value;
            if (request.Destaque.HasValue) noticia.Destaque = request.Destaque.Value;

            if (request.AutorNome != null) noticia.Autoria.AutorNome = request.AutorNome;
            if (request.Creditos != null) noticia.Autoria.Creditos = request.Creditos;

            if (request.SeoTitulo != null) noticia.Metadados.SeoTitle = request.SeoTitulo;
            if (request.SeoDescricao != null) noticia.Metadados.SeoDescription = request.SeoDescricao;
            if (request.OgImageUrl != null) noticia.Metadados.OgImageUrl = request.OgImageUrl;
            if (request.Canonical != null) noticia.Metadados.Canonical = request.Canonical;

            if (request.ImagemUrl != null) noticia.ImagemUrl = request.ImagemUrl;
            if (request.ConteudoNoticia != null) noticia.ConteudoNoticia = request.ConteudoNoticia;

            if (request.Tags != null) noticia.Tags = request.Tags;
            if (request.CategoriasExtras != null) noticia.CategoriasExtras = request.CategoriasExtras;

            if (request.Blocos != null)
            {
                noticia.Blocos.Clear();
                foreach (var b in request.Blocos.OrderBy(x => x.Ordem))
                {
                    noticia.Blocos.Add(new NoticiaBloco
                    {
                        Ordem = b.Ordem,
                        Tipo = b.Tipo,
                        ConfigJson = b.Config?.GetRawText(),
                        ValorJson = b.Valor.GetRawText(),
                        CriadoEm = DateTime.UtcNow
                    });
                }
            }

            if (request.CategoriaIds != null)
            {
                var desiredIds = request.CategoriaIds
                    .Where(x => x > 0)
                    .Distinct()
                    .Select(x => (long)x)
                    .ToList();

                if (desiredIds.Count == 0)
                    throw new ArgumentException("Informe ao menos uma categoria.");

                var categorias = await _categoriaRepository.FindByIdsAsync(desiredIds.Select(x => (int)x));
                if (categorias.Count != desiredIds.Count)
                    throw new ArgumentException("Uma ou mais categorias informadas não existem.");

                var currentIds = noticia.NoticiaCategorias.Select(nc => nc.CategoriaId).ToList();

                var toRemove = currentIds.Except(desiredIds).ToList();
                var toAdd = desiredIds.Except(currentIds).ToList();

                if (toRemove.Count > 0)
                {
                    var removals = noticia.NoticiaCategorias
                        .Where(nc => toRemove.Contains(nc.CategoriaId))
                        .ToList();
                    foreach (var nc in removals)
                        noticia.NoticiaCategorias.Remove(nc);
                }

                foreach (var addId in toAdd)
                    noticia.NoticiaCategorias.Add(new NoticiaCategoria { CategoriaId = addId });
            }

            noticia.Auditoria.AtualizadoEm = DateTime.UtcNow;
            noticia.Auditoria.Versao += 1;

            await _noticiaRepository.UpdateAsync(noticia);
            return true;
        }

        public async Task<bool> DeletarAsync(int id)
        {
            var noticia = await _noticiaRepository.FindByIdAsync(id);
            if (noticia == null) return false;

            await _noticiaRepository.DeleteAsync(noticia);
            await _noticiaRepository.CommitAsync();
            return true;
        }
    }
}
