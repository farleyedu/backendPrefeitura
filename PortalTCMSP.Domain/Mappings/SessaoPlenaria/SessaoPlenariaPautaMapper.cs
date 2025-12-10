using PortalTCMSP.Domain.DTOs.Requests.SessaoPlenaria.Pauta;
using PortalTCMSP.Domain.DTOs.Responses.Base;
using PortalTCMSP.Domain.DTOs.Responses.SessaoPlenaria.Pauta;
using PortalTCMSP.Domain.Entities.SessaoPlenariaEntity;
using PortalTCMSP.Domain.Helper;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.Mappings.SessaoPlenaria
{
    [ExcludeFromCodeCoverage]
    public static class SessaoPlenariaPautaMapper
    {
        public static ResultadoPaginadoResponse<SessaoPlenariaPautaResponse> Build(
        int page, int count, int total, IEnumerable<SessaoPlenariaPauta> pautas)
        {
            if (pautas is null)
                return null!; 

            var lista = pautas.ToResponse(); 
            return new ResultadoPaginadoResponse<SessaoPlenariaPautaResponse>(page, count, total, lista);
        }

        // entity -> response
        public static SessaoPlenariaPautaResponse ToResponse(this SessaoPlenariaPauta e) => new()
        {
            Id = e.Id,
            IdSessaoPlenaria = e.IdSessaoPlenaria,
            Numero = e.Numero,
            Tipo = e.Tipo,
            DataCriacao = e.DataCriacao,
            DataDaSesao = e.DataDaSesao,
            DataPublicacao = e.DataPublicacao,
            DataAtualizacao = e.DataAtualizacao,
            Anexos = e.Anexos.OrderBy(a => a.Ordem).Select(a => new SessaoPlenariaPautaAnexoItemResponse
            {
                Id = a.Id,
                Link = a.Link,
                TipoArquivo = a.TipoArquivo,
                NomeExibicao = a.NomeExibicao,
                Ordem = a.Ordem
            }).ToList()
        };

        public static IEnumerable<SessaoPlenariaPautaResponse> ToResponse(this IEnumerable<SessaoPlenariaPauta> list)
            => list.Select(ToResponse);

        // create -> entity
        public static SessaoPlenariaPauta FromCreate(this SessaoPlenariaPautaCreateRequest r, DateTime nowUtc) => new()
        {
            IdSessaoPlenaria = r.IdSessaoPlenaria,
            Numero = r.Numero?.Trim() ?? string.Empty,
            Tipo = r.Tipo,
            DataCriacao = nowUtc,
            DataDaSesao = r.DataDaSesao,
            DataPublicacao = r.DataPublicacao,
            Anexos = r.Anexos?.Select(a => new SessaoPlenariaPautaAnexo
            {
                Link = a.Link?.Trim() ?? string.Empty,
                TipoArquivo = a.TipoArquivo?.Trim(),
                NomeExibicao = a.NomeExibicao?.Trim(),
                Ordem = a.Ordem
            }).ToList() ?? new()
        };

        // update -> apply
        public static void ApplyPartialUpdate(
            this SessaoPlenariaPauta e,
            SessaoPlenariaPautaUpdateRequest r,
            DateTime nowUtc)
        {
            if (r.IdSessaoPlenaria.HasValue)
                e.IdSessaoPlenaria = r.IdSessaoPlenaria.Value; // <<< .Value resolve int?/int

            if (r.Numero is not null)
                e.Numero = r.Numero?.Trim();

            if (r.Tipo.HasValue)
                e.Tipo = r.Tipo.Value;

            if (r.DataDaSesao.HasValue)          // <<< manter nome correto
                e.DataDaSesao = r.DataDaSesao;

            if (r.DataPublicacao.HasValue)
                e.DataPublicacao = r.DataPublicacao;

            e.DataAtualizacao = nowUtc;
        }
    }
}
