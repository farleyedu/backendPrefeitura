using PortalTCMSP.Domain.DTOs.Requests.BannerRequest;
using PortalTCMSP.Domain.DTOs.Responses.Banner;
using PortalTCMSP.Domain.Entities.BannerEntity;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.Mappings.Home
{
    [ExcludeFromCodeCoverage]
    public static class BannerMapper
    {
        public static BannerResponse ToResponse(this Banner b) => new()
        {
            Id = b.Id,
            Nome = b.Nome,
            ImagemUrl = b.Imagem,  
            Ativo = b.Ativo,
            DataCriacao = b.DataCriacao,
            DataAtualizacao = b.DataAtualizacao
        };

        public static Banner ToEntity(this BannerCreateRequest r, DateTime nowUtc) => new()
        {
            Nome = r.Nome?.Trim() ?? string.Empty,
            Imagem = r.Imagem?.Trim() ?? string.Empty,
            Ativo = r.Ativo,
            DataCriacao = nowUtc
        };

        public static void MapUpdate(this Banner b, BannerUpdateRequest r, DateTime nowUtc)
        {
            b.Nome = r.Nome?.Trim() ?? string.Empty;
            b.Imagem = r.Imagem?.Trim() ?? string.Empty;
            b.Ativo = r.Ativo;
            b.DataAtualizacao = nowUtc;
        }
    }
}
