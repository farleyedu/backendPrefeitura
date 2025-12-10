using Microsoft.EntityFrameworkCore;
using PortalTCMSP.Domain.Entities.ServicosEntity.CartaServicosUsuarioEntity;
using PortalTCMSP.Infra.Data.Context;
using PortalTCMSP.Infra.Data.Repositories.Servicos.CartaServicosUsuario;

namespace PortalTCMSP.Infra.Data.Tests.Repositories.Servicos.CartaServicosUsuario
{
    public class CartaServicosUsuarioServicoItemDetalheRepositoryTests
    {
        private static PortalTCMSPContext CreateContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<PortalTCMSPContext>()
                .UseInMemoryDatabase(dbName)
                .Options;
            return new PortalTCMSPContext(options);
        }

        [Fact]
        public async Task CreateServicosItensDetalhesAsync_AddsNewDetalhes()
        {
            // Arrange
            var dbName = Guid.NewGuid().ToString();
            await using var context = CreateContext(dbName);
            var item = new CartaServicosUsuarioServicoItem
            {
                Id = 1,
                ItemDetalhe = new List<CartaServicosUsuarioItemDetalhe>()
            };
            context.CartaServicosUsuarioServicoItem.Add(item);
            await context.SaveChangesAsync();

            var repo = new CartaServicosUsuarioServicoItemDetalheRepository(context);
            var novos = new[]
            {
                new CartaServicosUsuarioItemDetalhe
                {
                    Id = 10,
                    IdCartaServicosUsuarioServicoItem = 1,
                    TituloDetalhe = "Detalhe 1"
                },
                new CartaServicosUsuarioItemDetalhe
                {
                    Id = 11,
                    IdCartaServicosUsuarioServicoItem = 1,
                    TituloDetalhe = "Detalhe 2"
                }
            };

            // Act
            await repo.CreateServicosItensDetalhesAsync(1, novos);

            // Assert
            var loaded = await context.CartaServicosUsuarioServicoItem
                .Include(x => x.ItemDetalhe)
                .FirstOrDefaultAsync(x => x.Id == 1);

            Assert.NotNull(loaded);
            Assert.Equal(2, loaded!.ItemDetalhe.Count);
            Assert.Contains(loaded.ItemDetalhe, d => d.TituloDetalhe == "Detalhe 1");
            Assert.Contains(loaded.ItemDetalhe, d => d.TituloDetalhe == "Detalhe 2");
        }

        [Fact]
        public async Task CreateServicosItensDetalhesAsync_ThrowsWhenItemNotFound()
        {
            // Arrange
            var dbName = Guid.NewGuid().ToString();
            await using var context = CreateContext(dbName);
            var repo = new CartaServicosUsuarioServicoItemDetalheRepository(context);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                repo.CreateServicosItensDetalhesAsync(999, new[] { new CartaServicosUsuarioItemDetalhe { IdCartaServicosUsuarioServicoItem = 999 } }));
        }

        [Fact]
        public async Task UpdateServicosItensDetalhesAsync_ReplacesExistingMatchingByIdCartaServicosUsuarioServicoItem()
        {
            // Arrange
            var dbName = Guid.NewGuid().ToString();
            await using var context = CreateContext(dbName);

            var existingDetalhe = new CartaServicosUsuarioItemDetalhe
            {
                Id = 100,
                IdCartaServicosUsuarioServicoItem = 2,
                TituloDetalhe = "Old Title"
            };

            var item = new CartaServicosUsuarioServicoItem
            {
                Id = 2,
                ItemDetalhe = new List<CartaServicosUsuarioItemDetalhe> { existingDetalhe }
            };

            context.CartaServicosUsuarioServicoItem.Add(item);
            await context.SaveChangesAsync();

            var repo = new CartaServicosUsuarioServicoItemDetalheRepository(context);
            var novos = new[]
            {
                new CartaServicosUsuarioItemDetalhe
                {
                    Id = 101,
                    IdCartaServicosUsuarioServicoItem = 2,
                    TituloDetalhe = "New Title"
                }
            };

            // Act
            await repo.UpdateServicosItensDetalhesAsync(2, novos);

            // Assert
            var loaded = await context.CartaServicosUsuarioServicoItem
                .Include(x => x.ItemDetalhe)
                .FirstOrDefaultAsync(x => x.Id == 2);

            Assert.NotNull(loaded);
            Assert.Single(loaded!.ItemDetalhe);
            var detalhe = loaded.ItemDetalhe.First();
            Assert.Equal(101, detalhe.Id);
            Assert.Equal("New Title", detalhe.TituloDetalhe);
            Assert.Equal(2, detalhe.IdCartaServicosUsuarioServicoItem);
        }

        [Fact]
        public async Task UpdateServicosItensDetalhesAsync_LeavesCollectionUnchangedWhenNoMatching()
        {
            // Arrange
            var dbName = Guid.NewGuid().ToString();
            await using var context = CreateContext(dbName);

            var existingDetalhe = new CartaServicosUsuarioItemDetalhe
            {
                Id = 200,
                IdCartaServicosUsuarioServicoItem = 2,
                TituloDetalhe = "Old Title"
            };

            var item = new CartaServicosUsuarioServicoItem
            {
                Id = 2,
                ItemDetalhe = new List<CartaServicosUsuarioItemDetalhe> { existingDetalhe }
            };

            context.CartaServicosUsuarioServicoItem.Add(item);
            await context.SaveChangesAsync();

            var repo = new CartaServicosUsuarioServicoItemDetalheRepository(context);
            var novos = new[]
            {
                new CartaServicosUsuarioItemDetalhe
                {
                    Id = 201,
                    // mismatch: different IdCartaServicosUsuarioServicoItem -> should not replace
                    IdCartaServicosUsuarioServicoItem = 999,
                    TituloDetalhe = "Should Not Replace"
                }
            };

            // Act
            await repo.UpdateServicosItensDetalhesAsync(2, novos);

            // Assert
            var loaded = await context.CartaServicosUsuarioServicoItem
                .Include(x => x.ItemDetalhe)
                .FirstOrDefaultAsync(x => x.Id == 2);

            Assert.NotNull(loaded);
            Assert.Single(loaded!.ItemDetalhe);
            var detalhe = loaded.ItemDetalhe.First();
            Assert.Equal(200, detalhe.Id);
            Assert.Equal("Old Title", detalhe.TituloDetalhe);
        }

        [Fact]
        public async Task UpdateServicosItensDetalhesAsync_ThrowsWhenItemNotFound()
        {
            // Arrange
            var dbName = Guid.NewGuid().ToString();
            await using var context = CreateContext(dbName);
            var repo = new CartaServicosUsuarioServicoItemDetalheRepository(context);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                repo.UpdateServicosItensDetalhesAsync(999, new[] { new CartaServicosUsuarioItemDetalhe { IdCartaServicosUsuarioServicoItem = 999 } }));
        }
    }
}