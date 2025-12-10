using PortalTCMSP.Domain.DTOs.Requests.Servicos.ProtocoloEletronico;
using PortalTCMSP.Domain.DTOs.Responses.Servicos.ProtocoloEletronico;
using PortalTCMSP.Domain.Helper;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.Mappings.Servicos.ProtocoloEletronico
{
    [ExcludeFromCodeCoverage]
    public static class ProtocoloEletronicoMapper
    {
        public static ProtocoloEletronicoResponse ToResponse(this Entities.ServicosEntity.ProtocoloEletronicoEntity.ProtocoloEletronico e) => new()
        {
            Id = e.Id,
            TituloPagina = e.TituloPagina,
            Slug = e.Slug,
            Ativo = e.Ativo,
            DataCriacao = e.DataCriacao,
            DataAtualizacao = e.DataAtualizacao,
            Acoes = [.. e.Acoes.OrderBy(a => a.Ordem).Select(a => new ProtocoloEletronicoAcaoResponse
        {
            Id = a.Id,
            Ativo = a.Ativo,
            Ordem = a.Ordem,
            Titulo = a.Titulo,
            DataPublicacao = a.DataPublicacao,
            TipoAcao = a.TipoAcao,
            UrlAcao = a.UrlAcao
        })]
        };

        public static IEnumerable<ProtocoloEletronicoResponse> ToResponse(this IEnumerable<Entities.ServicosEntity.ProtocoloEletronicoEntity.ProtocoloEletronico> list)
            => list.Select(ToResponse);

        public static Entities.ServicosEntity.ProtocoloEletronicoEntity.ProtocoloEletronico FromCreate(this ProtocoloEletronicoCreateRequest r, DateTime nowUtc) => new()
        {
            TituloPagina = r.TituloPagina.Trim(),
            Slug = SlugHelper.Slugify(r.Slug),
            Ativo = r.Ativo,
            DataCriacao = nowUtc,
            Acoes = r.Acoes?.Select(a => new Entities.ServicosEntity.ProtocoloEletronicoEntity.ProtocoloEletronicoAcao
            {
                Ativo = a.Ativo,
                Ordem = a.Ordem,
                Titulo = a.Titulo.Trim(),
                DataPublicacao = a.DataPublicacao,
                TipoAcao = a.TipoAcao,
                UrlAcao = a.UrlAcao.Trim()
            }).ToList() ?? []
        };

        public static void ApplyUpdate(this Entities.ServicosEntity.ProtocoloEletronicoEntity.ProtocoloEletronico e, ProtocoloEletronicoUpdateRequest r, DateTime nowUtc)
        {
            e.TituloPagina = r.TituloPagina.Trim();
            e.Slug = SlugHelper.Slugify(r.Slug);
            e.Ativo = r.Ativo;
            e.DataAtualizacao = nowUtc;
        }
    }

}
