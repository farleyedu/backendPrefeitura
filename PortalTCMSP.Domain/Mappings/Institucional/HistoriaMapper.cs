using PortalTCMSP.Domain.DTOs.Requests.Institucional.Base;
using PortalTCMSP.Domain.DTOs.Requests.Institucional.Historia;
using PortalTCMSP.Domain.DTOs.Requests.Institucional.Historia.PortalTCMSP.Domain.DTOs.Requests.Institucional.Historia;
using PortalTCMSP.Domain.DTOs.Responses.Institucional.Base;
using PortalTCMSP.Domain.DTOs.Responses.Institucional.Historia;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.Mappings.Institucional
{
    [ExcludeFromCodeCoverage]
    public static class HistoriaMapper
    {
        public static CreateInstitucionalRequest ToInstitucionalCreate(this CreateHistoriaRequest req)
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
                Ativo = req.Ativo
            };

            foreach (var b in req.Blocos.OrderBy(x => x.Ordem))
            {
                var bloco = new InstitucionalBlocoRequest
                {
                    Ordem = b.Ordem,
                    Html = null,
                    Titulo = b.Titulo,
                    Subtitulo = b.Subtitulo,
                    Ativo = "S"
                };

                foreach (var d in b.Descricoes.OrderBy(x => x.Ordem))
                    bloco.Descricoes.Add(new InstitucionalBlocoDescricaoRequest { Ordem = d.Ordem, Texto = d.Texto });

                if (!string.IsNullOrWhiteSpace(b.ImagemUrl))
                    bloco.Anexos.Add(new InstitucionalBlocoAnexoRequest { Ordem = 0, Link = b.ImagemUrl! });

                create.Blocos.Add(bloco);
            }

            return create;
        }

        public static UpdateInstitucionalRequest ToInstitucionalUpdate(this UpdateHistoriaRequest req)
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
                Ativo = req.Ativo
            };

            foreach (var b in req.Blocos.OrderBy(x => x.Ordem))
            {
                var bloco = new UpdateInstitucionalBlocoRequest
                {
                    Id = b.Id,
                    Ordem = b.Ordem,
                    Html = null,
                    Titulo = b.Titulo,
                    Subtitulo = b.Subtitulo,
                    Ativo = "S"
                };

                foreach (var d in b.Descricoes.OrderBy(x => x.Ordem))
                    bloco.Descricoes.Add(new UpdateInstitucionalBlocoDescricaoRequest { Id = d.Id, Ordem = d.Ordem, Texto = d.Texto });

                if (!string.IsNullOrWhiteSpace(b.ImagemUrl))
                    bloco.Anexos.Add(new UpdateInstitucionalBlocoAnexoRequest { Id = null, Ordem = 0, Link = b.ImagemUrl! });

                update.Blocos.Add(bloco);
            }

            return update;
        }

        public static HistoriaResponse ToHistoriaResponse(this InstitucionalResponse r)
        {
            var resp = new HistoriaResponse
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
                Ativo = r.Ativo
            };

            foreach (var b in r.Blocos.OrderBy(x => x.Ordem))
            {
                resp.Blocos.Add(new HistoriaBlocoResponse
                {
                    Id = b.Id,
                    Ordem = b.Ordem,
                    Titulo = b.Titulo,
                    Subtitulo = b.Subtitulo,
                    Descricoes = b.Descricoes.OrderBy(d => d.Ordem)
                        .Select(d => new HistoriaDescricaoResponse { Id = d.Id, Ordem = d.Ordem, Texto = d.Texto }).ToList(),
                    ImagemUrl = b.Anexos.FirstOrDefault()?.Link
                });
            }

            return resp;
        }

        public static List<HistoriaResponse> ToHistoriaResponse(this IEnumerable<InstitucionalResponse> list)
            => list.Select(ToHistoriaResponse).ToList();
    }
}
