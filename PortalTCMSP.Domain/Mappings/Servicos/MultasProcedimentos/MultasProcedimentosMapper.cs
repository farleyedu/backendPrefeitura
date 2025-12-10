using PortalTCMSP.Domain.DTOs.Requests.Servicos.MultasProcedimentos;
using PortalTCMSP.Domain.DTOs.Responses.Servicos.MultasProcedimentos;
using PortalTCMSP.Domain.Helper;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.Mappings.Servicos.MultasProcedimentos
{
    [ExcludeFromCodeCoverage]
    public static class MultasProcedimentosMapper
    {
        public static MultasProcedimentosResponse ToResponse(this Entities.ServicosEntity.MultasProcedimentosEntity.MultasProcedimentos e) => new()
        {
            Id = e.Id,
            TituloPagina = e.TituloPagina,
            Slug = e.Slug,
            Ativo = e.Ativo,
            DataCriacao = e.DataCriacao,
            DataAtualizacao = e.DataAtualizacao,
            Procedimentos = [.. e.Procedimentos.OrderBy(p => p.Ordem).Select(p => new MultasProcedimentosProcedimentoResponse
        {
            Id = p.Id,
            Ordem = p.Ordem,
            Texto = p.Texto,
            UrlImagem = p.UrlImagem
        })],
            PortariasRelacionadas = [.. e.PortariasRelacionadas.OrderBy(p => p.Ordem).Select(p => new MultasProcedimentosPortariaRelacionadaResponse
        {
            Id = p.Id,
            Ordem = p.Ordem,
            Titulo = p.Titulo,
            Url = p.Url
        })]
        };

        public static IEnumerable<MultasProcedimentosResponse> ToResponse(this IEnumerable<Entities.ServicosEntity.MultasProcedimentosEntity.MultasProcedimentos> list)
            => list.Select(ToResponse);

        public static Entities.ServicosEntity.MultasProcedimentosEntity.MultasProcedimentos FromCreate(this MultasProcedimentosCreateRequest r, DateTime nowUtc) => new()
        {
            TituloPagina = r.TituloPagina.Trim(),
            Slug = SlugHelper.Slugify(r.Slug),
            Ativo = r.Ativo,
            DataCriacao = nowUtc,
            Procedimentos = r.Procedimentos?.Select(p => new Entities.ServicosEntity.MultasProcedimentosEntity.MultasProcedimentosProcedimento
            {
                Ordem = p.Ordem,
                Texto = p.Texto.Trim(),
                UrlImagem = p.UrlImagem?.Trim()
            }).ToList() ?? [],
            PortariasRelacionadas = r.PortariasRelacionadas?.Select(p => new Entities.ServicosEntity.MultasProcedimentosEntity.MultasProcedimentosPortariaRelacionada
            {
                Ordem = p.Ordem,
                Titulo = p.Titulo.Trim(),
                Url = p.Url.Trim()
            }).ToList() ?? []
        };

        public static void ApplyUpdate(this Entities.ServicosEntity.MultasProcedimentosEntity.MultasProcedimentos e, MultasProcedimentosUpdateRequest r, DateTime nowUtc)
        {
            e.TituloPagina = r.TituloPagina.Trim();
            e.Slug = SlugHelper.Slugify(r.Slug);
            e.Ativo = r.Ativo;
            e.DataAtualizacao = nowUtc;
        }
    }

}
