using PortalTCMSP.Domain.DTOs.Requests.BannerRequest;
using PortalTCMSP.Domain.DTOs.Responses.Banner;
using PortalTCMSP.Domain.Entities.BannerEntity;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Unit.Tests.Services.FixFeature
{
    [ExcludeFromCodeCoverage]
    public class BannerServiceFixture
    {
        public BannerCreateRequest GetBannerCreateRequest(bool ativo = true) => new()
        {
            Nome = ativo ? "Banner Ativo" : "Banner Inativo",
            Imagem = ativo ? "img/ativo.jpg" : "img/inativo.jpg",
            Ativo = ativo
        };

        public BannerUpdateRequest GetBannerUpdateRequest(int id, bool ativo = true) => new()
        {
            Nome = ativo ? "Banner Atualizado Ativo" : "Banner Atualizado Inativo",
            Imagem = ativo ? "img/atualizado_ativo.jpg" : "img/atualizado_inativo.jpg",
            Ativo = ativo
        };

        public Banner GetBannerEntity(int id = 1, bool ativo = true) => new()
        {
            Id = id,
            Nome = ativo ? "Banner Entidade Ativo" : "Banner Entidade Inativo",
            Imagem = ativo ? "img/entidade_ativo.jpg" : "img/entidade_inativo.jpg",
            Ativo = ativo,
            DataCriacao = DateTime.UtcNow
        };

        public BannerResponse GetBannerResponse(int id = 1, bool ativo = true) => new()
        {
            Id = id,
            Nome = ativo ? "Banner Resp Ativo" : "Banner Resp Inativo",
            ImagemUrl = ativo ? "img/resp_ativo.jpg" : "img/resp_inativo.jpg",
            Ativo = ativo,
            DataCriacao = DateTime.UtcNow
        };
    }
}
