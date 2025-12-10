using PortalTCMSP.Domain.DTOs.Requests.Fiscalizacao.SecretariaControleExterno;
using PortalTCMSP.Domain.DTOs.Responses.Fiscalizacao.SecretariaControleExterno;
using PortalTCMSP.Domain.Entities.FiscalizacaoEntity.SecretariaControleExterno;
using PortalTCMSP.Domain.Helper;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.Mappings.Fiscalizacao
{
    [ExcludeFromCodeCoverage]
    public static class FiscalizacaoSecretariaControleExternoMapper
    {
        // Entity -> Response
        public static FiscalizacaoSecretariaResponse ToResponse(this FiscalizacaoSecretariaControleExterno e) => new()
        {
            Id = e.Id,
            Slug = e.Slug,
            Titulo = e.Titulo,
            Descricao = e.Descricao,
            Creditos = e.Creditos,
            Ativo = e.Ativo,
            DataCriacao = e.DataCriacao,
            DataAtualizacao = e.DataAtualizacao,
            Titulos = [.. e.Titulos.OrderBy(t => t.Ordem).Select(t => new FiscalizacaoSecretariaTituloItemResponse
            {
                Id = t.Id,
                Ordem = t.Ordem,
                Titulo = t.Titulo,
                ImagemUrl = t.ImagemUrl,
                Descricao = t.Descricao
            })],
            Carrossel = [.. e.Carrossel.OrderBy(c => c.Ordem).Select(c => new FiscalizacaoSecretariaCarrosselItemResponse
            {
                Id = c.Id,
                Ordem = c.Ordem,
                Titulo = c.Titulo,
                Descricao = c.Descricao,
                ImagemUrl = c.ImagemUrl
            })]
        };

        public static IEnumerable<FiscalizacaoSecretariaResponse> ToResponse(this IEnumerable<FiscalizacaoSecretariaControleExterno> list)
            => list.Select(ToResponse);

        // Create -> Entity
        public static FiscalizacaoSecretariaControleExterno FromCreate(this FiscalizacaoSecretariaCreateRequest r, DateTime nowUtc) => new()
        {
            Slug = SlugHelper.Slugify(r.Slug),
            Titulo = r.Titulo?.Trim() ?? string.Empty,
            Descricao = r.Descricao?.Trim(),
            Creditos = r.Creditos?.Trim(),
            Ativo = r.Ativo,
            DataCriacao = nowUtc,
            Titulos = r.Titulos?.Select(t => new FiscalizacaoSecretariaSecaoConteudoTitulo
            {
                Ordem = t.Ordem,
                Titulo = t.Titulo?.Trim() ?? string.Empty,
                Descricao = t.Descricao?.Trim(),
                ImagemUrl = t.ImagemUrl?.Trim() ?? string.Empty,
            }).ToList() ?? [],
            Carrossel = r.Carrossel?.Select(c => new FiscalizacaoSecretariaSecaoConteudoCarrosselItem
            {
                Ordem = c.Ordem,
                Titulo = c.Titulo?.Trim() ?? string.Empty,
                Descricao = c.Descricao?.Trim(),
                ImagemUrl = c.ImagemUrl?.Trim() ?? string.Empty
            }).ToList() ?? []
        };

        // Update -> apply (listas são substituídas via repository)
        public static void ApplyUpdate(this FiscalizacaoSecretariaControleExterno e, FiscalizacaoSecretariaUpdateRequest r, DateTime nowUtc)
        {
            e.Slug = SlugHelper.Slugify(r.Slug);
            e.Titulo = r.Titulo?.Trim() ?? string.Empty;
            e.Descricao = r.Descricao?.Trim();
            e.Creditos = r.Creditos?.Trim();
            e.Ativo = r.Ativo;
            e.DataAtualizacao = nowUtc;
        }
    }
}
