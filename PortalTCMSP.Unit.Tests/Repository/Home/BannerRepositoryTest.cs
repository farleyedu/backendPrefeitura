using Microsoft.EntityFrameworkCore;
using PortalTCMSP.Infra.Data.Context;
using PortalTCMSP.Infra.Data.Repositories.Home;
using PortalTCMSP.Unit.Tests.Repository.Home.FixFeature;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Unit.Tests.Repository.Home
{
    [ExcludeFromCodeCoverage]
    public class BannerRepositoryTest
    {
        private PortalTCMSPContext GetInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<PortalTCMSPContext>()
                .UseInMemoryDatabase(databaseName: "PortalTCMSPTestDB" + Guid.NewGuid())
                .Options;
            return new PortalTCMSPContext(options);
        }

        [Fact]
        public async Task InsertAsync_DeveInserirBanner()
        {
            // Arrange
            var fixture = new BannerRepositoryFixture();
            var context = GetInMemoryContext();
            var repository = new BannerRepository(context);
            var banner = fixture.GetBanner();

            // Act
            await repository.InsertAsync(banner);
            await repository.CommitAsync();

            // Assert
            var inserido = await repository.FindByIdAsync(banner.Id);
            Assert.NotNull(inserido);
            Assert.Equal(banner.Nome, inserido.Nome);
        }

        [Fact]
        public async Task DeleteAsync_DeveRemoverBanner()
        {
            // Arrange
            var fixture = new BannerRepositoryFixture();
            var context = GetInMemoryContext();
            var repository = new BannerRepository(context);
            var banner = fixture.GetBanner();

            await repository.InsertAsync(banner);
            await repository.CommitAsync();

            // Act
            await repository.DeleteAsync(banner.Id);
            await repository.CommitAsync();

            // Assert
            var removido = await repository.FindByIdAsync(banner.Id);
            Assert.Null(removido);
        }

        [Fact]
        public async Task UpdateAsync_DeveAtualizarBanner()
        {
            // Arrange
            var fixture = new BannerRepositoryFixture();
            var context = GetInMemoryContext();
            var repository = new BannerRepository(context);
            var banner = fixture.GetBanner();

            await repository.InsertAsync(banner);
            await repository.CommitAsync();

            // Act
            banner.Nome = "Banner Atualizado";
            await repository.UpdateAsync(banner);
            await repository.CommitAsync();

            // Assert
            var atualizado = await repository.FindByIdAsync(banner.Id);
            Assert.Equal("Banner Atualizado", atualizado?.Nome);
        }

        [Fact]
        public async Task AllAsync_DeveRetornarTodosOsBanners()
        {
            // Arrange
            var fixture = new BannerRepositoryFixture();
            var context = GetInMemoryContext();
            var repository = new BannerRepository(context);

            await repository.InsertAsync(fixture.GetBanner(1));
            await repository.InsertAsync(fixture.GetBanner(2));
            await repository.CommitAsync();

            // Act
            var todos = await repository.AllAsync();

            // Assert
            Assert.NotNull(todos);
            Assert.Equal(2, todos.Count());
        }
    }
}
