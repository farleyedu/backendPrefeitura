using PortalTCMSP.Domain.DTOs.Requests.Institucional.Base;
using PortalTCMSP.Domain.DTOs.Requests.Institucional.Relatorias;
using PortalTCMSP.Domain.DTOs.Responses.Institucional.Base;
using PortalTCMSP.Domain.DTOs.Responses.Institucional.Relatorias;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.Mappings.Institucional
{
    [ExcludeFromCodeCoverage]
    public static class RelatoriasMapper
    {
        // Create -> Institucional
        public static CreateInstitucionalRequest ToInstitucionalCreate(this CreateRelatoriasRequest req)
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
                {
                    var dReq = new InstitucionalBlocoDescricaoRequest
                    {
                        Ordem = d.Ordem,
                        Texto = d.Texto
                    };

                    if (d.Subtextos != null && d.Subtextos.Count > 0)
                    {
                        foreach (var s in d.Subtextos.OrderBy(x => x.Ordem))
                        {
                            dReq.Subtextos.Add(new CreateInstitucionalBlocoSubtextoRequest
                            {
                                Ordem = s.Ordem,
                                Texto = s.Texto
                            });
                        }
                    }

                    bloco.Descricoes.Add(dReq);
                }

                if (!string.IsNullOrWhiteSpace(b.ImagemUrl))
                    bloco.Anexos.Add(new InstitucionalBlocoAnexoRequest { Ordem = 0, Link = b.ImagemUrl! });

                create.Blocos.Add(bloco);
            }
            return create;
        }

        // Update -> Institucional (upsert)
        public static UpdateInstitucionalRequest ToInstitucionalUpdate(this UpdateRelatoriasRequest req)
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
                {
                    var dReq = new UpdateInstitucionalBlocoDescricaoRequest
                    {
                        Id = d.Id,
                        Ordem = d.Ordem,
                        Texto = d.Texto
                    };

                    if (d.Subtextos != null && d.Subtextos.Count > 0)
                    {
                        foreach (var s in d.Subtextos.OrderBy(x => x.Ordem))
                        {
                            dReq.Subtextos.Add(new UpdateInstitucionalBlocoSubtextoRequest
                            {
                                Id = s.Id,
                                Ordem = s.Ordem,
                                Texto = s.Texto
                            });
                        }
                    }

                    bloco.Descricoes.Add(dReq);
                }

                if (!string.IsNullOrWhiteSpace(b.ImagemUrl))
                    bloco.Anexos.Add(new UpdateInstitucionalBlocoAnexoRequest { Id = null, Ordem = 0, Link = b.ImagemUrl! });

                update.Blocos.Add(bloco);
            }
            return update;
        }

        // Institucional -> Relatorias (Response)
        public static RelatoriasResponse ToRelatoriasResponse(this InstitucionalResponse r)
        {
            var resp = new RelatoriasResponse
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
                resp.Blocos.Add(new RelatoriasBlocoResponse
                {
                    Id = b.Id,
                    Ordem = b.Ordem,
                    Titulo = b.Titulo,
                    Subtitulo = b.Subtitulo,
                    Descricoes = b.Descricoes
                        .OrderBy(d => d.Ordem)
                        .Select(d => new RelatoriasDescricaoResponse
                        {
                            Id = d.Id,
                            Ordem = d.Ordem,
                            Texto = d.Texto,
                            Subtextos = d.Subtextos == null ? null :
                        d.Subtextos.OrderBy(s => s.Ordem)
                                   .Select(s => new PortalTCMSP.Domain.DTOs.Responses.Institucional.Relatorias.InstitucionalBlocoSubtextoResponse
                                   {
                                       Id = s.Id,
                                       Ordem = s.Ordem,
                                       Texto = s.Texto
                                   }).ToList()
                        }).ToList(),
                    ImagemUrl = b.Anexos.FirstOrDefault()?.Link
                });
            }
            return resp;
        }

        public static List<RelatoriasResponse> ToRelatoriasResponse(this IEnumerable<InstitucionalResponse> list)
            => list.Select(ToRelatoriasResponse).ToList();
    }
}
