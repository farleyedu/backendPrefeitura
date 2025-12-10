using PortalTCMSP.Domain.DTOs.Requests.Institucional.Base;
using PortalTCMSP.Domain.DTOs.Requests.Institucional.Iso9001;
using PortalTCMSP.Domain.DTOs.Responses.Institucional.Base;
using PortalTCMSP.Domain.DTOs.Responses.Institucional.Iso9001;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.Mappings.Institucional
{
    [ExcludeFromCodeCoverage]
    public static class Iso9001Mapper
    {
        public static CreateInstitucionalRequest ToInstitucionalCreate(this CreateIso9001Request req)
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

                foreach (var d in b.Descricoes.OrderBy(x => x.Ordem))
                    bloco.Descricoes.Add(new InstitucionalBlocoDescricaoRequest { Ordem = d.Ordem, Texto = d.Texto });

                foreach (var a in b.Anexos.OrderBy(x => x.Ordem))
                    bloco.Anexos.Add(new InstitucionalBlocoAnexoRequest { Ordem = a.Ordem, Link = a.Link });

                create.Blocos.Add(bloco);
            }

            return create;
        }

        public static UpdateInstitucionalRequest ToInstitucionalUpdate(this UpdateIso9001Request req)
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

                foreach (var d in b.Descricoes.OrderBy(x => x.Ordem))
                    bloco.Descricoes.Add(new UpdateInstitucionalBlocoDescricaoRequest { Id = d.Id, Ordem = d.Ordem, Texto = d.Texto });

                foreach (var a in b.Anexos.OrderBy(x => x.Ordem))
                    bloco.Anexos.Add(new UpdateInstitucionalBlocoAnexoRequest { Id = a.Id, Ordem = a.Ordem, Link = a.Link });

                update.Blocos.Add(bloco);
            }

            return update;
        }

        public static Iso9001Response ToIso9001Response(this InstitucionalResponse r)
        {
            var resp = new Iso9001Response
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
                resp.Blocos.Add(new Iso9001BlocoResponse
                {
                    Id = b.Id,
                    Ordem = b.Ordem,
                    Titulo = b.Titulo,
                    Html = b.Html,
                    Descricoes = b.Descricoes.OrderBy(d => d.Ordem)
                        .Select(d => new Iso9001DescricaoResponse { Id = d.Id, Ordem = d.Ordem, Texto = d.Texto })
                        .ToList(),
                    Anexos = b.Anexos.OrderBy(a => a.Ordem)
                        .Select(a => new Iso9001AnexoResponse { Id = a.Id, Ordem = a.Ordem, Link = a.Link })
                        .ToList()
                });
            }

            return resp;
        }

        public static List<Iso9001Response> ToIso9001Response(this IEnumerable<InstitucionalResponse> list)
            => list.Select(ToIso9001Response).ToList();
    }
}
