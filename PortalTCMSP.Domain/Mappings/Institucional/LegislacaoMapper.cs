using PortalTCMSP.Domain.DTOs.Requests.Institucional.Base;
using PortalTCMSP.Domain.DTOs.Requests.Institucional.Legislacao;
using PortalTCMSP.Domain.DTOs.Responses.Institucional.Base;
using PortalTCMSP.Domain.DTOs.Responses.Institucional.Legislacao;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.Mappings.Institucional
{
    [ExcludeFromCodeCoverage]
    public static class LegislacaoMapper
    {
        // Create -> Institucional
        public static CreateInstitucionalRequest ToInstitucionalCreate(this CreateLegislacaoRequest req)
        {
            var create = new CreateInstitucionalRequest
            {
                Slug = req.Slug,
                Titulo = req.Titulo,
                Subtitulo = req.Subtitulo,
                Descricao = req.Descricao,
                AutorNome = req.AutorNome,
                Creditos = req.Creditos,
                SeoTitulo = req.SeoTitulo,
                SeoDescricao = req.SeoDescricao,
                DataCriacao = req.DataCriacao ?? DateTime.UtcNow,
                DataAtualizacao = req.DataAtualizacao,
                DataPublicacao = req.DataPublicacao,
                Ativo = req.Ativo
            };

            foreach (var b in req.Blocos.OrderBy(x => x.Ordem))
            {
                var bloco = new InstitucionalBlocoRequest
                {
                    Ordem = b.Ordem,
                    Html = b.Html,
                    Titulo = b.Titulo,
                    Subtitulo = null,
                    Ativo = "S"
                };

                foreach (var a in b.Anexos.OrderBy(x => x.Ordem))
                    bloco.Anexos.Add(new InstitucionalBlocoAnexoRequest { Ordem = a.Ordem, Link = a.Link });

                create.Blocos.Add(bloco);
            }

            return create;
        }

        // Update -> Institucional (upsert)
        public static UpdateInstitucionalRequest ToInstitucionalUpdate(this UpdateLegislacaoRequest req)
        {
            var update = new UpdateInstitucionalRequest
            {
                Slug = req.Slug,
                Titulo = req.Titulo,
                Subtitulo = req.Subtitulo,
                Descricao = req.Descricao,
                AutorNome = req.AutorNome,
                Creditos = req.Creditos,
                SeoTitulo = req.SeoTitulo,
                SeoDescricao = req.SeoDescricao,
                DataAtualizacao = req.DataAtualizacao ?? DateTime.UtcNow,
                DataPublicacao = req.DataPublicacao,
                Ativo = req.Ativo
            };

            foreach (var b in req.Blocos.OrderBy(x => x.Ordem))
            {
                var bloco = new UpdateInstitucionalBlocoRequest
                {
                    Id = b.Id,
                    Ordem = b.Ordem,
                    Html = b.Html,
                    Titulo = b.Titulo,
                    Subtitulo = null,
                    Ativo = "S"
                };

                foreach (var a in b.Anexos.OrderBy(x => x.Ordem))
                    bloco.Anexos.Add(new UpdateInstitucionalBlocoAnexoRequest { Id = a.Id, Ordem = a.Ordem, Link = a.Link });

                update.Blocos.Add(bloco);
            }

            return update;
        }

        // Institucional -> Legislação (Response)
        public static LegislacaoResponse ToLegislacaoResponse(this InstitucionalResponse r)
        {
            var resp = new LegislacaoResponse
            {
                Id = r.Id,
                Slug = r.Slug,
                Titulo = r.Titulo,
                Subtitulo = r.Subtitulo,
                Descricao = r.Descricao,
                AutorNome = r.AutorNome,
                Creditos = r.Creditos,
                SeoTitulo = r.SeoTitulo,
                SeoDescricao = r.SeoDescricao,
                DataCriacao = r.DataCriacao,
                DataAtualizacao = r.DataAtualizacao,
                DataPublicacao = r.DataPublicacao,
                Ativo = r.Ativo
            };

            foreach (var b in r.Blocos.OrderBy(x => x.Ordem))
            {
                resp.Blocos.Add(new LegislacaoBlocoResponse
                {
                    Id = b.Id,
                    Ordem = b.Ordem,
                    Html = b.Html,
                    Titulo = b.Titulo,
                    Anexos = b.Anexos
                        .OrderBy(a => a.Ordem)
                        .Select(a => new LegislacaoAnexoResponse { Id = a.Id, Ordem = a.Ordem, Link = a.Link })
                        .ToList()
                });
            }

            return resp;
        }

        public static List<LegislacaoResponse> ToLegislacaoResponse(this IEnumerable<InstitucionalResponse> list)
            => list.Select(ToLegislacaoResponse).ToList();
    }
}
