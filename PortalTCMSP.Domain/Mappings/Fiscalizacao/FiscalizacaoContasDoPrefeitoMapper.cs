using PortalTCMSP.Domain.DTOs.Requests.Fiscalizacao.ContasDoPrefeito;
using PortalTCMSP.Domain.DTOs.Responses.Base;
using PortalTCMSP.Domain.DTOs.Responses.Fiscalizacao.ContasDoPrefeito;
using PortalTCMSP.Domain.Entities.FiscalizacaoEntity;
using PortalTCMSP.Domain.Entities.FiscalizacaoEntity.ContasDoPrefeito;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.Mappings.Fiscalizacao
{
    [ExcludeFromCodeCoverage]
    public static class FiscalizacaoContasDoPrefeitoMapper
    {
        public static FiscalizacaoContasDoPrefeitoResponse ToResponse(this FiscalizacaoContasDoPrefeito e) => new()
        {
            Id = e.Id,
            Ano = e.Ano,
            Pauta = e.Pauta,
            DataSessao = e.DataSessao,
            DataPublicacao = e.DataPublicacao,
            DataCriacao = e.DataCriacao,
            DataAtualizacao = e.DataAtualizacao,
            Anexos = [.. e.Anexos
                .OrderBy(a => a.Ordem)
                .Select(a => new FiscalizacaoContasDoPrefeitoAnexoItemResponse
                {
                    Id = a.Id,
                    Link = a.Link,
                    TipoArquivo = a.TipoArquivo,
                    NomeExibicao = a.NomeExibicao,
                    Ordem = a.Ordem
                })]
        };

        public static IEnumerable<FiscalizacaoContasDoPrefeitoResponse> ToResponse(this IEnumerable<FiscalizacaoContasDoPrefeito> list)
            => list.Select(ToResponse);

        public static FiscalizacaoContasDoPrefeito FromCreate(this FiscalizacaoContasDoPrefeitoCreateRequest r, DateTime nowUtc) => new()
        {
            Ano = r.Ano?.Trim() ?? string.Empty,
            Pauta = r.Pauta?.Trim() ?? string.Empty,
            DataSessao = r.DataSessao,
            DataPublicacao = r.DataPublicacao,
            DataCriacao = nowUtc,
            Anexos = r.Anexos?.Select(a => new FiscalizacaoContasDoPrefeitoAnexo
            {
                Link = a.Link?.Trim() ?? string.Empty,
                TipoArquivo = a.TipoArquivo?.Trim(),
                NomeExibicao = a.NomeExibicao?.Trim(),
                Ordem = a.Ordem
            }).ToList() ?? []
        };

        public static void ApplyUpdate(this FiscalizacaoContasDoPrefeito e, FiscalizacaoContasDoPrefeitoUpdateRequest r, DateTime nowUtc)
        {
            e.Ano = r.Ano?.Trim() ?? string.Empty;
            e.Pauta = r.Pauta?.Trim() ?? string.Empty;
            e.DataSessao = r.DataSessao;
            e.DataPublicacao = r.DataPublicacao;
            e.DataAtualizacao = nowUtc;
        }

        public static ResultadoPaginadoResponse<FiscalizacaoContasDoPrefeitoResponse> BuildPaged(
            int page, int count, int total, IEnumerable<FiscalizacaoContasDoPrefeito> items)
        {
            var lista = items.ToResponse();
            return new ResultadoPaginadoResponse<FiscalizacaoContasDoPrefeitoResponse>(page, count, total, lista);
        }
    }
}
