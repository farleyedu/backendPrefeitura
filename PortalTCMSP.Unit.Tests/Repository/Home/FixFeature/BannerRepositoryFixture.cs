using PortalTCMSP.Domain.Entities.BannerEntity;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Unit.Tests.Repository.Home.FixFeature
{
    [ExcludeFromCodeCoverage]
    public class BannerRepositoryFixture
    {
        public Banner GetBanner(long id = 1) => new Banner
        {
            Id = id,
            Nome = "Banner Teste",
            Imagem = "img/teste.jpg",
            Ativo = true,
        };
    }
}
