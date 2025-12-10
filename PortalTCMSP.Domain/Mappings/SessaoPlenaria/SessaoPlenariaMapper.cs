using PortalTCMSP.Domain.DTOs.Requests.SessaoPlenaria.Base;
using PortalTCMSP.Domain.DTOs.Responses.SessaoPlenaria.Base;
using PortalTCMSP.Domain.Helper;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.Mappings.SessaoPlenaria
{
    [ExcludeFromCodeCoverage]
    public static class SessaoPlenariaMapper
    {
        public static SessaoPlenariaResponse ToResponse(this Entities.SessaoPlenariaEntity.SessaoPlenaria e) => new()
        {
            Id = e.Id,
            Slug = e.Slug,
            Titulo = e.Titulo,
            Descricao = e.Descricao,
            SeoTitulo = e.SeoTitulo,
            SeoDescricao = e.SeoDescricao,
            UrlVideo = e.UrlVideo,
            DataCriacao = e.DataCriacao,
            DataAtualizacao = e.DataAtualizacao,
            DataPublicacao = e.DataPublicacao,
            Ativo = e.Ativo == "S",
            QtdePautas = e.Pautas?.Count ?? 0,
            QtdeAtas = e.Atas?.Count ?? 0,
            QtdeNotas = e.NotasTaquigraficas?.Count ?? 0
        };

        public static IEnumerable<SessaoPlenariaResponse> ToResponse(this IEnumerable<Entities.SessaoPlenariaEntity.SessaoPlenaria> list)
            => list.Select(ToResponse);

        public static Entities.SessaoPlenariaEntity.SessaoPlenaria FromCreate(this SessaoPlenariaCreateRequest r, DateTime nowUtc) => new()
        {
            Slug = SlugHelper.Slugify(r.Slug),
            Titulo = r.Titulo?.Trim() ?? string.Empty,
            Descricao = r.Descricao?.Trim(),
            SeoTitulo = r.SeoTitulo?.Trim(),
            SeoDescricao = r.SeoDescricao?.Trim(),
            UrlVideo = r.UrlVideo?.Trim(),
            DataCriacao = nowUtc,
            DataPublicacao = r.DataPublicacao,
            Ativo = r.Ativo ? "S" : "N"
        };

        public static void ApplyPartialUpdate(
        this Entities.SessaoPlenariaEntity.SessaoPlenaria e,
        SessaoPlenariaUpdateRequest r,
        DateTime nowUtc)
        {
            if (!string.IsNullOrWhiteSpace(r.Slug))
                e.Slug = SlugHelper.Slugify(r.Slug);

            if (r.Titulo is not null)      
                e.Titulo = r.Titulo?.Trim();

            if (r.Descricao is not null)
                e.Descricao = r.Descricao?.Trim();

            if (r.SeoTitulo is not null)
                e.SeoTitulo = r.SeoTitulo?.Trim();

            if (r.SeoDescricao is not null)
                e.SeoDescricao = r.SeoDescricao?.Trim();

            if (r.UrlVideo is not null)
                e.UrlVideo = r.UrlVideo?.Trim();

            if (r.DataPublicacao.HasValue)
                e.DataPublicacao = r.DataPublicacao;

            if (r.Ativo.HasValue)       
                e.Ativo = r.Ativo.Value ? "S" : "N";

            e.DataAtualizacao = nowUtc;
        }
    }
}
