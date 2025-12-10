using Microsoft.EntityFrameworkCore;
using PortalTCMSP.Domain.Entities.ServicosEntity.PrazosProcessuaisEntity;
using PortalTCMSP.Infra.Data.Context;
using PortalTCMSP.Infra.Data.Repositories.Servicos.PrazosProcessuais;

namespace PortalTCMSP.Infra.Data.Tests.Repositories.Servicos.PrazosProcessuais
{
    public class PrazosProcessuaisItemRepositoryTests
    {
        private static PortalTCMSPContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<PortalTCMSPContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new PortalTCMSPContext(options);
        }

        [Fact]
        public async Task CreatePrazosProcessuaisItemAsync_WhenParentExists_AddsNewItems()
        {
            await using var context = CreateContext();

            var parent = new Domain.Entities.ServicosEntity.PrazosProcessuaisEntity.PrazosProcessuais
            {
                Id = 1,
                Ativo = true,
                DataCriacao = DateTime.UtcNow,
                PrazosProcessuaisItens = new List<PrazosProcessuaisItem>
                {
                    new PrazosProcessuaisItem
                    {
                        Id = 10,
                        IdPrazosProcessuais = 1,
                        Nome = "existing",
                        Ativo = true
                    }
                }
            };

            context.PrazosProcessuais.Add(parent);
            await context.SaveChangesAsync();

            var repo = new PrazosProcessuaisItemRepository(context);

            var novos = new[]
            {
                new PrazosProcessuaisItem
                {
                    Nome = "new item",
                    IdPrazosProcessuais = 1,
                    Ordem = 2,
                    Ativo = true
                }
            };

            await repo.CreatePrazosProcessuaisItemAsync(1, novos);

            await context.SaveChangesAsync();

            var reloaded = await context.PrazosProcessuais
                .Include(p => p.PrazosProcessuaisItens)
                .FirstOrDefaultAsync(p => p.Id == 1);

            Assert.NotNull(reloaded);
            Assert.Equal(2, reloaded!.PrazosProcessuaisItens.Count);
            Assert.Contains(reloaded.PrazosProcessuaisItens, i => i.Nome == "new item" && i.IdPrazosProcessuais == 1);
        }

        [Fact]
        public async Task CreatePrazosProcessuaisItemAsync_WhenParentNotFound_ThrowsInvalidOperationException()
        {
            await using var context = CreateContext();
            var repo = new PrazosProcessuaisItemRepository(context);

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                repo.CreatePrazosProcessuaisItemAsync(999, Array.Empty<PrazosProcessuaisItem>()));
        }

        [Fact]
        public async Task UpdatePrazosProcessuaisItemAsync_WhenParentNotFound_ThrowsInvalidOperationException()
        {
            await using var context = CreateContext();
            var repo = new PrazosProcessuaisItemRepository(context);

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                repo.UpdatePrazosProcessuaisItemAsync(12345, Array.Empty<PrazosProcessuaisItem>()));
        }

        [Fact]
        public async Task UpdatePrazosProcessuaisItemAsync_WhenMatchingItem_ReplacesItemProperties()
        {
            await using var context = CreateContext();

            var item = new PrazosProcessuaisItem
            {
                Id = 11,
                IdPrazosProcessuais = 9,
                Nome = "old name",
                Ordem = 1,
                Ativo = true
            };

            var parent = new Domain.Entities.ServicosEntity.PrazosProcessuaisEntity.PrazosProcessuais
            {
                Id = 9,
                Ativo = true,
                DataCriacao = DateTime.UtcNow,
                PrazosProcessuaisItens = new List<PrazosProcessuaisItem> { item }
            };

            context.PrazosProcessuais.Add(parent);
            await context.SaveChangesAsync();

            var repo = new PrazosProcessuaisItemRepository(context);

            item.Nome = "updated name";
            item.Ordem = 5;
            item.Ativo = false;

            var novos = new[] { item };

            await repo.UpdatePrazosProcessuaisItemAsync(9, novos);

            await context.SaveChangesAsync();

            var reloaded = await context.PrazosProcessuais
                .Include(p => p.PrazosProcessuaisItens)
                .FirstOrDefaultAsync(p => p.Id == 9);

            Assert.NotNull(reloaded);
            Assert.Single(reloaded!.PrazosProcessuaisItens);
            var updated = reloaded.PrazosProcessuaisItens.Single();
            Assert.Equal(11, updated.Id);
            Assert.Equal("updated name", updated.Nome);
            Assert.Equal(5, updated.Ordem);
            Assert.False(updated.Ativo);
        }

        [Fact]
        public async Task UpdatePrazosProcessuaisItemAsync_WhenNoMatchingItem_DoesNotModifyExisting()
        {
            await using var context = CreateContext();

            var existing = new PrazosProcessuaisItem
            {
                Id = 21,
                IdPrazosProcessuais = 20,
                Nome = "original",
                Ordem = 1,
                Ativo = true
            };

            var parent = new Domain.Entities.ServicosEntity.PrazosProcessuaisEntity.PrazosProcessuais
            {
                Id = 20,
                Ativo = true,
                DataCriacao = DateTime.UtcNow,
                PrazosProcessuaisItens = new List<PrazosProcessuaisItem> { existing }
            };

            context.PrazosProcessuais.Add(parent);
            await context.SaveChangesAsync();

            var repo = new PrazosProcessuaisItemRepository(context);

            var novo = new PrazosProcessuaisItem
            {
                Id = 999, 
                IdPrazosProcessuais = 20,
                Nome = "should not be added",
                Ordem = 2,
                Ativo = false
            };

            await repo.UpdatePrazosProcessuaisItemAsync(20, new[] { novo });

            await context.SaveChangesAsync();

            var reloaded = await context.PrazosProcessuais
                .Include(p => p.PrazosProcessuaisItens)
                .FirstOrDefaultAsync(p => p.Id == 20);

            Assert.NotNull(reloaded);
            Assert.Single(reloaded!.PrazosProcessuaisItens);
            var still = reloaded.PrazosProcessuaisItens.Single();
            Assert.Equal(21, still.Id);
            Assert.Equal("original", still.Nome);
        }
    }
}