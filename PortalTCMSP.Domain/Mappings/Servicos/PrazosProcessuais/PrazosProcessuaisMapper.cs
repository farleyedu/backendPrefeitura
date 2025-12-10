using PortalTCMSP.Domain.DTOs.Requests.Servicos.CartaServicosUsuario;
using PortalTCMSP.Domain.DTOs.Requests.Servicos.PrazosProcessuais;
using PortalTCMSP.Domain.DTOs.Responses.Servicos.PrazosProcessuais;
using PortalTCMSP.Domain.Entities.ServicosEntity.CartaServicosUsuarioEntity;
using PortalTCMSP.Domain.Entities.ServicosEntity.PrazosProcessuaisEntity;
using PortalTCMSP.Domain.Helper;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.Mappings.Servicos.PrazosProcessuais
{
    [ExcludeFromCodeCoverage]
    public static class PrazosProcessuaisMapper
    {
        public static PrazosProcessuaisResponse ToResponse(this Entities.ServicosEntity.PrazosProcessuaisEntity.PrazosProcessuais e) => new()
        {
            Id = e.Id,
            TituloPagina = e.TituloPagina,
            Slug = e.Slug,
            Ativo = e.Ativo,
            DataCriacao = e.DataCriacao,
            DataAtualizacao = e.DataAtualizacao,
            PrazosProcessuaisItens = e.PrazosProcessuaisItens.OrderBy(p => p.Ordem).Select(i => new PrazosProcessuaisItemResponse
            {
                Id = i.Id,
                Ativo = i.Ativo,
                Ordem = i.Ordem,
                Nome = i.Nome,
                DataPublicacao = i.DataPublicacao,
                TempoDecorrido = i.TempoDecorrido,
                Anexos = i.Anexos.OrderBy(a => a.Ordem).Select(a => new PrazosProcessuaisItemAnexoResponse
                {
                    Id = a.Id,
                    Ativo = a.Ativo,
                    Ordem = a.Ordem,
                    NomeArquivo = a.NomeArquivo,
                    Url = a.Url,
                    Tipo = a.Tipo
                }).ToList()
            }).ToList()
        };

        public static IEnumerable<PrazosProcessuaisResponse> ToResponse(this IEnumerable<Entities.ServicosEntity.PrazosProcessuaisEntity.PrazosProcessuais> list)
            => list.Select(ToResponse);

        public static Entities.ServicosEntity.PrazosProcessuaisEntity.PrazosProcessuais FromCreate(this PrazosProcessuaisCreateRequest r, DateTime nowUtc) => new()
        {
            TituloPagina = r.TituloPagina.Trim(),
            Slug = SlugHelper.Slugify(r.Slug),
            Ativo = r.Ativo,
            DataCriacao = nowUtc,
            //PrazosProcessuaisItens = r.PrazosProcessuaisItens?.Select(i => new Entities.ServicosEntity.PrazosProcessuaisEntity.PrazosProcessuaisItem
            //{
            //    Ativo = i.Ativo,
            //    Ordem = i.Ordem,
            //    Nome = i.Nome.Trim(),
            //    DataPublicacao = i.DataPublicacao,
            //    TempoDecorrido = i.TempoDecorrido.Trim(),
            //    Anexos = i.Anexos?.Select(a => new Entities.ServicosEntity.PrazosProcessuaisEntity.PrazosProcessuaisItemAnexo
            //    {
            //        Ativo = a.Ativo,
            //        Ordem = a.Ordem,
            //        NomeArquivo = a.NomeArquivo.Trim(),
            //        Url = a.Url.Trim(),
            //        Tipo = a.Tipo.Trim()
            //    }).ToList() ?? []
            //}).ToList() ?? []
        };

        public static void ApplyUpdate(this Entities.ServicosEntity.PrazosProcessuaisEntity.PrazosProcessuais e, PrazosProcessuaisUpdateRequest r, DateTime nowUtc)
        {
            e.TituloPagina = r.TituloPagina.Trim();
            e.Slug = SlugHelper.Slugify(r.Slug);
            e.Ativo = r.Ativo;
            e.DataAtualizacao = nowUtc;
        }

        public static PrazosProcessuaisItem ToEntity(this PrazosProcessuaisItemRequest request, long idPrazosProcessuais)
        {
            return new PrazosProcessuaisItem
            {
                IdPrazosProcessuais = idPrazosProcessuais,
                Ativo = request.Ativo,
                Ordem = request.Ordem,
                Nome = request.Nome,
                DataPublicacao = request.DataPublicacao,
                TempoDecorrido = request.TempoDecorrido,
            };
        }

        public static PrazosProcessuaisItemAnexo ToEntity(this PrazosProcessuaisItemAnexoRequest request, long idPrazosProcessuais)
        {
            return new PrazosProcessuaisItemAnexo
            {
                IdPrazosProcessuaisItem = idPrazosProcessuais,
                Ativo = request.Ativo,
                Ordem = request.Ordem,
                NomeArquivo = request.NomeArquivo,
                Url = request.Url,
                Tipo = request.Tipo
            };
        }
    }
}
