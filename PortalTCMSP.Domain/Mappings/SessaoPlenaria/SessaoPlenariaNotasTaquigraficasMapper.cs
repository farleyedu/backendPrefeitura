using PortalTCMSP.Domain.DTOs.Requests.SessaoPlenaria.NotasTaquigraficas;
using PortalTCMSP.Domain.DTOs.Responses.Base;
using PortalTCMSP.Domain.DTOs.Responses.SessaoPlenaria.NotasTaquigraficas;
using PortalTCMSP.Domain.Entities.SessaoPlenariaEntity;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.Mappings.SessaoPlenaria
{
    [ExcludeFromCodeCoverage]
    public static class SessaoPlenariaNotasTaquigraficasMapper
    {
        public static ResultadoPaginadoResponse<SessaoPlenariaNotasTaquigraficasResponse> Build(
        int page, int count, int total, IEnumerable<SessaoPlenariaNotasTaquigraficas> notas)
        {
            if (notas is null) return null!;
            var lista = notas.ToResponse();
            return new ResultadoPaginadoResponse<SessaoPlenariaNotasTaquigraficasResponse>(page, count, total, lista);
        }

        // Entity -> Response
        public static SessaoPlenariaNotasTaquigraficasResponse ToResponse(this SessaoPlenariaNotasTaquigraficas e) => new()
        {
            Id = e.Id,
            IdSessaoPlenaria = e.IdSessaoPlenaria,
            Numero = e.Numero,
            Tipo = e.Tipo,
            DataPublicacao = e.DataPublicacao,
            DataCriacao = e.DataCriacao,
            DataAtualizacao = e.DataAtualizacao,
            Anexos = e.Anexos.OrderBy(a => a.Ordem).Select(a => new SessaoPlenariaNotasTaquigraficasAnexoItemResponse
            {
                Id = a.Id,
                Link = a.Link,
                TipoArquivo = a.TipoArquivo,
                NomeExibicao = a.NomeExibicao,
                Ordem = a.Ordem
            }).ToList()
        };

        public static IEnumerable<SessaoPlenariaNotasTaquigraficasResponse> ToResponse(this IEnumerable<SessaoPlenariaNotasTaquigraficas> list)
            => list.Select(ToResponse);

        // Create -> Entity
        public static SessaoPlenariaNotasTaquigraficas FromCreate(this SessaoPlenariaNotasTaquigraficasCreateRequest r, DateTime nowUtc) => new()
        {
            IdSessaoPlenaria = r.IdSessaoPlenaria,
            Numero = r.Numero?.Trim() ?? string.Empty,
            Tipo = r.Tipo,
            DataPublicacao = r.DataPublicacao,
            DataCriacao = nowUtc,
            Anexos = r.Anexos?.Select(a => new SessaoPlenariaNotasTaquigraficasAnexos
            {
                Link = a.Link?.Trim() ?? string.Empty,
                TipoArquivo = a.TipoArquivo?.Trim(),
                NomeExibicao = a.NomeExibicao?.Trim(),
                Ordem = a.Ordem
            }).ToList() ?? new()
        };

        // Update -> apply

        public static void ApplyPartialUpdate(
            this SessaoPlenariaNotasTaquigraficas e,
            SessaoPlenariaNotasTaquigraficasUpdateRequest r,
            DateTime nowUtc)
        {
            if (r.IdSessaoPlenaria.HasValue)
                e.IdSessaoPlenaria = r.IdSessaoPlenaria.Value;

            if (r.Numero is not null)
                e.Numero = r.Numero?.Trim();

            if (r.Tipo.HasValue)
                e.Tipo = r.Tipo.Value;

            if (r.DataPublicacao.HasValue)
                e.DataPublicacao = r.DataPublicacao;

            e.DataAtualizacao = nowUtc;
        }
    }
}
