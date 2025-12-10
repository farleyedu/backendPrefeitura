using Microsoft.EntityFrameworkCore;
using PortalTCMSP.Domain.Entities.ServicosEntity.CartaServicosUsuarioEntity;
using PortalTCMSP.Infra.Data.Context;
using PortalTCMSP.Infra.Data.Repositories.Servicos.CartaServicosUsuario;

namespace PortalTCMSP.Infra.Tests.Repositories.Servicos.CartaServicosUsuario
{
    public class CartaServicosUsuarioDescritivoItemDetalheRepositoryTests
    {
        private static PortalTCMSPContext CreateContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<PortalTCMSPContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;
            return new PortalTCMSPContext(options);
        }

        [Fact]
        public async Task CreateDescritivoItemDetalheAsync_AddsNewItems_WhenParentExists()
        {
            var dbName = Guid.NewGuid().ToString();
            await using var context = CreateContext(dbName);

            // Arrange: create parent item without children
            var parent = new CartaServicosUsuarioItemDetalhe
            {
                TituloDetalhe = "parent",
                DescritivoItemDetalhe = new List<CartaServicosUsuarioDescritivoItemDetalhe>()
            };
            context.CartaServicosUsuarioItemDetalhe.Add(parent);
            await context.SaveChangesAsync();

            var novos = new[]
            {
                new CartaServicosUsuarioDescritivoItemDetalhe { IdCartaServicosUsuarioItemDetalhe = parent.Id, Descritivo = "d1", Ordem = 1 },
                new CartaServicosUsuarioDescritivoItemDetalhe { IdCartaServicosUsuarioItemDetalhe = parent.Id, Descritivo = "d2", Ordem = 2 }
            };

            var repo = new CartaServicosUsuarioDescritivoItemDetalheRepository(context);

            // Act
            await repo.CreateDescritivoItemDetalheAsync(parent.Id, novos);
            await context.SaveChangesAsync();

            // Assert - children persisted and associated to parent
            var persisted = context.CartaServicosUsuarioDescritivoItemDetalhe
                .Where(d => d.IdCartaServicosUsuarioItemDetalhe == parent.Id)
                .OrderBy(d => d.Ordem)
                .ToList();

            Assert.Equal(2, persisted.Count);
            Assert.Equal("d1", persisted[0].Descritivo);
            Assert.Equal("d2", persisted[1].Descritivo);
        }

        [Fact]
        public async Task CreateDescritivoItemDetalheAsync_Throws_WhenParentNotFound()
        {
            var dbName = Guid.NewGuid().ToString();
            await using var context = CreateContext(dbName);

            var repo = new CartaServicosUsuarioDescritivoItemDetalheRepository(context);
            var missingId = 9999L;
            var novos = new[] { new CartaServicosUsuarioDescritivoItemDetalhe { IdCartaServicosUsuarioItemDetalhe = missingId, Descritivo = "x" } };

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => repo.CreateDescritivoItemDetalheAsync(missingId, novos));
            Assert.Contains(missingId.ToString(), ex.Message);
        }

        [Fact]
        public async Task UpdateDescritivoItemDetalheAsync_ReplacesFirstMatchingItem_WhenNovosProvided()
        {
            var dbName = Guid.NewGuid().ToString();
            await using var context = CreateContext(dbName);

            // Arrange: parent with two existing children
            var parent = new CartaServicosUsuarioItemDetalhe
            {
                TituloDetalhe = "parent",
                DescritivoItemDetalhe = new List<CartaServicosUsuarioDescritivoItemDetalhe>()
            };

            var existing1 = new CartaServicosUsuarioDescritivoItemDetalhe { Descritivo = "old1", Ordem = 1, IdCartaServicosUsuarioItemDetalhe = 0 }; 
            var existing2 = new CartaServicosUsuarioDescritivoItemDetalhe { Descritivo = "old2", Ordem = 2, IdCartaServicosUsuarioItemDetalhe = 0 };

            parent.DescritivoItemDetalhe.Add(existing1);
            parent.DescritivoItemDetalhe.Add(existing2);

            context.CartaServicosUsuarioItemDetalhe.Add(parent);
            await context.SaveChangesAsync();

            // Ensure children's foreign key is parent.Id
            foreach (var child in parent.DescritivoItemDetalhe)
                child.IdCartaServicosUsuarioItemDetalhe = parent.Id;
            await context.SaveChangesAsync();

            var novos = new[]
            {
                new CartaServicosUsuarioDescritivoItemDetalhe { IdCartaServicosUsuarioItemDetalhe = parent.Id, Descritivo = "new1", Ordem = 10 }
            };

            var repo = new CartaServicosUsuarioDescritivoItemDetalheRepository(context);

            // Act
            await repo.UpdateDescritivoItemDetalheAsync(parent.Id, novos);
            await context.SaveChangesAsync();

            // Assert: one of the old items replaced by new1, total count remains 2
            var persisted = context.CartaServicosUsuarioDescritivoItemDetalhe
                .Where(d => d.IdCartaServicosUsuarioItemDetalhe == parent.Id)
                .Select(d => d.Descritivo)
                .ToList();

            Assert.Equal(2, persisted.Count);
            Assert.Contains("new1", persisted);
            Assert.Contains("old2", persisted);
            Assert.DoesNotContain("old1", persisted);
        }

        [Fact]
        public async Task UpdateDescritivoItemDetalheAsync_Throws_WhenParentNotFound()
        {
            var dbName = Guid.NewGuid().ToString();
            await using var context = CreateContext(dbName);

            var repo = new CartaServicosUsuarioDescritivoItemDetalheRepository(context);
            var missingId = 4242L;
            var novos = new[] { new CartaServicosUsuarioDescritivoItemDetalhe { IdCartaServicosUsuarioItemDetalhe = missingId, Descritivo = "x" } };

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => repo.UpdateDescritivoItemDetalheAsync(missingId, novos));
            Assert.Contains(missingId.ToString(), ex.Message);
        }
    }
}