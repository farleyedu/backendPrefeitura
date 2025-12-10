using Microsoft.EntityFrameworkCore;
using PortalTCMSP.Domain.Entities.ServicosEntity.CartaServicosUsuarioEntity;
using PortalTCMSP.Infra.Data.Context;
using PortalTCMSP.Infra.Data.Repositories.Servicos.CartaServicosUsuario;

namespace PortalTCMSP.Infra.Data.Tests.Repositories.Servicos
{
    public class CartaServicosUsuarioServicoItemRepositoryTests
    {
        private static PortalTCMSPContext CreateContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<PortalTCMSPContext>()
                .UseInMemoryDatabase(dbName)
                .Options;
            return new PortalTCMSPContext(options);
        }

        [Fact]
        public async Task CreateServicosItensAsync_Adds_New_Items_To_Parent()
        {
            // Arrange
            var dbName = Guid.NewGuid().ToString();
            await using var context = CreateContext(dbName);

            var parent = new CartaServicosUsuarioServico
            {
                Id = 100,
                IdCartaServicosUsuario = 50,
                ServicosItens = new List<CartaServicosUsuarioServicoItem>()
            };

            context.CartaServicosUsuarioServico.Add(parent);
            await context.SaveChangesAsync();

            var novos = new[]
            {
                new CartaServicosUsuarioServicoItem { IdCartaServicosUsuarioServico = parent.Id, Titulo = "Novo 1" },
                new CartaServicosUsuarioServicoItem { IdCartaServicosUsuarioServico = parent.Id, Titulo = "Novo 2" }
            };

            var repo = new CartaServicosUsuarioServicoItemRepository(context);

            // Act
            await repo.CreateServicosItensAsync(parent.Id, novos);

            // Assert
            var reloaded = await context.CartaServicosUsuarioServico
                .Include(p => p.ServicosItens)
                .FirstAsync(p => p.Id == parent.Id);

            Assert.Equal(2, reloaded.ServicosItens.Count);
            Assert.Contains(reloaded.ServicosItens, i => i.Titulo == "Novo 1");
            Assert.Contains(reloaded.ServicosItens, i => i.Titulo == "Novo 2");
        }

        [Fact]
        public async Task CreateServicosItensAsync_Throws_When_Parent_NotFound()
        {
            // Arrange
            var dbName = Guid.NewGuid().ToString();
            await using var context = CreateContext(dbName);
            var repo = new CartaServicosUsuarioServicoItemRepository(context);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                repo.CreateServicosItensAsync(9999, new[] { new CartaServicosUsuarioServicoItem() }));

            Assert.Contains("CartaServicosUsuarioServico with Id 9999 not found", ex.Message);
        }

        [Fact]
        public async Task UpdateServicosItensAsync_Replaces_First_Matching_By_ForeignKey()
        {
            // Arrange
            var dbName = Guid.NewGuid().ToString();
            await using var context = CreateContext(dbName);

            var parent = new CartaServicosUsuarioServico
            {
                Id = 200,
                IdCartaServicosUsuario = 20,
                ServicosItens = new List<CartaServicosUsuarioServicoItem>
                {
                    new CartaServicosUsuarioServicoItem { Id = 1, IdCartaServicosUsuarioServico = 200, Titulo = "Existente A" },
                    new CartaServicosUsuarioServicoItem { Id = 2, IdCartaServicosUsuarioServico = 200, Titulo = "Existente B" }
                }
            };

            context.CartaServicosUsuarioServico.Add(parent);
            await context.SaveChangesAsync();

            var novos = new[]
            {
                new CartaServicosUsuarioServicoItem { Id = 3, IdCartaServicosUsuarioServico = 200, Titulo = "Novo Substituto" }
            };

            var repo = new CartaServicosUsuarioServicoItemRepository(context);

            // Act
            await repo.UpdateServicosItensAsync(parent.Id, novos);

            // Assert
            var reloaded = await context.CartaServicosUsuarioServico
                .Include(p => p.ServicosItens)
                .FirstAsync(p => p.Id == parent.Id);

            Assert.Equal(2, reloaded.ServicosItens.Count);

            Assert.Contains(reloaded.ServicosItens, i => i.Titulo == "Novo Substituto");
            Assert.Contains(reloaded.ServicosItens, i => i.Titulo == "Existente B" || i.Titulo == "Existente A");
        }

        [Fact]
        public async Task UpdateServicosItensAsync_Does_Nothing_When_No_Matching_Item_By_ForeignKey()
        {
            // Arrange
            var dbName = Guid.NewGuid().ToString();
            await using var context = CreateContext(dbName);

            var parent = new CartaServicosUsuarioServico
            {
                Id = 300,
                IdCartaServicosUsuario = 30,
                ServicosItens = new List<CartaServicosUsuarioServicoItem>
                {
                    new CartaServicosUsuarioServicoItem { Id = 10, IdCartaServicosUsuarioServico = 300, Titulo = "Existente" }
                }
            };

            context.CartaServicosUsuarioServico.Add(parent);
            await context.SaveChangesAsync();

            var novos = new[]
            {
                new CartaServicosUsuarioServicoItem { Id = 11, IdCartaServicosUsuarioServico = 999, Titulo = "Nao Encontrado" }
            };

            var repo = new CartaServicosUsuarioServicoItemRepository(context);

            // Act
            await repo.UpdateServicosItensAsync(parent.Id, novos);

            var reloaded = await context.CartaServicosUsuarioServico
                .Include(p => p.ServicosItens)
                .FirstAsync(p => p.Id == parent.Id);

            Assert.Single(reloaded.ServicosItens);
            Assert.Equal("Existente", reloaded.ServicosItens.First().Titulo);
        }

        [Fact]
        public async Task UpdateServicosItensAsync_Throws_When_Parent_NotFound()
        {
            // Arrange
            var dbName = Guid.NewGuid().ToString();
            await using var context = CreateContext(dbName);
            var repo = new CartaServicosUsuarioServicoItemRepository(context);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                repo.UpdateServicosItensAsync(4242, new[] { new CartaServicosUsuarioServicoItem() }));

            Assert.Contains("CartaServicosUsuarioServico with Id 4242 not found", ex.Message);
        }
    }
}