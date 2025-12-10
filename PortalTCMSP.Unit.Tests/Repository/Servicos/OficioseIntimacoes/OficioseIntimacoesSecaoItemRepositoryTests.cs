using Microsoft.EntityFrameworkCore;
using PortalTCMSP.Domain.Entities.ServicosEntity.OficioseIntimacoesEntity;
using PortalTCMSP.Infra.Data.Context;
using PortalTCMSP.Infra.Data.Repositories.Servicos.OficioseIntimacoes;

namespace PortalTCMSP.Infra.Data.Tests.Repositories.Servicos.OficioseIntimacoes
{
    public class OficioseIntimacoesSecaoItemRepositoryTests
    {
        private static PortalTCMSPContext CreateContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<PortalTCMSPContext>()
                .UseInMemoryDatabase(dbName)
                .Options;
            return new PortalTCMSPContext(options);
        }

        [Fact]
        public async Task CreateSecaoItensAsync_WhenSecaoNotFound_ThrowsInvalidOperationException()
        {
            using var context = CreateContext(Guid.NewGuid().ToString());
            var repo = new OficioseIntimacoesSecaoItemRepository(context);

            var novos = new[] { new OficioseIntimacoesSecaoItem { Ordem = 1, Descricao = "x" } };

            await Assert.ThrowsAsync<InvalidOperationException>(() => repo.CreateSecaoItensAsync(999, novos));
        }

        [Fact]
        public async Task CreateSecaoItensAsync_WhenSecaoExists_AddsItemsToSecaoCollectionAndUpdatesContext()
        {
            var dbName = Guid.NewGuid().ToString();
            using (var context = CreateContext(dbName))
            {
                var secao = new OficioseIntimacoesSecao
                {
                    Id = 1,
                    SecaoItem = null 
                };
                context.OficioseIntimacoesSecao.Add(secao);
                await context.SaveChangesAsync();
            }

            using (var context = CreateContext(dbName))
            {
                var repo = new OficioseIntimacoesSecaoItemRepository(context);

                var novos = new[]
                {
                    new OficioseIntimacoesSecaoItem { Ordem = 1, Descricao = "Item A", IdOficioseIntimacoesSecao = 1 },
                    new OficioseIntimacoesSecaoItem { Ordem = 2, Descricao = "Item B", IdOficioseIntimacoesSecao = 1 }
                };

                await repo.CreateSecaoItensAsync(1, novos);

                var updated = await context.OficioseIntimacoesSecao
                    .Include(s => s.SecaoItem)
                    .FirstOrDefaultAsync(s => s.Id == 1);

                Assert.NotNull(updated);
                Assert.NotNull(updated.SecaoItem);
                Assert.Equal(2, updated.SecaoItem.Count);
                Assert.Contains(updated.SecaoItem, i => i.Descricao == "Item A");
                Assert.Contains(updated.SecaoItem, i => i.Descricao == "Item B");
            }
        }

        [Fact]
        public async Task UpdateSecaoItensAsync_WhenSecaoNotFound_ThrowsInvalidOperationException()
        {
            using var context = CreateContext(Guid.NewGuid().ToString());
            var repo = new OficioseIntimacoesSecaoItemRepository(context);

            var novos = new[] { new OficioseIntimacoesSecaoItem { Ordem = 1, Descricao = "x" } };

            await Assert.ThrowsAsync<InvalidOperationException>(() => repo.UpdateSecaoItensAsync(999, novos));
        }

        [Fact]
        public async Task UpdateSecaoItensAsync_WhenSecaoHasNullCollection_SetsEmptyCollectionAndDoesNotThrow()
        {
            var dbName = Guid.NewGuid().ToString();
            using (var context = CreateContext(dbName))
            {
                var secao = new OficioseIntimacoesSecao
                {
                    Id = 2,
                    SecaoItem = null 
                };
                context.OficioseIntimacoesSecao.Add(secao);
                await context.SaveChangesAsync();
            }

            using (var context = CreateContext(dbName))
            {
                var repo = new OficioseIntimacoesSecaoItemRepository(context);

                var novos = new[] { new OficioseIntimacoesSecaoItem { Ordem = 1, Descricao = "Should not be added" } };

                await repo.UpdateSecaoItensAsync(2, novos);

                var updated = await context.OficioseIntimacoesSecao
                    .Include(s => s.SecaoItem)
                    .FirstOrDefaultAsync(s => s.Id == 2);

                Assert.NotNull(updated);
                Assert.NotNull(updated.SecaoItem);
                Assert.Empty(updated.SecaoItem);
            }
        }
    }
}