using Microsoft.EntityFrameworkCore;
using PortalTCMSP.Domain.Entities.ServicosEntity.MultasProcedimentosEntity;
using PortalTCMSP.Infra.Data.Context;
using PortalTCMSP.Infra.Data.Repositories.Servicos.MultasProcedimentos;

namespace PortalTCMSP.Tests.Infra.Data.Repositories.Servicos.MultasProcedimentos
{
    public class MultasProcedimentosProcedimentoRepositoryTests
    {
        private static PortalTCMSPContext CreateContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<PortalTCMSPContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;
            return new PortalTCMSPContext(options);
        }

        [Fact]
        public async Task CreateProcedimentosAsync_AppendsNewProcedimentos_WhenEntityExists_AndProcedimentosWasNull()
        {
            // Arrange
            var dbName = nameof(CreateProcedimentosAsync_AppendsNewProcedimentos_WhenEntityExists_AndProcedimentosWasNull);
            await using var ctx = CreateContext(dbName);
            var mp = new Domain.Entities.ServicosEntity.MultasProcedimentosEntity.MultasProcedimentos { Id = 1, Procedimentos = null };
            ctx.MultasProcedimentos.Add(mp);
            await ctx.SaveChangesAsync();

            var repo = new MultasProcedimentosProcedimentoRepository(ctx);
            var novos = new[]
            {
                new MultasProcedimentosProcedimento { IdMultasProcedimentos = 1, Texto = "novo1", Ordem = 1 }
            };

            // Act
            await repo.CreateProcedimentosAsync(1, novos);

            var loaded = await ctx.MultasProcedimentos
                .Include(x => x.Procedimentos)
                .FirstOrDefaultAsync(x => x.Id == 1);

            Assert.NotNull(loaded);
            Assert.NotNull(loaded.Procedimentos);
            Assert.Single(loaded.Procedimentos);
            Assert.Equal("novo1", loaded.Procedimentos.First().Texto);
        }

        [Fact]
        public async Task CreateProcedimentosAsync_Throws_WhenEntityNotFound()
        {
            // Arrange
            var dbName = nameof(CreateProcedimentosAsync_Throws_WhenEntityNotFound);
            await using var ctx = CreateContext(dbName);
            var repo = new MultasProcedimentosProcedimentoRepository(ctx);
            var novos = Array.Empty<MultasProcedimentosProcedimento>();

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => repo.CreateProcedimentosAsync(999, novos));
        }

        [Fact]
        public async Task UpdateProcedimentosAsync_DoesNotReplaceExistingProcedimentos_WhenImplementationIsNoop()
        {
            // Arrange
            var dbName = nameof(UpdateProcedimentosAsync_DoesNotReplaceExistingProcedimentos_WhenImplementationIsNoop);
            await using var ctx = CreateContext(dbName);
            var existingProc = new MultasProcedimentosProcedimento { IdMultasProcedimentos = 2, Texto = "original", Ordem = 1 };
            var mp = new Domain.Entities.ServicosEntity.MultasProcedimentosEntity.MultasProcedimentos { Id = 2, Procedimentos = new List<MultasProcedimentosProcedimento> { existingProc } };
            ctx.MultasProcedimentos.Add(mp);
            await ctx.SaveChangesAsync();

            var repo = new MultasProcedimentosProcedimentoRepository(ctx);
            var novos = new[]
            {
                new MultasProcedimentosProcedimento { IdMultasProcedimentos = 2, Texto = "novo", Ordem = 2 }
            };

            // Act
            await repo.UpdateProcedimentosAsync(2, novos);

            var loaded = await ctx.MultasProcedimentos
                .Include(x => x.Procedimentos)
                .FirstOrDefaultAsync(x => x.Id == 2);

            Assert.NotNull(loaded);
            Assert.NotNull(loaded.Procedimentos);
            Assert.Single(loaded.Procedimentos);
            Assert.Equal("original", loaded.Procedimentos.First().Texto);
            Assert.DoesNotContain(loaded.Procedimentos, p => p.Texto == "novo");
        }

        [Fact]
        public async Task UpdateProcedimentosAsync_SetsProcedimentosToEmptyList_WhenProcedimentosWasNull()
        {
            // Arrange
            var dbName = nameof(UpdateProcedimentosAsync_SetsProcedimentosToEmptyList_WhenProcedimentosWasNull);
            await using var ctx = CreateContext(dbName);
            var mp = new Domain.Entities.ServicosEntity.MultasProcedimentosEntity.MultasProcedimentos { Id = 3, Procedimentos = null };
            ctx.MultasProcedimentos.Add(mp);
            await ctx.SaveChangesAsync();

            var repo = new MultasProcedimentosProcedimentoRepository(ctx);
            var novos = Array.Empty<MultasProcedimentosProcedimento>();

            // Act
            await repo.UpdateProcedimentosAsync(3, novos);

            var loaded = await ctx.MultasProcedimentos
                .Include(x => x.Procedimentos)
                .FirstOrDefaultAsync(x => x.Id == 3);

            Assert.NotNull(loaded);
            Assert.NotNull(loaded.Procedimentos);
            Assert.Empty(loaded.Procedimentos);
        }

        [Fact]
        public async Task UpdateProcedimentosAsync_Throws_WhenEntityNotFound()
        {
            // Arrange
            var dbName = nameof(UpdateProcedimentosAsync_Throws_WhenEntityNotFound);
            await using var ctx = CreateContext(dbName);
            var repo = new MultasProcedimentosProcedimentoRepository(ctx);
            var novos = Array.Empty<MultasProcedimentosProcedimento>();

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => repo.UpdateProcedimentosAsync(9999, novos));
        }
    }
}