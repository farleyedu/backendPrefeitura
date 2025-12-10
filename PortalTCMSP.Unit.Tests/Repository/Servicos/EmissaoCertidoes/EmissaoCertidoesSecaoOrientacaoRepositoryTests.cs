using Microsoft.EntityFrameworkCore;
using PortalTCMSP.Domain.Entities.ServicosEntity.EmissaoCertidoesEntity;
using PortalTCMSP.Infra.Data.Context;
using PortalTCMSP.Infra.Data.Repositories.Servicos.EmissaoCertidoes;

namespace PortalTCMSP.Infra.Data.Tests.Repositories.Servicos.EmissaoCertidoes
{
    public class EmissaoCertidoesSecaoOrientacaoRepositoryTests
    {
        private static PortalTCMSPContext CreateContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<PortalTCMSPContext>()
                .UseInMemoryDatabase(dbName)
                .Options;
            return new PortalTCMSPContext(options);
        }

        [Fact]
        public async Task CreateScoesAsync_Throws_When_Emissao_NotFound()
        {
            using var context = CreateContext(nameof(CreateScoesAsync_Throws_When_Emissao_NotFound));
            var repo = new EmissaoCertidoesSecaoOrientacaoRepository(context);

            var novos = new[] { new EmissaoCertidoesSecaoOrientacao { TituloPagina = "T" } };

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => repo.CreateScoesAsync(9999, novos));
            Assert.Contains("EmissaoCertidoes 9999", ex.Message);
        }

        [Fact]
        public async Task CreateScoesAsync_Adds_New_When_SecaoOrientacoes_Null()
        {
            const long id = 1;
            using var context = CreateContext(nameof(CreateScoesAsync_Adds_New_When_SecaoOrientacoes_Null));
            var emissao = new Domain.Entities.ServicosEntity.EmissaoCertidoesEntity.EmissaoCertidoes { Id = id, SecaoOrientacoes = null };
            context.EmissaoCertidoes.Add(emissao);
            await context.SaveChangesAsync();

            var repo = new EmissaoCertidoesSecaoOrientacaoRepository(context);
            var novos = new[]
            {
                new EmissaoCertidoesSecaoOrientacao { TituloPagina = "New 1" },
                new EmissaoCertidoesSecaoOrientacao { TituloPagina = "New 2" }
            };

            await repo.CreateScoesAsync(id, novos);
            await context.SaveChangesAsync();

            var loaded = await context.EmissaoCertidoes
                .Include(e => e.SecaoOrientacoes)
                .FirstAsync(e => e.Id == id);

            Assert.NotNull(loaded.SecaoOrientacoes);
            Assert.Equal(2, loaded.SecaoOrientacoes.Count);
            Assert.Contains(loaded.SecaoOrientacoes, s => s.TituloPagina == "New 1");
        }

        [Fact]
        public async Task CreateScoesAsync_Appends_When_SecaoOrientacoes_Exists()
        {
            const long id = 2;
            using var context = CreateContext(nameof(CreateScoesAsync_Appends_When_SecaoOrientacoes_Exists));
            var emissao = new Domain.Entities.ServicosEntity.EmissaoCertidoesEntity.EmissaoCertidoes
            {
                Id = id,
                SecaoOrientacoes = new List<EmissaoCertidoesSecaoOrientacao>
                {
                    new EmissaoCertidoesSecaoOrientacao { TituloPagina = "Existing" }
                }
            };
            context.EmissaoCertidoes.Add(emissao);
            await context.SaveChangesAsync();

            var repo = new EmissaoCertidoesSecaoOrientacaoRepository(context);
            var novos = new[]
            {
                new EmissaoCertidoesSecaoOrientacao { TituloPagina = "Appended" }
            };

            await repo.CreateScoesAsync(id, novos);
            await context.SaveChangesAsync();

            var loaded = await context.EmissaoCertidoes
                .Include(e => e.SecaoOrientacoes)
                .FirstAsync(e => e.Id == id);

            Assert.Equal(2, loaded.SecaoOrientacoes.Count);
            Assert.Contains(loaded.SecaoOrientacoes, s => s.TituloPagina == "Existing");
            Assert.Contains(loaded.SecaoOrientacoes, s => s.TituloPagina == "Appended");
        }

        [Fact]
        public async Task UpdateSecoesAsync_Throws_When_Emissao_NotFound()
        {
            using var context = CreateContext(nameof(UpdateSecoesAsync_Throws_When_Emissao_NotFound));
            var repo = new EmissaoCertidoesSecaoOrientacaoRepository(context);

            var novos = new[] { new EmissaoCertidoesSecaoOrientacao { TituloPagina = "T" } };

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => repo.UpdateSecoesAsync(5555, novos));
            Assert.Contains("EmissaoCertidoes 5555", ex.Message);
        }

        [Fact]
        public async Task UpdateSecoesAsync_Initializes_To_Empty_When_Null_And_Does_Not_Add_Novos()
        {
            const long id = 3;
            using var context = CreateContext(nameof(UpdateSecoesAsync_Initializes_To_Empty_When_Null_And_Does_Not_Add_Novos));
            var emissao = new Domain.Entities.ServicosEntity.EmissaoCertidoesEntity.EmissaoCertidoes { Id = id, SecaoOrientacoes = null };
            context.EmissaoCertidoes.Add(emissao);
            await context.SaveChangesAsync();

            var repo = new EmissaoCertidoesSecaoOrientacaoRepository(context);
            var novos = new[] { new EmissaoCertidoesSecaoOrientacao { TituloPagina = "ShouldNotBeAdded" } };

            await repo.UpdateSecoesAsync(id, novos);
            await context.SaveChangesAsync();

            var loaded = await context.EmissaoCertidoes
                .Include(e => e.SecaoOrientacoes)
                .FirstAsync(e => e.Id == id);

            Assert.NotNull(loaded.SecaoOrientacoes);
            Assert.Empty(loaded.SecaoOrientacoes);
        }

        [Fact]
        public async Task UpdateSecoesAsync_Does_Not_Modify_When_Collection_Exists()
        {
            const long id = 4;
            using var context = CreateContext(nameof(UpdateSecoesAsync_Does_Not_Modify_When_Collection_Exists));
            var emissao = new Domain.Entities.ServicosEntity.EmissaoCertidoesEntity.EmissaoCertidoes
            {
                Id = id,
                SecaoOrientacoes = new List<EmissaoCertidoesSecaoOrientacao>
                {
                    new EmissaoCertidoesSecaoOrientacao { TituloPagina = "ExistingKeep" }
                }
            };
            context.EmissaoCertidoes.Add(emissao);
            await context.SaveChangesAsync();

            var repo = new EmissaoCertidoesSecaoOrientacaoRepository(context);
            var novos = new[] { new EmissaoCertidoesSecaoOrientacao { TituloPagina = "NewShouldNotBeAdded" } };

            await repo.UpdateSecoesAsync(id, novos);
            await context.SaveChangesAsync();

            var loaded = await context.EmissaoCertidoes
                .Include(e => e.SecaoOrientacoes)
                .FirstAsync(e => e.Id == id);

            Assert.Single(loaded.SecaoOrientacoes);
            Assert.Contains(loaded.SecaoOrientacoes, s => s.TituloPagina == "ExistingKeep");
            Assert.DoesNotContain(loaded.SecaoOrientacoes, s => s.TituloPagina == "NewShouldNotBeAdded");
        }
    }
}