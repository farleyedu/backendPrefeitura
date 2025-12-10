using PortalTCMSP.Domain.DTOs.Requests.SessaoPlenaria.Jurispudencia;
using PortalTCMSP.Domain.DTOs.Responses.SessaoPlenaria.Jurispudencia;
using PortalTCMSP.Domain.Entities.SessaoPlenariaEntity;
using PortalTCMSP.Domain.Helper;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.Mappings.SessaoPlenaria
{
    [ExcludeFromCodeCoverage]
    public static class SessaoPlenariaJurispudenciaMapper
    {
        // Entity -> Response
        public static SessaoPlenariaJurisprudenciaResponse ToResponse(this SessaoPlenariaJurispudencia e) => new()
        {
            Id = e.Id,
            Slug = e.Slug,
            Titulo = e.Titulo,
            Descricao = e.Descricao,

            TituloBoletins = e.TituloBoletins,
            DescricaoBoletins = e.DescricaoBoletins,
            LinkBoletins = e.LinkBoletins,

            TituloPesquisa = e.TituloPesquisa,
            DescricaoPesquisa = e.DescricaoPesquisa,
            LinkPesquisa = e.LinkPesquisa,

            TituloSumulas = e.TituloSumulas,
            DescricaoSumulas = e.DescricaoSumulas,
            LinkSumulas = e.LinkSumulas,

            Ativo = e.Ativo,
            DataCriacao = e.DataCriacao,
            DataAtualizacao = e.DataAtualizacao
        };

        public static IEnumerable<SessaoPlenariaJurisprudenciaResponse> ToResponse(this IEnumerable<SessaoPlenariaJurispudencia> list)
            => list.Select(ToResponse);

        // CreateRequest -> Entity
        public static SessaoPlenariaJurispudencia FromCreate(this SessaoPlenariaJurisprudenciaCreateRequest r, DateTime nowUtc) => new()
        {
            Slug = SlugHelper.Slugify(r.Slug),
            Titulo = r.Titulo?.Trim() ?? string.Empty,
            Descricao = r.Descricao?.Trim() ?? string.Empty,

            TituloBoletins = r.TituloBoletins?.Trim() ?? string.Empty,
            DescricaoBoletins = r.DescricaoBoletins?.Trim() ?? string.Empty,
            LinkBoletins = r.LinkBoletins?.Trim() ?? string.Empty,

            TituloPesquisa = r.TituloPesquisa?.Trim() ?? string.Empty,
            DescricaoPesquisa = r.DescricaoPesquisa?.Trim() ?? string.Empty,
            LinkPesquisa = r.LinkPesquisa?.Trim() ?? string.Empty,

            TituloSumulas = r.TituloSumulas?.Trim() ?? string.Empty,
            DescricaoSumulas = r.DescricaoSumulas?.Trim() ?? string.Empty,
            LinkSumulas = r.LinkSumulas?.Trim() ?? string.Empty,

            Ativo = r.Ativo,
            DataCriacao = nowUtc
        };

        // UpdateRequest -> apply
        public static void ApplyPartialUpdate(
        this SessaoPlenariaJurispudencia e,
        SessaoPlenariaJurisprudenciaUpdateRequest r,
        DateTime nowUtc)
        {
            if (!string.IsNullOrWhiteSpace(r.Slug))
                e.Slug = SlugHelper.Slugify(r.Slug);

            if (r.Titulo is not null)
                e.Titulo = r.Titulo?.Trim();

            if (r.Descricao is not null)
                e.Descricao = r.Descricao?.Trim();

            if (r.TituloBoletins is not null)
                e.TituloBoletins = r.TituloBoletins?.Trim();

            if (r.DescricaoBoletins is not null)
                e.DescricaoBoletins = r.DescricaoBoletins?.Trim();

            if (r.LinkBoletins is not null)
                e.LinkBoletins = r.LinkBoletins?.Trim();

            if (r.TituloPesquisa is not null)
                e.TituloPesquisa = r.TituloPesquisa?.Trim();

            if (r.DescricaoPesquisa is not null)
                e.DescricaoPesquisa = r.DescricaoPesquisa?.Trim();

            if (r.LinkPesquisa is not null)
                e.LinkPesquisa = r.LinkPesquisa?.Trim();

            if (r.TituloSumulas is not null)
                e.TituloSumulas = r.TituloSumulas?.Trim();

            if (r.DescricaoSumulas is not null)
                e.DescricaoSumulas = r.DescricaoSumulas?.Trim();

            if (r.LinkSumulas is not null)
                e.LinkSumulas = r.LinkSumulas?.Trim();

            if (r.Ativo.HasValue)
                e.Ativo = r.Ativo.Value;

            e.DataAtualizacao = nowUtc;
        }
    }
}
