using PortalTCMSP.Domain.DTOs.Requests.Servicos.OficioseIntimacoes;
using PortalTCMSP.Domain.DTOs.Responses.Servicos.OficioseIntimacoes;
using PortalTCMSP.Domain.Helper;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.Mappings.Servicos.OficioseIntimacoes
{
    [ExcludeFromCodeCoverage]
    public static class OficioseIntimacoesMapper
    {
        public static OficioseIntimacoesResponse ToResponse(this Entities.ServicosEntity.OficioseIntimacoesEntity.OficioseIntimacoes e) => new()
        {
            Id = e.Id,
            Titulo = e.Titulo,
            Slug = e.Slug,
            Ativo = e.Ativo,
            DataCriacao = e.DataCriacao,
            DataAtualizacao = e.DataAtualizacao,
            Secoes = e.Secoes.OrderBy(s => s.Ordem).Select(s => new OficioseIntimacoesSecaoResponse
            {
                Id = s.Id,
                Ordem = s.Ordem,
                Nome = s.Nome,
                SecaoItem = s.SecaoItem.OrderBy(i => i.Ordem)
                    .Select(i => new OficioseIntimacoesSecaoItemResponse
                    {
                        Id = i.Id,
                        Ordem = i.Ordem,
                        Descricao = i.Descricao
                    }).ToList()
            }).ToList()
        };

        public static IEnumerable<OficioseIntimacoesResponse> ToResponse(this IEnumerable<Entities.ServicosEntity.OficioseIntimacoesEntity.OficioseIntimacoes> list)
            => list.Select(ToResponse);

        public static Entities.ServicosEntity.OficioseIntimacoesEntity.OficioseIntimacoes FromCreate(this OficioseIntimacoesCreateRequest r, DateTime nowUtc) => new()
        {
            Titulo = r.Titulo.Trim(),
            Slug = SlugHelper.Slugify(r.Slug),
            Ativo = r.Ativo,
            DataCriacao = nowUtc,
            Secoes = r.Secoes?.Select(s => new Entities.ServicosEntity.OficioseIntimacoesEntity.OficioseIntimacoesSecao
            {
                Ordem = s.Ordem,
                Nome = s.Nome.Trim(),
                SecaoItem = s.SecaoItem?.Select(i => new Entities.ServicosEntity.OficioseIntimacoesEntity.OficioseIntimacoesSecaoItem
                {
                    Ordem = i.Ordem,
                    Descricao = i.Descricao.Trim()
                }).ToList() ?? []
            }).ToList() ?? []
        };

        public static void ApplyUpdate(this Entities.ServicosEntity.OficioseIntimacoesEntity.OficioseIntimacoes e, OficioseIntimacoesUpdateRequest r, DateTime nowUtc)
        {
            e.Titulo = r.Titulo.Trim();
            e.Slug = SlugHelper.Slugify(r.Slug);
            e.Ativo = r.Ativo;
            e.DataAtualizacao = nowUtc;
        }
    }
}
