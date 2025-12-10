using PortalTCMSP.Domain.DTOs.Requests.Institucional.Base;
using PortalTCMSP.Domain.DTOs.Requests.Institucional.Organograma;
using PortalTCMSP.Domain.DTOs.Responses.Institucional.Base;
using PortalTCMSP.Domain.DTOs.Responses.Institucional.Organograma;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.Mappings.Institucional
{
    [ExcludeFromCodeCoverage]
    public static class OrganogramaMapper
    {
        public static CreateInstitucionalRequest ToInstitucionalCreate(this CreateOrganogramaRequest req)
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

                if (!string.IsNullOrWhiteSpace(b.ImagemUrl))
                    bloco.Anexos.Add(new InstitucionalBlocoAnexoRequest
                    {
                        Ordem = 0,
                        Link = b.ImagemUrl!
                    });

                create.Blocos.Add(bloco);
            }

            return create;
        }

        public static UpdateInstitucionalRequest ToInstitucionalUpdate(this UpdateOrganogramaRequest req)
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

                // Mantemos um único anexo como imagem do bloco (se veio)
                if (!string.IsNullOrWhiteSpace(b.ImagemUrl))
                    bloco.Anexos.Add(new UpdateInstitucionalBlocoAnexoRequest
                    {
                        Id = null,
                        Ordem = 0,
                        Link = b.ImagemUrl!
                    });

                update.Blocos.Add(bloco);
            }

            return update;
        }

        public static OrganogramaResponse ToOrganogramaResponse(this InstitucionalResponse r)
        {
            var resp = new OrganogramaResponse
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
                resp.Blocos.Add(new OrganogramaBlocoResponse
                {
                    Id = b.Id,
                    Ordem = b.Ordem,
                    Titulo = b.Titulo,
                    ImagemUrl = b.Anexos.FirstOrDefault()?.Link
                });
            }

            return resp;
        }

        public static List<OrganogramaResponse> ToOrganogramaResponse(this IEnumerable<InstitucionalResponse> list)
            => list.Select(ToOrganogramaResponse).ToList();
    }
}
