using PortalTCMSP.Domain.DTOs.Requests.Servicos.EmissaoCertidoes;
using PortalTCMSP.Domain.DTOs.Responses.Servicos.EmissaoCertidoes;
using PortalTCMSP.Domain.Entities.ServicosEntity.EmissaoCertidoesEntity;
using PortalTCMSP.Domain.Helper;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.Mappings.Servicos.EmissaoCertidoes
{
    [ExcludeFromCodeCoverage]
    public static class EmissaoCertidoesMapper
    {
        public static EmissaoCertidoesResponse ToResponse(this Entities.ServicosEntity.EmissaoCertidoesEntity.EmissaoCertidoes e) => new()
        {
            Id = e.Id,
            TituloPagina = e.TituloPagina,
            Slug = e.Slug,
            Ativo = e.Ativo,
            DataCriacao = e.DataCriacao,
            DataAtualizacao = e.DataAtualizacao,
            Acoes = e.Acoes.OrderBy(a => a.Ordem).Select(a => new EmissaoCertidoesAcaoResponse
            {
                Id = a.Id,
                Ativo = a.Ativo,
                Ordem = a.Ordem,
                Titulo = a.Titulo,
                DataPublicacao = a.DataPublicacao,
                TipoAcao = a.TipoAcao,
                UrlAcao = a.UrlAcao
            }).ToList(),
            SecaoOrientacoes = e.SecaoOrientacoes.OrderBy(s => s.Id).Select(s => new EmissaoCertidoesSecaoOrientacaoResponse
            {
                Id = s.Id,
                TipoSecao = s.TipoSecao,
                TituloPagina = s.TituloPagina,
                Descricao = s.Descricao,
                Orientacoes = s.Orientacoes.OrderBy(o => o.Ordem).Select(o => new EmissaoCertidoesOrientacaoResponse
                {
                    Id = o.Id,
                    Ordem = o.Ordem,
                    Descritivos = o.Descritivos.OrderBy(d => d.Id).Select(d => d.Texto).ToList()
                }).ToList()
            }).ToList()
        };

        public static IEnumerable<EmissaoCertidoesResponse> ToResponse(this IEnumerable<Entities.ServicosEntity.EmissaoCertidoesEntity.EmissaoCertidoes> list) => list.Select(ToResponse);

        public static Entities.ServicosEntity.EmissaoCertidoesEntity.EmissaoCertidoes FromCreate(this EmissaoCertidoesCreateRequest r, DateTime nowUtc)
        {
            var e = new Entities.ServicosEntity.EmissaoCertidoesEntity.EmissaoCertidoes
            {
                TituloPagina = r.TituloPagina.Trim(),
                Slug = SlugHelper.Slugify(r.Slug),
                Ativo = r.Ativo,
                DataCriacao = nowUtc,
                //Acoes = r.Acoes?.Select(a => new EmissaoCertidoesAcao
                //{
                //    Ativo = a.Ativo,
                //    Ordem = a.Ordem,
                //    Titulo = a.Titulo.Trim(),
                //    DataPublicacao = a.DataPublicacao,
                //    TipoAcao = a.TipoAcao,
                //    UrlAcao = a.UrlAcao.Trim()
                //}).ToList() ?? [],

                //SecaoOrientacoes = r.SecaoOrientacoes?.Select(s => new EmissaoCertidoesSecaoOrientacao
                //{
                //    TipoSecao = s.TipoSecao,
                //    TituloPagina = s.TituloPagina.Trim(),
                //    Descricao = s.Descricao.Trim(),
                //    Orientacoes = s.Orientacoes?.Select(o => new EmissaoCertidoesOrientacao
                //    {
                //        Ordem = o.Ordem,
                //        Descritivos = o.Descritivos?.Select(d => new EmissaoCertidoesDescritivo { Texto = d?.Trim() ?? string.Empty }).ToList() ?? []
                //    }).ToList() ?? []
                //}).ToList() ?? []
            };

            foreach (var ac in e.Acoes) ac.IdEmissaoCertidoes = e.Id;
            foreach (var s in e.SecaoOrientacoes)
            {
                s.IdEmissaoCertidoes = e.Id;
                foreach (var o in s.Orientacoes)
                {
                    o.IdEmissaoCertidoesSecaoOrientacao = s.Id;
                    foreach (var d in o.Descritivos) d.IdEmissaoCertidoesOrientacao = o.Id;
                }
            }

            return e;
        }

        public static void ApplyUpdate(this Entities.ServicosEntity.EmissaoCertidoesEntity.EmissaoCertidoes e, EmissaoCertidoesUpdateRequest r, DateTime nowUtc)
        {
            e.TituloPagina = r.TituloPagina.Trim();
            e.Slug = SlugHelper.Slugify(r.Slug);
            e.Ativo = r.Ativo;
            e.DataAtualizacao = nowUtc;
        }
    }
}
