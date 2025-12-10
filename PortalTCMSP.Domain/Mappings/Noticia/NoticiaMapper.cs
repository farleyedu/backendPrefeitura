using PortalTCMSP.Domain.DTOs.Requests.NoticiaRequest;
using PortalTCMSP.Domain.DTOs.Responses.Base;
using PortalTCMSP.Domain.DTOs.Responses.Noticia;
using PortalTCMSP.Domain.Entities.NoticiaEntity;
using PortalTCMSP.Domain.Helper;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace PortalTCMSP.Domain.Mappings.Noticia
{
    [ExcludeFromCodeCoverage]
    public static class NoticiaMapper
    {
        public static PortalTCMSP.Domain.Entities.NoticiaEntity.Noticia ToEntity(NoticiaCreateRequest req)
        {
            var entity = new PortalTCMSP.Domain.Entities.NoticiaEntity.Noticia
            {
                Slug = string.IsNullOrWhiteSpace(req.Slug) ? SlugHelper.Slugify(req.Titulo) : SlugHelper.Slugify(req.Slug),
                Titulo = req.Titulo,
                Subtitulo = req.Subtitulo,
                Resumo = req.Resumo,
                ImagemUrl = req.ImagemUrl,
                ConteudoNoticia = req.ConteudoNoticia,
                Tags = req.Tags,
                CategoriasExtras = req.CategoriasExtras,
                Ativo = req.Ativo,
                Destaque = req.Destaque,
                DataPublicacao = req.DataPublicacao ?? DateTime.UtcNow,
                Autoria = new Autoria { AutorNome = req.AutorNome, Creditos = req.Creditos },
                Metadados = new Metadados
                {
                    SeoTitle = req.SeoTitulo,
                    SeoDescription = req.SeoDescricao,
                    OgImageUrl = req.OgImageUrl,
                    Canonical = req.Canonical
                },
                Auditoria = new Auditoria { CriadoEm = DateTime.UtcNow, AtualizadoEm = DateTime.UtcNow, Versao = 1 }
            };

            if (req.Blocos != null)
            {
                entity.Blocos = [.. req.Blocos
                    .OrderBy(b => b.Ordem)
                    .Select(b => new NoticiaBloco
                    {
                        Ordem = b.Ordem,
                        Tipo = b.Tipo,
                        ConfigJson = b.Config?.GetRawText(),
                        ValorJson = b.Valor.GetRawText(),
                        CriadoEm = DateTime.UtcNow
                    })];
            }

            return entity;
        }

        public static void ApplyUpdate(PortalTCMSP.Domain.Entities.NoticiaEntity.Noticia entity, NoticiaUpdateRequest req)
        {
            entity.Slug = SlugHelper.Slugify(req.Slug);
            entity.Titulo = req.Titulo;
            entity.Subtitulo = req.Subtitulo;
            entity.Resumo = req.Resumo;
            entity.ImagemUrl = req.ImagemUrl;
            entity.Tags = req.Tags;
            entity.CategoriasExtras = req.CategoriasExtras;
            entity.Ativo = req.Ativo;
            entity.ConteudoNoticia = req.ConteudoNoticia;
            entity.Destaque = req.Destaque;
            if (req.DataPublicacao.HasValue) entity.DataPublicacao = req.DataPublicacao.Value;

            entity.Autoria.AutorNome = req.AutorNome;
            entity.Autoria.Creditos = req.Creditos;

            entity.Metadados.SeoTitle = req.SeoTitulo;
            entity.Metadados.SeoDescription = req.SeoDescricao;
            entity.Metadados.OgImageUrl = req.OgImageUrl;
            entity.Metadados.Canonical = req.Canonical;

            entity.Auditoria.AtualizadoEm = DateTime.UtcNow;
            entity.Auditoria.Versao += 1;

            entity.Blocos.Clear();
            if (req.Blocos != null)
            {
                foreach (var b in req.Blocos.OrderBy(x => x.Ordem))
                {
                    entity.Blocos.Add(new NoticiaBloco
                    {
                        Ordem = b.Ordem,
                        Tipo = b.Tipo,
                        ConfigJson = b.Config?.GetRawText(),
                        ValorJson = b.Valor.GetRawText(),
                        CriadoEm = DateTime.UtcNow
                    });
                }
            }
        }

        public static NoticiaCompletaResponse BuildCompleteResponse(PortalTCMSP.Domain.Entities.NoticiaEntity.Noticia n)
        {
            var resp = new NoticiaCompletaResponse
            {
                Id = n.Id,
                Slug = n.Slug,
                Titulo = n.Titulo,
                Subtitulo = n.Subtitulo,
                Resumo = n.Resumo,
                AutorNome = n.Autoria.AutorNome,
                Creditos = n.Autoria.Creditos,
                SeoTitulo = n.Metadados.SeoTitle,
                SeoDescricao = n.Metadados.SeoDescription,
                OgImageUrl = n.Metadados.OgImageUrl,
                Canonical = n.Metadados.Canonical,
                PublicadoQuando = n.DataPublicacao,
                Ativo = n.Ativo,
                Destaques = n.Destaque,
                Tags = n.Tags,
                CategoriasExtras = n.CategoriasExtras,
                ImageUrl = n.ImagemUrl,
                CriadoEm = n.Auditoria.CriadoEm,
                CriadoPor = n.Auditoria.CriadoPor,
                AtualizadoEm = n.Auditoria.AtualizadoEm,
                AtualizadoPor = n.Auditoria.AtualizadoPor,
                Versao = n.Auditoria.Versao,
                ConteudoNoticia = n.ConteudoNoticia
            };

            resp.Categorias = n.NoticiaCategorias
                .Select(nc => new CategoriaItem { Id = nc.Categoria.Id, Nome = nc.Categoria.Nome, Slug = nc.Categoria.Slug })
                .ToList();

            foreach (var b in n.Blocos.OrderBy(x => x.Ordem))
            {
                resp.Blocos.Add(new BlocoResponse
                {
                    Ordem = b.Ordem,
                    Tipo = b.Tipo,
                    Config = string.IsNullOrWhiteSpace(b.ConfigJson) ? (JsonElement?)null : JsonDocument.Parse(b.ConfigJson!).RootElement,
                    Valor = JsonDocument.Parse(b.ValorJson).RootElement
                });
            }

            return resp;
        }

        public static ResultadoPaginadoResponse<NoticiaResponse> BuildList(
            int page, int count, int total, IEnumerable<PortalTCMSP.Domain.Entities.NoticiaEntity.Noticia> items)
        {
            var list = items.Select(n => new NoticiaResponse
            {
                Id = n.Id,
                Slug = n.Slug,
                Titulo = n.Titulo,
                Subtitulo = n.Subtitulo,
                Resumo = n.Resumo,
                PublicadoQuando = n.DataPublicacao,
                Autor = n.Autoria.AutorNome,
                Categorias = n.NoticiaCategorias
                    .Select(nc => new CategoriaItem { Id = nc.Categoria.Id, Nome = nc.Categoria.Nome, Slug = nc.Categoria.Slug })
                    .ToList(),
                Views = n.Visualizacao,
                ImageUrl = n.ImagemUrl,
                Ativo = n.Ativo,
                Destaques = n.Destaque,
                ConteudoNoticia = n.ConteudoNoticia ?? string.Empty,
            });

            return new ResultadoPaginadoResponse<NoticiaResponse>(page, count, total, list);
        }
    }
}