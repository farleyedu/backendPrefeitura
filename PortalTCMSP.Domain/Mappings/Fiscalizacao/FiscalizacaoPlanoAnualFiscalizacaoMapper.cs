using PortalTCMSP.Domain.DTOs.Requests.Fiscalizacao.PlanoAnualFiscalizacao;
using PortalTCMSP.Domain.DTOs.Responses.Fiscalizacao.PlanoAnualFiscalizacao;
using PortalTCMSP.Domain.Entities.FiscalizacaoEntity.PlanoAnualFiscalizacao;
using PortalTCMSP.Domain.Helper;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.Mappings.Fiscalizacao
{
    [ExcludeFromCodeCoverage]
    public static class FiscalizacaoPlanoAnualFiscalizacaoMapper
    {
        public static FiscalizacaoPlanoAnualFiscalizacaoResolucaoResponse ToResponse(this FiscalizacaoPlanoAnualFiscalizacaoResolucao e) => new()
        {
            Id = e.Id,
            Slug = e.Slug,
            Numero = e.Numero,
            Ano = e.Ano,
            Ativo = e.Ativo,
            DataPublicacao = e.DataPublicacao,
            Titulo = e.Titulo,
            SubTitulo = e.SubTitulo,
            Ementa = e.Ementa == null ? null : new FiscalizacaoResolucaoEmentaResponse
            {
                Id = e.Ementa.Id,
                Descritivo = e.Ementa.Descritivo,
                LinksArtigos = e.Ementa.LinksArtigos.Select(l => l.Link).ToList()
            },
            Dispositivos = e.Dispositivos.OrderBy(d => d.Ordem).Select(d => new FiscalizacaoResolucaoDispositivoResponse
            {
                Id = d.Id,
                Ordem = d.Ordem,
                Artigo = d.Artigo,
                Paragrafo = d.Paragrafo.OrderBy(p => p.Ordem).Select(p => new FiscalizacaoResolucaoParagrafoResponse
                {
                    Id = p.Id,
                    Ordem = p.Ordem,
                    Texto = p.Texto
                }).ToList(),
                Incisos = d.Incisos.OrderBy(i => i.Ordem).Select(i => new FiscalizacaoResolucaoIncisoResponse
                {
                    Id = i.Id,
                    Ordem = i.Ordem,
                    Texto = i.Texto
                }).ToList()
            }).ToList(),
            Anexos = e.Anexos.OrderBy(a => a.Numero).Select(a => new FiscalizacaoResolucaoAnexoResponse
            {
                Id = a.Id,
                Numero = a.Numero,
                Titulo = a.Titulo,
                Descritivo = a.Descritivo,
                TemasPrioritarios = a.TemasPrioritarios.OrderBy(t => t.Ordem).Select(t => new FiscalizacaoResolucaoTemaPrioritarioResponse
                {
                    Id = t.Id,
                    Ordem = t.Ordem,
                    Tema = t.Tema,
                    Descricao = t.Descricao
                }).ToList(),
                Atividades = a.Atividades.Select(at => new FiscalizacaoResolucaoAtividadeResponse
                {
                    Id = at.Id,
                    Tipo = at.Tipo,
                    AtividadeItem = at.AtividadeItem.Select(ai => new FiscalizacaoResolucaoAtividadeItemResponse
                    {
                        Descricao = ai.Descricao,
                        Quantidade = ai.Quantidade,
                        DUSFs = ai.DUSFs
                    }).ToList()
                }).ToList(),
                Distribuicao = a.Distribuicao.Select(d => new FiscalizacaoResolucaoDistribuicaoResponse
                {
                    Id = d.Id,
                    TipoFiscalizacao = d.TipoFiscalizacao,
                    TotalPAF = d.TotalPAF,
                    DezPorCentoTotalPAF = d.DezPorCentoTotalPAF,
                    LimitePorConselheiro = d.LimitePorConselheiro,
                    LimiteConselheiros = d.LimiteConselheiros,
                    LimitePlenoeCameras = d.LimitePlenoeCameras,
                    LimitePresidente = d.LimitePresidente,
                    ListaDePrioridades = d.ListaDePrioridades
                }).ToList()
            }).ToList(),
            Atas = e.Atas.OrderBy(a => a.Ordem).Select(a => new FiscalizacaoResolucaoAtaResponse
            {
                Id = a.Id,
                Ordem = a.Ordem,
                TituloAta = a.TituloAta,
                TituloAtaAEsquerda = a.TituloAtaAEsquerda,
                ConteudoAta = a.ConteudoAta
            }).ToList()
        };

        public static IEnumerable<FiscalizacaoPlanoAnualFiscalizacaoResolucaoResponse> ToResponse(this IEnumerable<FiscalizacaoPlanoAnualFiscalizacaoResolucao> list)
        => list.Select(ToResponse);

        public static FiscalizacaoPlanoAnualFiscalizacaoResolucao FromCreate(this FiscalizacaoPlanoAnualFiscalizacaoResolucaoCreateRequest r, DateTime nowUtc)
        {
            var e = new FiscalizacaoPlanoAnualFiscalizacaoResolucao
            {
                Slug = SlugHelper.Slugify(r.Slug),
                Numero = r.Numero,
                Ano = r.Ano,
                DataPublicacao = r.DataPublicacao,
                Titulo = r.Titulo?.Trim() ?? string.Empty,
                SubTitulo = r.SubTitulo?.Trim(),
                Ativo = r.Ativo,
                Dispositivos = r.Dispositivos?.Select(d => new FiscalizacaoResolucaoDispositivo
                {
                    Ordem = d.Ordem,
                    Artigo = d.Artigo?.Trim() ?? string.Empty,
                    Paragrafo = d.Paragrafo?.Select(p => new FiscalizacaoResolucaoParagrafo
                    {
                        Ordem = p.Ordem,
                        Texto = p.Texto?.Trim() ?? string.Empty
                    }).ToList() ?? [],
                    Incisos = d.Incisos?.Select(i => new FiscalizacaoResolucaoInciso
                    {
                        Ordem = i.Ordem,
                        Texto = i.Texto?.Trim() ?? string.Empty
                    }).ToList() ?? []
                }).ToList() ?? [],
                Anexos = r.Anexos?.Select(a => new FiscalizacaoResolucaoAnexo
                {
                    Numero = a.Numero,
                    Titulo = a.Titulo?.Trim() ?? string.Empty,
                    Descritivo = a.Descritivo?.Trim(),
                    TemasPrioritarios = a.TemasPrioritarios?.Select(t => new FiscalizacaoResolucaoTemaPrioritario
                    {
                        Ordem = t.Ordem,
                        Tema = t.Tema?.Trim() ?? string.Empty,
                        Descricao = t.Descricao?.Trim()
                    }).ToList() ?? [],
                    Atividades = a.Atividades?.Select(at => new FiscalizacaoResolucaoAtividade
                    {
                        Tipo = at.Tipo?.Trim() ?? string.Empty,
                        AtividadeItem = at.AtividadeItem?.Select(ai => new FiscalizacaoResolucaoAtividadeItem
                        {
                            Descricao = ai.Descricao?.Trim() ?? string.Empty,
                            Quantidade = ai.Quantidade,
                            DUSFs = ai.DUSFs
                        }).ToList() ?? []
                    }).ToList() ?? [],
                    Distribuicao = a.Distribuicao?.Select(d => new FiscalizacaoResolucaoDistribuicao
                    {
                        TipoFiscalizacao = d.TipoFiscalizacao,
                        TotalPAF = d.TotalPAF,
                        DezPorCentoTotalPAF = d.DezPorCentoTotalPAF,
                        LimitePorConselheiro = d.LimitePorConselheiro,
                        LimiteConselheiros = d.LimiteConselheiros,
                        LimitePlenoeCameras = d.LimitePlenoeCameras,
                        LimitePresidente = d.LimitePresidente,
                        ListaDePrioridades = d.ListaDePrioridades
                    }).ToList() ?? []
                }).ToList() ?? [],
                Atas = r.Atas?.Select(a => new FiscalizacaoResolucaoAta
                {
                    Ordem = a.Ordem,
                    TituloAta = a.TituloAta?.Trim() ?? string.Empty,
                    TituloAtaAEsquerda = a.TituloAtaAEsquerda,
                    ConteudoAta = a.ConteudoAta?.Trim() ?? string.Empty
                }).ToList() ?? []
            };

            if (r.Ementa != null)
            {
                var em = new FiscalizacaoResolucaoEmenta
                {
                    Descritivo = r.Ementa.Descritivo?.Trim() ?? string.Empty,
                    LinksArtigos = r.Ementa.LinksArtigos?.Select(l => new FiscalizacaoResolucaoEmentaLink { Link = l?.Trim() ?? string.Empty }).ToList() ?? []
                };
                e.Ementa = em;
            }

            return e;
        }

        public static void ApplyUpdate(this FiscalizacaoPlanoAnualFiscalizacaoResolucao e, FiscalizacaoPlanoAnualFiscalizacaoResolucaoUpdateRequest r, DateTime nowUtc)
        {
            e.Slug = SlugHelper.Slugify(r.Slug);
            e.Numero = r.Numero;
            e.Ano = r.Ano;
            e.DataPublicacao = r.DataPublicacao;
            e.Titulo = r.Titulo?.Trim() ?? string.Empty;
            e.SubTitulo = r.SubTitulo?.Trim();
        }
    }
}
