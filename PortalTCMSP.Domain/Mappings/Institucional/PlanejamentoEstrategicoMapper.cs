using PortalTCMSP.Domain.DTOs.Requests.Institucional.Base;
using PortalTCMSP.Domain.DTOs.Requests.Institucional.PlanejamentoEstrategico;
using PortalTCMSP.Domain.DTOs.Responses.Institucional.Base;
using PortalTCMSP.Domain.DTOs.Responses.Institucional.PlanejamentoEstrategico;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.Mappings.Institucional
{
    [ExcludeFromCodeCoverage]
    public static class PlanejamentoEstrategicoMapper
    {
        // Create -> Institucional
        public static CreateInstitucionalRequest ToInstitucionalCreate(this CreatePlanejamentoEstrategicoRequest req)
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
                    Subtitulo = null,
                    Ativo = "S"
                };

                foreach (var d in b.Descricoes.OrderBy(x => x.Ordem))
                    bloco.Descricoes.Add(new InstitucionalBlocoDescricaoRequest { Ordem = d.Ordem, Texto = d.Texto });

                create.Blocos.Add(bloco);
            }
            return create;
        }

        // Update -> Institucional (upsert)
        public static UpdateInstitucionalRequest ToInstitucionalUpdate(this UpdatePlanejamentoEstrategicoRequest req)
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
                    Subtitulo = null,
                    Ativo = "S"
                };

                foreach (var d in b.Descricoes.OrderBy(x => x.Ordem))
                    bloco.Descricoes.Add(new UpdateInstitucionalBlocoDescricaoRequest
                    {
                        Id = d.Id,
                        Ordem = d.Ordem,
                        Texto = d.Texto
                    });

                update.Blocos.Add(bloco);
            }
            return update;
        }

        // Institucional -> PlanejamentoEstratégico
        public static PlanejamentoEstrategicoResponse ToPlanejamentoEstrategicoResponse(this InstitucionalResponse r)
        {
            var resp = new PlanejamentoEstrategicoResponse
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
                resp.Blocos.Add(new PlanejamentoEstrategicoBlocoResponse
                {
                    Id = b.Id,
                    Ordem = b.Ordem,
                    Titulo = b.Titulo,
                    Descricoes = b.Descricoes.OrderBy(d => d.Ordem)
                        .Select(d => new PlanejamentoEstrategicoDescricaoResponse { Id = d.Id, Ordem = d.Ordem, Texto = d.Texto })
                        .ToList()
                });
            }
            return resp;
        }

        public static List<PlanejamentoEstrategicoResponse> ToPlanejamentoEstrategicoResponse(this IEnumerable<InstitucionalResponse> list)
            => list.Select(ToPlanejamentoEstrategicoResponse).ToList();
    }
}
