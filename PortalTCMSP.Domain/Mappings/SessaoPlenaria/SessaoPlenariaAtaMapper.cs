using PortalTCMSP.Domain.DTOs.Requests.SessaoPlenaria.Ata;
using PortalTCMSP.Domain.DTOs.Responses.Base;
using PortalTCMSP.Domain.DTOs.Responses.SessaoPlenaria.Ata;
using PortalTCMSP.Domain.Entities.SessaoPlenariaEntity;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.Mappings.SessaoPlenaria
{
    [ExcludeFromCodeCoverage]
    public static class SessaoPlenariaAtaMapper
    {
        // Entity -> Response
        public static SessaoPlenariaAtaResponse ToResponse(this SessaoPlenariaAta e) => new()
        {
            Id = e.Id,
            IdSessaoPlenaria = e.IdSessaoPlenaria,
            Numero = e.Numero,
            Tipo = e.Tipo,
            DataPublicacao = e.DataPublicacao,
            DataCriacao = e.DataCriacao,
            DataAtualizacao = e.DataAtualizacao,
            Anexos = e.Anexos.OrderBy(a => a.Ordem).Select(a => new SessaoPlenariaAtaAnexoItemResponse
            {
                Id = a.Id,
                Link = a.Link,
                TipoArquivo = a.TipoArquivo,
                NomeExibicao = a.NomeExibicao,
                Ordem = a.Ordem
            }).ToList()
        };

        public static IEnumerable<SessaoPlenariaAtaResponse> ToResponse(this IEnumerable<SessaoPlenariaAta> list)
            => list.Select(ToResponse);

        // Create -> Entity
        public static SessaoPlenariaAta FromCreate(this SessaoPlenariaAtaCreateRequest r, DateTime nowUtc) => new()
        {
            IdSessaoPlenaria = r.IdSessaoPlenaria,
            Numero = r.Numero?.Trim() ?? string.Empty,
            Tipo = r.Tipo,
            DataPublicacao = r.DataPublicacao,
            DataCriacao = nowUtc,
            Anexos = r.Anexos?.Select(a => new SessaoPlenariaAtaAnexo
            {
                Link = a.Link?.Trim() ?? string.Empty,
                TipoArquivo = a.TipoArquivo?.Trim(),
                NomeExibicao = a.NomeExibicao?.Trim(),
                Ordem = a.Ordem
            }).ToList() ?? new()
        };

        // Update -> apply
        public static void ApplyPartialUpdate(
    this SessaoPlenariaAta e,
    SessaoPlenariaAtaUpdateRequest r,
    DateTime nowUtc)
        {
            if (r.IdSessaoPlenaria.HasValue)
            {
                if (r.IdSessaoPlenaria.Value <= 0)
                    e.IdSessaoPlenaria = null;        
                else
                    e.IdSessaoPlenaria = r.IdSessaoPlenaria.Value;
            }

            if (r.Numero is not null)
                e.Numero = r.Numero?.Trim();

            if (r.Tipo.HasValue)
                e.Tipo = r.Tipo.Value;

            if (r.DataPublicacao.HasValue)
                e.DataPublicacao = r.DataPublicacao;

            e.DataAtualizacao = nowUtc;
        }

        public static ResultadoPaginadoResponse<SessaoPlenariaAtaResponse> Build(
        int page, int count, int total, IEnumerable<SessaoPlenariaAta> atas)
        {
            if (atas is null) return null!;
            var lista = atas.ToResponse();
            return new ResultadoPaginadoResponse<SessaoPlenariaAtaResponse>(page, count, total, lista);
        }
    }
}