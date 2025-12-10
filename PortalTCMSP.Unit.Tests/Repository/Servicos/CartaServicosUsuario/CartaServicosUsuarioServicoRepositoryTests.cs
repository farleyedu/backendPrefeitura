using Microsoft.EntityFrameworkCore;
using PortalTCMSP.Domain.Entities.ServicosEntity.CartaServicosUsuarioEntity;
using PortalTCMSP.Infra.Data.Context;
using PortalTCMSP.Infra.Data.Repositories.Servicos.CartaServicosUsuario;

namespace PortalTCMSP.Infra.Data.Tests.Repositories.Servicos.CartaServicosUsuario
{
    public class CartaServicosUsuarioServicoRepositoryTests
    {
        private static PortalTCMSPContext CreateInMemoryContext(string dbName = null)
        {
            var options = new DbContextOptionsBuilder<PortalTCMSPContext>()
                .UseInMemoryDatabase(dbName ?? Guid.NewGuid().ToString())
                .Options;

            return new PortalTCMSPContext(options);
        }

        [Fact]
        public async Task CreateServicosAsync_WhenCartaNotFound_ThrowsInvalidOperationException()
        {
            // Arrange
            await using var context = CreateInMemoryContext();
            var repo = new CartaServicosUsuarioServicoRepository(context);
            var novos = new List<CartaServicosUsuarioServico>
            {
                new CartaServicosUsuarioServico { Titulo = "S1" }
            };

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => repo.CreateServicosAsync(123, novos));
        }

        [Fact]
        public async Task CreateServicosAsync_WhenCartaFound_AddsNewServicos_And_SetsEntityStateModified()
        {
            // Arrange
            await using var context = CreateInMemoryContext();
            var carta = new Domain.Entities.ServicosEntity.CartaServicosUsuarioEntity.CartaServicosUsuario
            {
                Id = 1,
                Servicos = new List<CartaServicosUsuarioServico>
                {
                    new CartaServicosUsuarioServico { Id = 10, Titulo = "Existing" }
                }
            };
            context.CartaServicosUsuario.Add(carta);
            await context.SaveChangesAsync();

            var repo = new CartaServicosUsuarioServicoRepository(context);
            var novos = new List<CartaServicosUsuarioServico>
            {
                new CartaServicosUsuarioServico { Id = 20, Titulo = "NewService" }
            };

            // Act
            await repo.CreateServicosAsync(1, novos);

            // Assert
            var loaded = await context.CartaServicosUsuario.Include(c => c.Servicos).FirstAsync(c => c.Id == 1);
            Assert.Contains(loaded.Servicos, s => s.Titulo == "NewService");
            var entryState = context.Entry(loaded).State;
            Assert.Equal(EntityState.Modified, entryState);
        }

        [Fact]
        public async Task UpdateServicosAsync_WhenCartaNotFound_ThrowsInvalidOperationException()
        {
            // Arrange
            await using var context = CreateInMemoryContext();
            var repo = new CartaServicosUsuarioServicoRepository(context);
            var novos = new List<CartaServicosUsuarioServico>
            {
                new CartaServicosUsuarioServico { Titulo = "S1" }
            };

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => repo.UpdateServicosAsync(999, novos));
        }

        [Fact]
        public async Task UpdateServicosAsync_WhenCartaFound_DoesNotThrow_And_SetsEntityStateModified_WithoutChangingExistingCollection()
        {
            // Arrange
            await using var context = CreateInMemoryContext();
            var existing = new Domain.Entities.ServicosEntity.CartaServicosUsuarioEntity.CartaServicosUsuario
            {
                Id = 5,
                Servicos = new List<CartaServicosUsuarioServico>
                {
                    new CartaServicosUsuarioServico { Id = 101, Titulo = "ExistingService" }
                }
            };
            context.CartaServicosUsuario.Add(existing);
            await context.SaveChangesAsync();

            var repo = new CartaServicosUsuarioServicoRepository(context);
            var novos = new List<CartaServicosUsuarioServico>
            {
                new CartaServicosUsuarioServico { Id = 202, Titulo = "IgnoredByCurrentImplementation" }
            };

            // Act
            await repo.UpdateServicosAsync(5, novos);

            var loaded = await context.CartaServicosUsuario.Include(c => c.Servicos).FirstAsync(c => c.Id == 5);
            Assert.Single(loaded.Servicos);
            Assert.Contains(loaded.Servicos, s => s.Titulo == "ExistingService");
            var entryState = context.Entry(loaded).State;
            Assert.Equal(EntityState.Modified, entryState);
        }
    }
}