using PortalTCMSP.Domain.DTOs.Requests.Institucional.Base;
using PortalTCMSP.Domain.DTOs.Responses.Institucional.Base;
using PortalTCMSP.Domain.Entities.BlocoEntity;
using PortalTCMSP.Domain.Entities.InstitucionalBlocoEntity;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.Mappings.Institucional
{
    [ExcludeFromCodeCoverage]
    public static class InstitucionalMapper
    {
        public static InstitucionalResponse ToResponse(this PortalTCMSP.Domain.Entities.InstitucionalEntity.Institucional e)
        {
            return new InstitucionalResponse
            {
                Id = e.Id,
                Slug = e.Slug,
                Titulo = e.Titulo,
                Subtitulo = e.Subtitulo,
                Descricao = e.Descricao,
                Resumo = e.Resumo,
                AutorNome = e.AutorNome,
                Creditos = e.Creditos,
                SeoTitulo = e.SeoTitulo,
                SeoDescricao = e.SeoDescricao,
                ImagemUrlPrincipal = e.ImagemUrlPrincipal,
                DataCriacao = e.DataCriacao,
                DataAtualizacao = e.DataAtualizacao,
                DataPublicacao = e.DataPublicacao,
                Ativo = e.Ativo,
                Blocos = e.Blocos
                    .OrderBy(b => b.Ordem)
                    .Select(b => new InstitucionalBlocoResponse
                    {
                        Id = b.Id,
                        Ordem = b.Ordem,
                        Html = b.Html,
                        Titulo = b.Titulo,
                        Subtitulo = b.Subtitulo,
                        Ativo = b.Ativo,
                        Descricoes = b.Descricoes
                        .OrderBy(d => d.Ordem)
                        .Select(d => new InstitucionalBlocoDescricaoResponse
                        {
                            Id = d.Id,
                            Ordem = d.Ordem,
                            Texto = d.Texto,
                            Subtextos = (d.Subtextos != null && d.Subtextos.Any())
                                ? d.Subtextos.OrderBy(s => s.Ordem)
                                             .Select(s => new InstitucionalBlocoSubtextoResponse
                                             {
                                                 Id = s.Id,
                                                 Ordem = s.Ordem,
                                                 Texto = s.Texto
                                             }).ToList()
                                : null
                        }).ToList(),
                        Anexos = b.Anexos
                            .OrderBy(a => a.Ordem)
                            .Select(a => new InstitucionalBlocoAnexoResponse
                            {
                                Id = a.Id,
                                Ordem = a.Ordem,
                                Link = a.Link
                            }).ToList()
                    }).ToList()
            };
        }

        public static List<InstitucionalResponse> ToResponse(this IEnumerable<PortalTCMSP.Domain.Entities.InstitucionalEntity.Institucional> list)
            => list.Select(ToResponse).ToList();

        public static PortalTCMSP.Domain.Entities.InstitucionalEntity.Institucional ToEntity(this CreateInstitucionalRequest req)
        {
            var ent = new PortalTCMSP.Domain.Entities.InstitucionalEntity.Institucional
            {
                Titulo = req.Titulo,
                Slug = req.Slug,
                Subtitulo = req.Subtitulo,
                Descricao = req.Descricao,
                Resumo = req.Resumo,
                AutorNome = req.AutorNome,
                Creditos = req.Creditos,
                SeoTitulo = req.SeoTitulo,
                SeoDescricao = req.SeoDescricao,
                ImagemUrlPrincipal = req.ImagemUrlPrincipal,
                DataCriacao = req.DataCriacao == default ? DateTime.UtcNow : req.DataCriacao,
                DataAtualizacao = req.DataAtualizacao,
                DataPublicacao = req.DataPublicacao,
                Ativo = req.Ativo
            };

            foreach (var b in req.Blocos.OrderBy(x => x.Ordem))
            {
                var bloco = new InstitucionalBloco
                {
                    Ordem = b.Ordem,
                    Html = b.Html,
                    Titulo = b.Titulo,
                    Subtitulo = b.Subtitulo,
                    Ativo = b.Ativo
                };

                foreach (var d in b.Descricoes.OrderBy(x => x.Ordem))
                {
                    var desc = new InstitucionalBlocoDescricao
                    {
                        Ordem = d.Ordem,
                        Texto = d.Texto
                    };

                    if (d.Subtextos != null && d.Subtextos.Count > 0)
                    {
                        foreach (var s in d.Subtextos.OrderBy(x => x.Ordem))
                        {
                            desc.Subtextos.Add(new PortalTCMSP.Domain.Entities.InstitucionalBlocoEntity.InstitucionalBlocoSubtexto
                            {
                                Ordem = s.Ordem,
                                Texto = s.Texto
                            });
                        }
                    }

                    bloco.Descricoes.Add(desc);
                }

                foreach (var a in b.Anexos.OrderBy(x => x.Ordem))
                    bloco.Anexos.Add(new InstitucionalBlocoAnexo { Ordem = a.Ordem, Link = a.Link });

                ent.Blocos.Add(bloco);
            }

            return ent;
        }

        public static UpsertDiff MapUpdate(this PortalTCMSP.Domain.Entities.InstitucionalEntity.Institucional current, UpdateInstitucionalRequest req)
        {
            static bool IsNewId(long? id) => !id.HasValue || id.Value <= 0;

            var diff = new UpsertDiff();

            current.Titulo = req.Titulo;
            current.Subtitulo = req.Subtitulo;
            current.Slug = req.Slug;
            current.Descricao = req.Descricao;
            current.Resumo = req.Resumo;
            current.AutorNome = req.AutorNome;
            current.Creditos = req.Creditos;
            current.SeoTitulo = req.SeoTitulo;
            current.SeoDescricao = req.SeoDescricao;
            current.ImagemUrlPrincipal = req.ImagemUrlPrincipal;
            current.DataAtualizacao = req.DataAtualizacao ?? DateTime.UtcNow;
            current.DataPublicacao = req.DataPublicacao;
            current.Ativo = req.Ativo;

            var currentBlocksById = current.Blocos.ToDictionary(b => b.Id);
            var incomingBlocksById = req.Blocos.Where(b => !IsNewId(b.Id))
                                               .ToDictionary(b => b.Id!.Value, b => b);

            foreach (var blk in current.Blocos.Where(b => !incomingBlocksById.ContainsKey(b.Id)).ToList())
                diff.BlocosToRemove.Add(blk);

            foreach (var bReq in req.Blocos.OrderBy(b => b.Ordem))
            {
                PortalTCMSP.Domain.Entities.BlocoEntity.InstitucionalBloco bloco;

                if (IsNewId(bReq.Id))
                {
                    bloco = new InstitucionalBloco
                    {
                        IdInstitucional = current.Id,
                        Ordem = bReq.Ordem,
                        Html = bReq.Html,
                        Titulo = bReq.Titulo,
                        Subtitulo = bReq.Subtitulo,
                        Ativo = bReq.Ativo
                    };
                    current.Blocos.Add(bloco);
                }
                else
                {
                    if (!currentBlocksById.TryGetValue(bReq.Id!.Value, out bloco))
                        throw new InvalidOperationException($"Bloco Id={bReq.Id} não pertence ao Institucional Id={current.Id}.");

                    bloco.Ordem = bReq.Ordem;
                    bloco.Html = bReq.Html;
                    bloco.Titulo = bReq.Titulo;
                    bloco.Subtitulo = bReq.Subtitulo;
                    bloco.Ativo = bReq.Ativo;
                }

                var currentDescById = bloco.Descricoes.ToDictionary(d => d.Id);
                var incomingDescById = bReq.Descricoes.Where(d => !IsNewId(d.Id))
                                                      .ToDictionary(d => d.Id!.Value, d => d);

                foreach (var d in bloco.Descricoes.Where(d => !incomingDescById.ContainsKey(d.Id)).ToList())
                    diff.DescricoesToRemove.Add(d);

                foreach (var dReq in bReq.Descricoes.OrderBy(d => d.Ordem))
                {
                    InstitucionalBlocoDescricao desc;
                    if (IsNewId(dReq.Id))
                    {
                        desc = new InstitucionalBlocoDescricao
                        {
                            Ordem = dReq.Ordem,
                            Texto = dReq.Texto
                        };
                        bloco.Descricoes.Add(desc);
                    }
                    else
                    {
                        if (!currentDescById.TryGetValue(dReq.Id!.Value, out desc))
                            throw new InvalidOperationException($"Descrição Id={dReq.Id} não pertence ao Bloco Id={bReq.Id}.");

                        desc.Ordem = dReq.Ordem;
                        desc.Texto = dReq.Texto;
                    }
                    var currentSubById = (desc.Subtextos ??= new List<InstitucionalBlocoSubtexto>())
                                         .ToDictionary(s => s.Id);
                    var incomingSubById = (dReq.Subtextos ?? new List<UpdateInstitucionalBlocoSubtextoRequest>())
                                          .Where(s => !IsNewId(s.Id))
                                          .ToDictionary(s => s.Id!.Value, s => s);

                    foreach (var s in desc.Subtextos.Where(s => !incomingSubById.ContainsKey(s.Id)).ToList())
                        diff.SubtextosToRemove.Add(s);

                    if (dReq.Subtextos != null)
                    {
                        foreach (var sReq in dReq.Subtextos.OrderBy(s => s.Ordem))
                        {
                            if (IsNewId(sReq.Id))
                            {
                                desc.Subtextos.Add(new InstitucionalBlocoSubtexto
                                {
                                    Ordem = sReq.Ordem,
                                    Texto = sReq.Texto
                                });
                            }
                            else
                            {
                                if (!currentSubById.TryGetValue(sReq.Id!.Value, out var sub))
                                    throw new InvalidOperationException($"Subtexto Id={sReq.Id} não pertence à Descrição Id={dReq.Id}.");

                                sub.Ordem = sReq.Ordem;
                                sub.Texto = sReq.Texto;
                            }
                        }
                    }
                }
                var currentAnxById = bloco.Anexos.ToDictionary(a => a.Id);
                var incomingAnxById = bReq.Anexos.Where(a => !IsNewId(a.Id))
                                                 .ToDictionary(a => a.Id!.Value, a => a);

                foreach (var a in bloco.Anexos.Where(a => !incomingAnxById.ContainsKey(a.Id)).ToList())
                    diff.AnexosToRemove.Add(a);

                foreach (var aReq in bReq.Anexos.OrderBy(a => a.Ordem))
                {
                    if (IsNewId(aReq.Id))
                    {
                        bloco.Anexos.Add(new InstitucionalBlocoAnexo
                        {
                            Ordem = aReq.Ordem,
                            Link = aReq.Link
                        });
                    }
                    else
                    {
                        if (!currentAnxById.TryGetValue(aReq.Id!.Value, out var anx))
                            throw new InvalidOperationException($"Anexo Id={aReq.Id} não pertence ao Bloco Id={bReq.Id}.");

                        anx.Ordem = aReq.Ordem;
                        anx.Link = aReq.Link;
                    }
                }
            }

            return diff;
        }

        public sealed class UpsertDiff
        {
            public List<InstitucionalBloco> BlocosToRemove { get; } = new();
            public List<InstitucionalBlocoDescricao> DescricoesToRemove { get; } = new();
            public List<InstitucionalBlocoAnexo> AnexosToRemove { get; } = new();
            public List<InstitucionalBlocoSubtexto> SubtextosToRemove { get; } = new();
        }
    }
}