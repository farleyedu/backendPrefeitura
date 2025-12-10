using Microsoft.EntityFrameworkCore;
using PortalTCMSP.Domain.Entities.ServicosEntity.OficioseIntimacoesEntity;
using PortalTCMSP.Infra.Data.Context;
using PortalTCMSP.Infra.Data.Repositories.Servicos.OficioseIntimacoes;

namespace PortalTCMSP.Infra.Data.Tests.Repositories.Servicos.OficioseIntimacoes
{
    public class OficioseIntimacoesSecaoRepositoryTests
    {
        private static PortalTCMSPContext CreateInMemoryContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<PortalTCMSPContext>()
                .UseInMemoryDatabase(dbName)
                .Options;
            return new PortalTCMSPContext(options);
        }

        [Fact]
        public async Task CreateSecoesAsync_AddsNewSecoes_WhenSecoesIsNull()
        {
            // Arrange
            var dbName = Guid.NewGuid().ToString();
            await using var context = CreateInMemoryContext(dbName);
            var root = new Domain.Entities.ServicosEntity.OficioseIntimacoesEntity.OficioseIntimacoes { Id = 1, Secoes = null };
            context.OficioseIntimacoes.Add(root);
            await context.SaveChangesAsync();

            var repository = new OficioseIntimacoesSecaoRepository(context);
            var novas = new[]
            {
                new OficioseIntimacoesSecao { Id = 10, Nome = "A" },
                new OficioseIntimacoesSecao { Id = 11, Nome = "B" }
            };

            // Act
            await repository.CreateSecoesAsync(1, novas);

            // Assert
            var entity = await context.OficioseIntimacoes.Include(o => o.Secoes).FirstAsync(o => o.Id == 1);
            Assert.NotNull(entity.Secoes);
            Assert.Equal(2, entity.Secoes.Count);
            Assert.Contains(entity.Secoes, s => s.Id == 10 && s.Nome == "A");
            Assert.Contains(entity.Secoes, s => s.Id == 11 && s.Nome == "B");
            Assert.Equal(EntityState.Modified, context.Entry(entity).State);
        }

        [Fact]
        public async Task CreateSecoesAsync_AppendsToExistingSecoes_WhenSecoesNotNull()
        {
            // Arrange
            var dbName = Guid.NewGuid().ToString();
            await using var context = CreateInMemoryContext(dbName);
            var existing = new OficioseIntimacoesSecao { Id = 1, Nome = "Existing" };
            var root = new Domain.Entities.ServicosEntity.OficioseIntimacoesEntity.OficioseIntimacoes { Id = 2, Secoes = new List<OficioseIntimacoesSecao> { existing } };
            context.OficioseIntimacoes.Add(root);
            await context.SaveChangesAsync();

            var repository = new OficioseIntimacoesSecaoRepository(context);
            var novas = new[]
            {
                new OficioseIntimacoesSecao { Id = 2, Nome = "New1" },
                new OficioseIntimacoesSecao { Id = 3, Nome = "New2" }
            };

            // Act
            await repository.CreateSecoesAsync(2, novas);

            // Assert
            var entity = await context.OficioseIntimacoes.Include(o => o.Secoes).FirstAsync(o => o.Id == 2);
            Assert.NotNull(entity.Secoes);
            Assert.Equal(3, entity.Secoes.Count);
            Assert.Contains(entity.Secoes, s => s.Id == 1 && s.Nome == "Existing");
            Assert.Contains(entity.Secoes, s => s.Id == 2 && s.Nome == "New1");
            Assert.Contains(entity.Secoes, s => s.Id == 3 && s.Nome == "New2");
            Assert.Equal(EntityState.Modified, context.Entry(entity).State);
        }

        [Fact]
        public async Task CreateSecoesAsync_ThrowsInvalidOperationException_WhenEntityNotFound()
        {
            // Arrange
            var dbName = Guid.NewGuid().ToString();
            await using var context = CreateInMemoryContext(dbName);
            var repository = new OficioseIntimacoesSecaoRepository(context);
            var novas = new[] { new OficioseIntimacoesSecao { Id = 100, Nome = "X" } };

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => repository.CreateSecoesAsync(9999, novas));
        }

        [Fact]
        public async Task UpdateSecoesAsync_InitializesSecoes_WhenSecoesIsNull()
        {
            // Arrange
            var dbName = Guid.NewGuid().ToString();
            await using var context = CreateInMemoryContext(dbName);
            var root = new Domain.Entities.ServicosEntity.OficioseIntimacoesEntity.OficioseIntimacoes { Id = 3, Secoes = null };
            context.OficioseIntimacoes.Add(root);
            await context.SaveChangesAsync();

            var repository = new OficioseIntimacoesSecaoRepository(context);
            var novas = new[] { new OficioseIntimacoesSecao { Id = 200, Nome = "ShouldBeIgnored" } };

            // Act
            await repository.UpdateSecoesAsync(3, novas);

            // Assert
            var entity = await context.OficioseIntimacoes.Include(o => o.Secoes).FirstAsync(o => o.Id == 3);
            Assert.NotNull(entity.Secoes);
            Assert.Empty(entity.Secoes); 
            Assert.Equal(EntityState.Modified, context.Entry(entity).State);
        }

        [Fact]
        public async Task UpdateSecoesAsync_DoesNotReplaceExistingSecoes_WithGivenImplementation()
        {
            // Arrange
            var dbName = Guid.NewGuid().ToString();
            await using var context = CreateInMemoryContext(dbName);
            var existing = new OficioseIntimacoesSecao { Id = 5, Nome = "KeepMe" };
            var root = new Domain.Entities.ServicosEntity.OficioseIntimacoesEntity.OficioseIntimacoes { Id = 4, Secoes = new List<OficioseIntimacoesSecao> { existing } };
            context.OficioseIntimacoes.Add(root);
            await context.SaveChangesAsync();

            var repository = new OficioseIntimacoesSecaoRepository(context);
            var novas = new[] { new OficioseIntimacoesSecao { Id = 6, Nome = "NewShouldNotReplace" } };

            // Act
            await repository.UpdateSecoesAsync(4, novas);

            // Assert
            var entity = await context.OficioseIntimacoes.Include(o => o.Secoes).FirstAsync(o => o.Id == 4);
            Assert.NotNull(entity.Secoes);
            Assert.Single(entity.Secoes);
            Assert.Equal(5, entity.Secoes.First().Id);
            Assert.Equal(EntityState.Modified, context.Entry(entity).State);
        }
    }
}