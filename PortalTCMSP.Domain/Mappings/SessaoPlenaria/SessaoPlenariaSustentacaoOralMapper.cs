using PortalTCMSP.Domain.DTOs.Requests.SessaoPlenaria.SustentacaoOral;
using PortalTCMSP.Domain.DTOs.Responses.SessaoPlenaria.SustentacaoOral;
using PortalTCMSP.Domain.Entities.SessaoPlenariaEntity;
using PortalTCMSP.Domain.Helper;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.Mappings.SessaoPlenaria
{
    [ExcludeFromCodeCoverage]
    public static class SessaoPlenariaSustentacaoOralMapper
    {
        // Entity -> Response
        public static SessaoPlenariaSustentacaoOralResponse ToResponse(this SessaoPlenariaSustentacaoOral e) => new()
        {
            Id = e.Id,
            Slug = e.Slug,
            Titulo = e.Titulo,
            Descricao = e.Descricao,
            Ativo = e.Ativo,
            DataCriacao = e.DataCriacao,
            DataAtualizacao = e.DataAtualizacao,
            Anexos = e.Anexos
                .OrderBy(a => a.Ordem, StringComparer.OrdinalIgnoreCase)
                .Select(a => new SessaoPlenariaSustentacaoOralAnexoItemResponse
                {
                    Id = a.Id,
                    Ordem = a.Ordem,
                    Titulo = a.Titulo,
                    Descricao = a.Descricao
                }).ToList()
        };

        public static IEnumerable<SessaoPlenariaSustentacaoOralResponse> ToResponse(this IEnumerable<SessaoPlenariaSustentacaoOral> list)
            => list.Select(ToResponse);

        // Create -> Entity
        public static SessaoPlenariaSustentacaoOral FromCreate(this SessaoPlenariaSustentacaoOralCreateRequest r, DateTime nowUtc) => new()
        {
            Slug = SlugHelper.Slugify(r.Slug),
            Titulo = r.Titulo?.Trim(),
            Descricao = r.Descricao?.Trim(),
            Ativo = r.Ativo,
            DataCriacao = nowUtc,
            Anexos = r.Anexos?.Select(a => new SessaoPlenariaSustentacaoOralAnexos
            {
                Ordem = a.Ordem?.Trim() ?? string.Empty,
                Titulo = a.Titulo?.Trim() ?? string.Empty,
                Descricao = a.Descricao?.Trim()
            }).ToList() ?? new()
        };

        // Update -> apply
        public static void ApplyUpdate(
           this SessaoPlenariaSustentacaoOral e,
           SessaoPlenariaSustentacaoOralUpdateRequest r,
           DateTime nowUtc)
        {
            if (!string.IsNullOrWhiteSpace(r.Slug))
                e.Slug = SlugHelper.Slugify(r.Slug);

            if (r.Titulo is not null)        
                e.Titulo = r.Titulo?.Trim();

            if (r.Descricao is not null)
                e.Descricao = r.Descricao?.Trim();

            if (r.Ativo.HasValue)        
                e.Ativo = r.Ativo.Value;

            e.DataAtualizacao = nowUtc;
        }
    }
}
