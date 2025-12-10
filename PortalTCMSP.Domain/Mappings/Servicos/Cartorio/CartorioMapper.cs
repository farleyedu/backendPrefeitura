using PortalTCMSP.Domain.DTOs.Requests.Servicos.Cartorio;
using PortalTCMSP.Domain.DTOs.Responses.Servicos.Cartorio;
using PortalTCMSP.Domain.Helper;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.Mappings.Servicos.Cartorio
{
    [ExcludeFromCodeCoverage]
    public static class CartorioMapper
    {
        public static CartorioResponse ToResponse(this Entities.ServicosEntity.CartorioEntity.Cartorio e) => new()
        {
            Id = e.Id,
            TituloPagina = e.TituloPagina,
            Slug = e.Slug,
            Ativo = e.Ativo,
            DataCriacao = e.DataCriacao,
            DataAtualizacao = e.DataAtualizacao,
            Atendimentos = [.. e.Atendimentos.OrderBy(a => a.Ordem).Select(a => new CartorioAtendimentoResponse
        {
            Id = a.Id,
            Ordem = a.Ordem,
            Titulo = a.Titulo,
            Descricao = a.Descricao
        })]
        };

        public static IEnumerable<CartorioResponse> ToResponse(this IEnumerable<Entities.ServicosEntity.CartorioEntity.Cartorio> list)
            => list.Select(ToResponse);

        public static Entities.ServicosEntity.CartorioEntity.Cartorio FromCreate(this CartorioCreateRequest r, DateTime nowUtc) => new()
        {
            TituloPagina = r.TituloPagina.Trim(),
            Slug = SlugHelper.Slugify(r.Slug),
            Ativo = r.Ativo,
            DataCriacao = nowUtc,
            Atendimentos = r.Atendimentos?.Select(a => new Entities.ServicosEntity.CartorioEntity.CartorioAtendimento
            {
                Ordem = a.Ordem,
                Titulo = a.Titulo.Trim(),
                Descricao = a.Descricao.Trim()
            }).ToList() ?? []
        };

        public static void ApplyUpdate(this Entities.ServicosEntity.CartorioEntity.Cartorio e, CartorioUpdateRequest r, DateTime nowUtc)
        {
            e.TituloPagina = r.TituloPagina.Trim();
            e.Slug = SlugHelper.Slugify(r.Slug);
            e.Ativo = r.Ativo;
            e.DataAtualizacao = nowUtc;
        }
    }

}
