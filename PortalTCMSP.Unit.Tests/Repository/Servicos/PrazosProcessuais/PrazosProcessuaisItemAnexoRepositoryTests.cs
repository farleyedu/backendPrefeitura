using Microsoft.EntityFrameworkCore;
using PortalTCMSP.Domain.Entities.ServicosEntity.PrazosProcessuaisEntity;
using PortalTCMSP.Infra.Data.Context;
using PortalTCMSP.Infra.Data.Repositories.Servicos.PrazosProcessuais;

namespace PortalTCMSP.Tests.Repositories.Servicos.PrazosProcessuais
{
    public class PrazosProcessuaisItemAnexoRepositoryTests
    {
        private static PortalTCMSPContext CreateContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<PortalTCMSPContext>()
                .UseInMemoryDatabase(dbName)
                .Options;

            return new PortalTCMSPContext(options);
        }

        [Fact]
        public async Task CreateAnexoAsync_AddsNewAnexosAndKeepsExisting()
        {
            // Arrange
            var dbName = Guid.NewGuid().ToString();
            await using var context = CreateContext(dbName);

            var item = new PrazosProcessuaisItem
            {
                Id = 1,
                Nome = "Item 1",
                Anexos = new List<PrazosProcessuaisItemAnexo>()
            };

            context.PrazosProcessuaisItem.Add(item);
            await context.SaveChangesAsync();

            var novos = new[]
            {
                new PrazosProcessuaisItemAnexo
                {
                    Id = 100,
                    NomeArquivo = "file1.pdf",
                    Url = "/files/file1.pdf",
                    Tipo = "pdf",
                    Ativo = true,
                    Ordem = 1,
                    IdPrazosProcessuaisItem = 1
                },
                new PrazosProcessuaisItemAnexo
                {
                    Id = 101,
                    NomeArquivo = "file2.pdf",
                    Url = "/files/file2.pdf",
                    Tipo = "pdf",
                    Ativo = true,
                    Ordem = 2,
                    IdPrazosProcessuaisItem = 1
                }
            };

            var repo = new PrazosProcessuaisItemAnexoRepository(context);

            // Act
            await repo.CreateAnexoAsync(1, novos);

            // Assert
            var persisted = await context.PrazosProcessuaisItem
                .Include(i => i.Anexos)
                .FirstOrDefaultAsync(i => i.Id == 1);

            Assert.NotNull(persisted);
            Assert.Equal(2, persisted.Anexos.Count);
            Assert.Contains(persisted.Anexos, a => a.NomeArquivo == "file1.pdf" && a.Url == "/files/file1.pdf");
            Assert.Contains(persisted.Anexos, a => a.NomeArquivo == "file2.pdf" && a.Url == "/files/file2.pdf");
        }

        [Fact]
        public async Task CreateAnexoAsync_ThrowsWhenItemNotFound()
        {
            // Arrange
            var dbName = Guid.NewGuid().ToString();
            await using var context = CreateContext(dbName);
            var repo = new PrazosProcessuaisItemAnexoRepository(context);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                repo.CreateAnexoAsync(999, Array.Empty<PrazosProcessuaisItemAnexo>()));

            Assert.Contains("PrazosProcessuaisItem with Id 999 not found", ex.Message);
        }

        [Fact]
        public async Task UpdateAnexoAsync_ReplacesExistingAnexoAndIgnoresNonExistingIds()
        {
            // Arrange
            var dbName = Guid.NewGuid().ToString();
            await using var context = CreateContext(dbName);

            var existingAnexo = new PrazosProcessuaisItemAnexo
            {
                Id = 200,
                NomeArquivo = "old.pdf",
                Url = "/old.pdf",
                Tipo = "pdf",
                Ativo = true,
                Ordem = 1,
                IdPrazosProcessuaisItem = 2
            };

            var item = new PrazosProcessuaisItem
            {
                Id = 2,
                Nome = "Item 2",
                Anexos = new List<PrazosProcessuaisItemAnexo> { existingAnexo }
            };

            context.PrazosProcessuaisItem.Add(item);
            await context.SaveChangesAsync();

            var novoMatching = new PrazosProcessuaisItemAnexo
            {
                Id = 200,
                NomeArquivo = "new.pdf",
                Url = "/new.pdf",
                Tipo = "pdf",
                Ativo = false,
                Ordem = 5,
                IdPrazosProcessuaisItem = 2
            };

            var novoNonExisting = new PrazosProcessuaisItemAnexo
            {
                Id = 999,
                NomeArquivo = "should-be-ignored.pdf",
                Url = "/ignored.pdf",
                Tipo = "pdf",
                Ativo = true,
                Ordem = 9,
                IdPrazosProcessuaisItem = 2
            };

            var repo = new PrazosProcessuaisItemAnexoRepository(context);

            // Act
            await repo.UpdateAnexoAsync(2, new[] { novoMatching, novoNonExisting });

            // Assert
            var persisted = await context.PrazosProcessuaisItem
                .Include(i => i.Anexos)
                .FirstOrDefaultAsync(i => i.Id == 2);

            Assert.NotNull(persisted);
            Assert.Single(persisted.Anexos);

            var anexo = persisted.Anexos.Single();
            Assert.Equal(200, anexo.Id);
            Assert.Equal("new.pdf", anexo.NomeArquivo);
            Assert.Equal("/new.pdf", anexo.Url);
            Assert.Equal(5, anexo.Ordem);
            Assert.False(anexo.Ativo);
        }

        [Fact]
        public async Task UpdateAnexoAsync_ThrowsWhenItemNotFound()
        {
            // Arrange
            var dbName = Guid.NewGuid().ToString();
            await using var context = CreateContext(dbName);
            var repo = new PrazosProcessuaisItemAnexoRepository(context);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                repo.UpdateAnexoAsync(12345, new[] { new PrazosProcessuaisItemAnexo { Id = 1 } }));

            Assert.Contains("PrazosProcessuaisItem with Id 12345 not found", ex.Message);
        }
    }
}