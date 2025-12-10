using PortalTCMSP.Domain.DTOs.Requests.Fiscalizacao.RelatorioFiscalizacao;
using PortalTCMSP.Domain.DTOs.Responses.Fiscalizacao.RelatorioFiscalizacao;
using PortalTCMSP.Domain.Entities.FiscalizacaoEntity.RelatorioFiscalizacao;
using PortalTCMSP.Domain.Helper;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.Mappings.Fiscalizacao
{
    [ExcludeFromCodeCoverage]
    public static class FiscalizacaoRelatorioFiscalizacaoMapper
    {
        public static FiscalizacaoRelatorioFiscalizacaoResponse ToResponse(this FiscalizacaoRelatorioFiscalizacao e) => new()
        {
            Id = e.Id,
            Slug = e.Slug,
            Titulo = e.Titulo,
            Descricao = e.Descricao,
            Ativo = e.Ativo,
            DataCriacao = e.DataCriacao,
            DataAtualizacao = e.DataAtualizacao,
            Carrocel = e.Carrocel?.OrderBy(c => c.Ordem).Select(c => new FiscalizacaoRelatorioFiscalizacaoCarrosselResponse
            {
                Id = c.Id,
                Ativo = c.Ativo,
                Ordem = c.Ordem,
                Titulo = c.Titulo,
                ConteudoCarrocel = c.ConteudoCarrocel.OrderBy(cc => cc.Ordem).Select(cc => new FiscalizacaoRelatorioFiscalizacaoConteudoCarrosselResponse
                {
                    Id = cc.Id,
                    Ordem = cc.Ordem,
                    Ativo = cc.Ativo,
                    Descricao = cc.Descricao,
                    Link = cc.Link,
                    ConteudoLink = cc.ConteudoLink == null ? null : new FiscalizacaoRelatorioFiscalizacaoConteudoLinkResponse
                    {
                        Id = cc.ConteudoLink.Id,
                        Titulo = cc.ConteudoLink.Titulo,
                        DataPublicacao = cc.ConteudoLink.DataPublicacao,
                        ConselheiroRelator = cc.ConteudoLink.ConselheiroRelator,
                        ImagemUrl = cc.ConteudoLink.ImagemUrl,
                        PosicionamentoAEsquerda = cc.ConteudoLink.PosicionamentoAEsquerda,
                        Descritivo = cc.ConteudoLink.Descritivo,
                        PeriodoRealizacao = cc.ConteudoLink.PeriodoRealizacao,
                        PeriodoAbrangencia = cc.ConteudoLink.PeriodoAbrangencia,
                        TituloDestaque = cc.ConteudoLink.TituloDestaque,
                        ConteudoDestaque = cc.ConteudoLink.ConteudoDestaque.OrderBy(d => d.Id).Select(d => new FiscalizacaoRelatorioFiscalizacaoConteudoDestaqueResponse
                        {
                            Titulo = d.Titulo,
                            ImagemUrl = d.ImagemUrl,
                            Descricoes = d.Descricoes.OrderBy(x => x.Id).Select(x => new FiscalizacaoRelatorioFiscalizacaoDescricaoResponse 
                            { 
                                Descricao = x.Descricao 
                            }).ToList()
                        }).ToList(),
                        TcRelacionados = new FiscalizacaoRelatorioFiscalizacaoTcRelacionadosResponse
                        {
                            Tc = cc.ConteudoLink.TcRelacionados.OrderBy(t => t.Ordem).Select(t => new FiscalizacaoRelatorioFiscalizacaoTcResponse 
                            { 
                                Link = t.Link, 
                                Descritivo = t.Descritivo, 
                                Ordem = t.Ordem 
                            }).ToList()
                        },
                        Anexos = new FiscalizacaoRelatorioFiscalizacaoAnexosResponse
                        {
                            DocumentosAnexos = cc.ConteudoLink.DocumentosAnexos.OrderBy(a => a.Ordem).Select(a => new FiscalizacaoRelatorioFiscalizacaoDocumentoAnexoResponse { Id = a.Id, Link = a.Link, TipoArquivo = a.TipoArquivo, NomeExibicao = a.NomeExibicao, Ordem = a.Ordem }).ToList(),
                            ImagensAnexas = cc.ConteudoLink.ImagensAnexas.OrderBy(a => a.Ordem).Select(a => new FiscalizacaoRelatorioFiscalizacaoImagemAnexaResponse { Id = a.Id, ImagemUrl = a.ImagemUrl, NomeExibicao = a.NomeExibicao, Ordem = a.Ordem }).ToList()
                        }
                    }
                }).ToList()
            }).ToList() ?? new List<FiscalizacaoRelatorioFiscalizacaoCarrosselResponse>()
        };

        public static IEnumerable<FiscalizacaoRelatorioFiscalizacaoResponse> ToResponse(this IEnumerable<FiscalizacaoRelatorioFiscalizacao> list) => list.Select(ToResponse);

        public static FiscalizacaoRelatorioFiscalizacao FromCreate(this FiscalizacaoRelatorioFiscalizacaoCreateRequest r, DateTime nowUtc) => new()
        {
            Slug = SlugHelper.Slugify(r.Slug),
            Titulo = r.Titulo?.Trim() ?? string.Empty,
            Descricao = r.Descricao?.Trim(),
            Ativo = r.Ativo,
            DataCriacao = nowUtc,
            Carrocel = r.Carrocel?.Select(c => new FiscalizacaoRelatorioFiscalizacaoCarrossel
            {
                Ativo = c.Ativo,
                Ordem = c.Ordem,
                Titulo = c.Titulo?.Trim(),
                ConteudoCarrocel = c.ConteudoCarrocel?.Select(cc => new FiscalizacaoRelatorioFiscalizacaoConteudoCarrocel
                {
                    Ordem = cc.Ordem,
                    Ativo = cc.Ativo,
                    Descricao = cc.Descricao?.Trim(),
                    Link = cc.Link?.Trim(),
                    ConteudoLink = cc.ConteudoLink == null ? null : new FiscalizacaoRelatorioFiscalizacaoConteudoLink
                    {
                        Titulo = cc.ConteudoLink.Titulo?.Trim(),
                        DataPublicacao = cc.ConteudoLink.DataPublicacao,
                        ConselheiroRelator = cc.ConteudoLink.ConselheiroRelator?.Trim(),
                        ImagemUrl = cc.ConteudoLink.ImagemUrl?.Trim(),
                        PosicionamentoAEsquerda = cc.ConteudoLink.PosicionamentoAEsquerda,
                        Descritivo = cc.ConteudoLink.Descritivo?.Trim(),
                        PeriodoRealizacao = cc.ConteudoLink.PeriodoRealizacao?.Trim(),
                        PeriodoAbrangencia = cc.ConteudoLink.PeriodoAbrangencia?.Trim(),
                        TituloDestaque = cc.ConteudoLink.TituloDestaque?.Trim(),
                        ConteudoDestaque = cc.ConteudoLink.ConteudoDestaque?.Select(d => new FiscalizacaoRelatorioFiscalizacaoConteudoDestaque
                        {
                            Titulo = d.Titulo?.Trim(),
                            ImagemUrl = d.ImagemUrl?.Trim(),
                            Descricoes = d.Descricoes?.Select(x => new FiscalizacaoRelatorioFiscalizacaoDescricao { Descricao = x.Descricao?.Trim() }).ToList() ?? []
                        }).ToList() ?? [],
                        TcRelacionados = cc.ConteudoLink.TcRelacionados?.Tc?.Select(t => new FiscalizacaoRelatorioFiscalizacaoTcRelacionado
                        {
                            Link = t.Link?.Trim(),
                            Descritivo = t.Descritivo?.Trim(),
                            Ordem = t.Ordem
                        }).ToList() ?? [],
                        DocumentosAnexos = cc.ConteudoLink.Anexos?.DocumentosAnexos?.Select(a => new FiscalizacaoRelatorioFiscalizacaoDocumentoAnexo
                        {
                            Link = a.Link?.Trim(),
                            TipoArquivo = a.TipoArquivo?.Trim(),
                            NomeExibicao = a.NomeExibicao?.Trim(),
                            Ordem = a.Ordem
                        }).ToList() ?? [],
                        ImagensAnexas = cc.ConteudoLink.Anexos?.ImagensAnexas?.Select(a => new FiscalizacaoRelatorioFiscalizacaoImagemAnexa
                        {
                            ImagemUrl = a.ImagemUrl?.Trim(),
                            NomeExibicao = a.NomeExibicao?.Trim(),
                            Ordem = a.Ordem
                        }).ToList() ?? []
                    }
                }).ToList() ?? []
            }).ToList() ?? []
        };

        public static void ApplyUpdate(this FiscalizacaoRelatorioFiscalizacao e, FiscalizacaoRelatorioFiscalizacaoConteudoUpdateRequest r, DateTime nowUtc)
        {
            e.Slug = SlugHelper.Slugify(r.Slug);
            e.Titulo = r.Titulo?.Trim() ?? string.Empty;
            e.Descricao = r.Descricao?.Trim();
            e.Ativo = r.Ativo;
            e.DataAtualizacao = nowUtc;
        }
    }
}
