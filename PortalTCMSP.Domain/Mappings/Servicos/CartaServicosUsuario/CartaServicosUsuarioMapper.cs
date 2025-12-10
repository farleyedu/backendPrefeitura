using PortalTCMSP.Domain.DTOs.Requests.Servicos.CartaServicosUsuario;
using PortalTCMSP.Domain.DTOs.Requests.Servicos.CartaServicosUsuario.Application.DTO.Servicos.CartaServicosUsuarioDto;
using PortalTCMSP.Domain.DTOs.Responses.Servicos.CartaServicosUsuario;
using PortalTCMSP.Domain.DTOs.Responses.Servicos.CartaServicosUsuario.Application.DTO.Servicos.CartaServicosUsuarioDto;
using PortalTCMSP.Domain.Entities.ServicosEntity.CartaServicosUsuarioEntity;
using PortalTCMSP.Domain.Helper;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.Mappings.Servicos.CartaServicosUsuario
{
    [ExcludeFromCodeCoverage]
    public static class CartaServicosUsuarioMapper
    {
        public static CartaServicosUsuarioResponse ToResponse(this Entities.ServicosEntity.CartaServicosUsuarioEntity.CartaServicosUsuario e) => new()
        {
            Id = e.Id,
            TituloPagina = e.TituloPagina,
            TituloPesquisa = e.TituloPesquisa,
            Slug = e.Slug,
            Ativo = e.Ativo,
            Servicos = e.Servicos?
                .OrderBy(s => s.Ordem)
                .Select(s => new CartaServicosUsuarioServicoResponse
                {
                    Id = s.Id,
                    Ativo = s.Ativo,
                    Ordem = s.Ordem,
                    Titulo = s.Titulo,
                    ServicosItens = s.ServicosItens?
                        .OrderBy(i => i.Ordem)
                        .Select(i => new CartaServicosUsuarioServicoItemResponse
                        {
                            Id = i.Id,
                            Ativo = i.Ativo,
                            Ordem = i.Ordem,
                            Titulo = i.Titulo,
                            Acao = i.Acao ?? string.Empty,
                            LinkItem = i.LinkItem ?? string.Empty,
                            ItemDetalhe = i.ItemDetalhe?
                                .OrderBy(d => d.Ordem)
                                .Select(d => new CartaServicosUsuarioItemDetalheResponse
                                {
                                    Id = d.Id,
                                    Ativo = d.Ativo,
                                    Ordem = d.Ordem,
                                    TituloDetalhe = d.TituloDetalhe,
                                    DescritivoItemDetalhe = d.DescritivoItemDetalhe?
                                        .OrderBy(x => x.Ordem)
                                        .Select(x => new CartaServicosUsuarioDescritivoItemDetalheResponse
                                        {
                                            Id = x.Id,
                                            Ordem = x.Ordem,
                                            Descritivo = x.Descritivo
                                        }).ToList() ?? []
                                }).ToList() ?? []
                        }).ToList() ?? []
                }).ToList() ?? []
        };

        public static IEnumerable<CartaServicosUsuarioResponse> ToResponse(this IEnumerable<Entities.ServicosEntity.CartaServicosUsuarioEntity.CartaServicosUsuario> list)
            => list.Select(ToResponse);

        public static Entities.ServicosEntity.CartaServicosUsuarioEntity.CartaServicosUsuario FromCreate(this CartaServicosUsuarioRequest r, DateTime nowUtc) => new()
        {
            TituloPagina = r.TituloPagina?.Trim() ?? string.Empty,
            TituloPesquisa = r.TituloPesquisa?.Trim() ?? string.Empty,
            Slug = SlugHelper.Slugify(r.Slug ?? string.Empty),
            Ativo = r.Ativo,
            DataCriacao = nowUtc,
            //Servicos = r.Servicos?.Select(s => new CartaServicosUsuarioServico
            //{
            //    Ativo = s.Ativo,
            //    Ordem = s.Ordem,
            //    Titulo = s.Titulo?.Trim() ?? string.Empty,
            //    ServicosItens = s.ServicosItens?.Select(i => new CartaServicosUsuarioServicoItem
            //    {
            //        Ativo = i.Ativo,
            //        Ordem = i.Ordem,
            //        Titulo = i.Titulo?.Trim() ?? string.Empty,
            //        Acao = i.Acao?.Trim() ?? string.Empty,
            //        LinkItem = i.LinkItem?.Trim() ?? string.Empty,
            //        ItemDetalhe = i.ItemDetalhe?.Select(d => new CartaServicosUsuarioItemDetalhe
            //        {
            //            Ativo = d.Ativo,
            //            Ordem = d.Ordem,
            //            TituloDetalhe = d.TituloDetalhe?.Trim() ?? string.Empty,
            //            DescritivoItemDetalhe = d.DescritivoItemDetalhe?.Select(x => new CartaServicosUsuarioDescritivoItemDetalhe
            //            {
            //                Ordem = x.Ordem,
            //                Descritivo = x.Descritivo?.Trim() ?? string.Empty
            //            }).ToList() ?? []
            //        }).ToList() ?? []
            //    }).ToList() ?? []
            //}).ToList() ?? []
        };

        public static void ApplyUpdate(this Entities.ServicosEntity.CartaServicosUsuarioEntity.CartaServicosUsuario e, CartaServicosUsuarioRequest r, DateTime nowUtc)
        {
            e.TituloPagina = r.TituloPagina?.Trim() ?? string.Empty;
            e.TituloPesquisa = r.TituloPesquisa?.Trim() ?? string.Empty;
            e.Slug = SlugHelper.Slugify(r.Slug ?? string.Empty);
            e.Ativo = r.Ativo;
            e.DataAtualizacao = nowUtc;
        }

        public static Entities.ServicosEntity.CartaServicosUsuarioEntity.CartaServicosUsuario ToEntity(this CartaServicosUsuarioRequest request, DateTime dataCriacao)
        {
            return new Entities.ServicosEntity.CartaServicosUsuarioEntity.CartaServicosUsuario
            {
                TituloPagina = request.TituloPagina.Trim(),
                TituloPesquisa = request.TituloPesquisa.Trim(),
                Slug = request.Slug.Trim(),
                Ativo = request.Ativo,
                DataCriacao = dataCriacao
                //Servicos = request.Servicos?.Select(s => s.ToEntity(0)).ToList() ?? []
            };
        }

        public static CartaServicosUsuarioServico ToEntity(this CartaServicosUsuarioServicoRequest request, long idCartaServicosUsuario)
        {
            return new CartaServicosUsuarioServico
            {
                IdCartaServicosUsuario = idCartaServicosUsuario,
                Ativo = request.Ativo,
                Ordem = request.Ordem,
                Titulo = request.Titulo?.Trim() ?? string.Empty,
                ServicosItens = request.ServicosItens?.Select(i => i.ToEntity(0)).ToList() ?? []
            };
        }

        public static CartaServicosUsuarioServicoItem ToEntity(this CartaServicosUsuarioServicoItemRequest request, long idCartaServicosUsuarioServico)
        {
            return new CartaServicosUsuarioServicoItem
            {
                Ativo = request.Ativo,
                Ordem = request.Ordem,
                Titulo = request.Titulo?.Trim() ?? string.Empty,
                Acao = request.Acao?.Trim() ?? string.Empty,
                LinkItem = request.LinkItem?.Trim() ?? string.Empty
            };
        }

        public static CartaServicosUsuarioItemDetalhe ToEntity(this CartaServicosUsuarioItemDetalheRequest request, long idCartaServicosUsuarioServicoItem)
        {
            return new CartaServicosUsuarioItemDetalhe
            {
                IdCartaServicosUsuarioServicoItem = idCartaServicosUsuarioServicoItem,
                Ordem = request.Ordem,
                Ativo = request.Ativo,
                TituloDetalhe = request.TituloDetalhe//,
                //DescritivoItemDetalhe = request.DescritivoItemDetalhe?.Select(d => d.ToEntity(0)).ToList() ?? []
            };
        }

        public static CartaServicosUsuarioDescritivoItemDetalhe ToEntity(this CartaServicosUsuarioDescritivoItemDetalheRequest request, long idCartaServicosUsuarioItemDetalhe)
        {
            return new CartaServicosUsuarioDescritivoItemDetalhe
            {
                IdCartaServicosUsuarioItemDetalhe = idCartaServicosUsuarioItemDetalhe,
                Ordem = request.Ordem,
                Descritivo = request.Descritivo.Trim()
            };
        }
    }
}
