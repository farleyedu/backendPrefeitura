using PortalTCMSP.Domain.DTOs.Requests.Institucional.Base;
using PortalTCMSP.Domain.DTOs.Requests.Institucional.Colegiado;
using PortalTCMSP.Domain.DTOs.Responses.Institucional.Base;
using PortalTCMSP.Domain.DTOs.Responses.Institucional.Colegiado;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.Mappings.Institucional
{
    [ExcludeFromCodeCoverage]
    public static class ColegiadoMapper
    {
        public static CreateInstitucionalRequest ToInstitucionalCreate(this CreateColegiadoRequest req)
        {
            var create = new CreateInstitucionalRequest
            {
                Slug = req.Slug,
                Titulo = req.Titulo,
                Subtitulo = req.Subtitulo,
                Descricao = req.Descricao,
                Resumo = null,
                AutorNome = req.AutorNome,
                Creditos = req.Creditos,
                SeoTitulo = req.SeoTitulo,
                SeoDescricao = req.SeoDescricao,
                ImagemUrlPrincipal = null,
                DataCriacao = req.DataCriacao ?? DateTime.UtcNow,
                DataAtualizacao = req.DataAtualizacao,
                DataPublicacao = req.DataPublicacao,
                Ativo = req.Ativo
            };

            foreach (var m in req.Blocos.OrderBy(x => x.Ordem))
            {
                var bloco = new InstitucionalBlocoRequest
                {
                    Ordem = m.Ordem,
                    Html = null,
                    Titulo = m.Nome,
                    Subtitulo = m.Cargo,
                    Ativo = "S"
                };

                bloco.Descricoes.Add(new InstitucionalBlocoDescricaoRequest
                {
                    Ordem = 0,
                    Texto = m.Descricao
                });

                if (!string.IsNullOrWhiteSpace(m.ImagemUrl))
                {
                    bloco.Anexos.Add(new InstitucionalBlocoAnexoRequest
                    {
                        Ordem = 0,
                        Link = m.ImagemUrl!
                    });
                }

                create.Blocos.Add(bloco);
            }

            return create;
        }

        // Colegiado -> Institucional (Update / Upsert)
        public static UpdateInstitucionalRequest ToInstitucionalUpdate(this UpdateColegiadoRequest req)
        {
            var update = new UpdateInstitucionalRequest
            {
                Slug = req.Slug,
                Titulo = req.Titulo,
                Subtitulo = req.Subtitulo,
                Descricao = req.Descricao,
                Resumo = null,
                AutorNome = req.AutorNome,
                Creditos = req.Creditos,
                SeoTitulo = req.SeoTitulo,
                SeoDescricao = req.SeoDescricao,
                ImagemUrlPrincipal = null,
                DataAtualizacao = req.DataAtualizacao ?? DateTime.UtcNow,
                DataPublicacao = req.DataPublicacao,
                Ativo = req.Ativo
            };

            foreach (var m in req.Blocos.OrderBy(x => x.Ordem))
            {
                var bloco = new UpdateInstitucionalBlocoRequest
                {
                    Id = m.Id,
                    Ordem = m.Ordem,
                    Html = null,
                    Titulo = m.Nome,
                    Subtitulo = m.Cargo,
                    Ativo = "S"
                };

                bloco.Descricoes.Add(new UpdateInstitucionalBlocoDescricaoRequest
                {
                    Id = null,
                    Ordem = 0,
                    Texto = m.Descricao
                });

                if (!string.IsNullOrWhiteSpace(m.ImagemUrl))
                {
                    bloco.Anexos.Add(new UpdateInstitucionalBlocoAnexoRequest
                    {
                        Id = null,
                        Ordem = 0,
                        Link = m.ImagemUrl!
                    });
                }

                update.Blocos.Add(bloco);
            }

            return update;
        }

        public static ColegiadoResponse ToColegiadoResponse(this InstitucionalResponse r)
        {
            var resp = new ColegiadoResponse
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
                resp.Blocos.Add(new ColegiadoMembroResponse
                {
                    Id = b.Id,
                    Ordem = b.Ordem,
                    Nome = b.Titulo ?? string.Empty,
                    Cargo = b.Subtitulo ?? string.Empty,
                    Descricao = b.Descricoes.FirstOrDefault()?.Texto ?? string.Empty,
                    ImagemUrl = b.Anexos.FirstOrDefault()?.Link
                });
            }

            return resp;
        }

        public static List<ColegiadoResponse> ToColegiadoResponse(this IEnumerable<InstitucionalResponse> list)
            => list.Select(ToColegiadoResponse).ToList();
    }
}
