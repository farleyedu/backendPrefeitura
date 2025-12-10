using Microsoft.EntityFrameworkCore;
using PortalTCMSP.Domain.Entities.ServicosEntity.EmissaoCertidoesEntity;
using PortalTCMSP.Infra.Data.Context;
using PortalTCMSP.Infra.Data.Repositories.Servicos.EmissaoCertidoes;

namespace PortalTCMSP.Infra.Data.Tests.Repositories.Servicos.EmissaoCertidoes
{
    public class EmissaoCertidoesAcaoRepositoryTests
    {
        private static PortalTCMSPContext CreateContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<PortalTCMSPContext>()
                .UseInMemoryDatabase(dbName)
                .Options;
            return new PortalTCMSPContext(options);
        }

        [Fact]
        public async Task CreateAcoesAsync_AddsNewActions_WhenEmissaoExistsAndAcoesNull()
        {
            var context = CreateContext(nameof(CreateAcoesAsync_AddsNewActions_WhenEmissaoExistsAndAcoesNull));
            var emissao = new Domain.Entities.ServicosEntity.EmissaoCertidoesEntity.EmissaoCertidoes { Id = 1, Acoes = null };
            context.EmissaoCertidoes.Add(emissao);
            await context.SaveChangesAsync();

            var repo = new EmissaoCertidoesAcaoRepository(context);
            var novos = new[]
            {
                new EmissaoCertidoesAcao { IdEmissaoCertidoes = 1, Titulo = "Titulo A" }
            };

            await repo.CreateAcoesAsync(1, novos);

            var loaded = await context.EmissaoCertidoes.Include(e => e.Acoes).FirstAsync(e => e.Id == 1);
            Assert.NotNull(loaded.Acoes);
            Assert.Single(loaded.Acoes);
            Assert.Equal("Titulo A", loaded.Acoes.First().Titulo);
        }

        [Fact]
        public async Task CreateAcoesAsync_Throws_WhenEmissaoNotFound()
        {
            var context = CreateContext(nameof(CreateAcoesAsync_Throws_WhenEmissaoNotFound));
            var repo = new EmissaoCertidoesAcaoRepository(context);

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                repo.CreateAcoesAsync(999, Array.Empty<EmissaoCertidoesAcao>()));

            Assert.Contains("EmissaoCertidoes 999", ex.Message);
        }

        [Fact]
        public async Task UpdateAcoesAsync_DoesNotAddNovos_WhenMethodDoesNotApplyThem()
        {
            var context = CreateContext(nameof(UpdateAcoesAsync_DoesNotAddNovos_WhenMethodDoesNotApplyThem));
            var emissao = new Domain.Entities.ServicosEntity.EmissaoCertidoesEntity.EmissaoCertidoes
            {
                Id = 2,
                Acoes = new List<EmissaoCertidoesAcao>
                {
                    new EmissaoCertidoesAcao { IdEmissaoCertidoes = 2, Titulo = "Original" }
                }
            };
            context.EmissaoCertidoes.Add(emissao);
            await context.SaveChangesAsync();

            var repo = new EmissaoCertidoesAcaoRepository(context);
            var novos = new[]
            {
                new EmissaoCertidoesAcao { IdEmissaoCertidoes = 2, Titulo = "Novo" }
            };

            await repo.UpdateAcoesAsync(2, novos);

            var loaded = await context.EmissaoCertidoes.Include(e => e.Acoes).FirstAsync(e => e.Id == 2);
            Assert.Single(loaded.Acoes);
            Assert.Equal("Original", loaded.Acoes.First().Titulo);
        }

        [Fact]
        public async Task UpdateAcoesAsync_InitializesAcoes_WhenNull()
        {
            var context = CreateContext(nameof(UpdateAcoesAsync_InitializesAcoes_WhenNull));
            var emissao = new Domain.Entities.ServicosEntity.EmissaoCertidoesEntity.EmissaoCertidoes { Id = 3, Acoes = null };
            context.EmissaoCertidoes.Add(emissao);
            await context.SaveChangesAsync();

            var repo = new EmissaoCertidoesAcaoRepository(context);
            await repo.UpdateAcoesAsync(3, Array.Empty<EmissaoCertidoesAcao>());

            var loaded = await context.EmissaoCertidoes.Include(e => e.Acoes).FirstAsync(e => e.Id == 3);
            Assert.NotNull(loaded.Acoes);
            Assert.Empty(loaded.Acoes);
        }

        [Fact]
        public async Task UpdateAcoesAsync_Throws_WhenEmissaoNotFound()
        {
            var context = CreateContext(nameof(UpdateAcoesAsync_Throws_WhenEmissaoNotFound));
            var repo = new EmissaoCertidoesAcaoRepository(context);

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                repo.UpdateAcoesAsync(555, Array.Empty<EmissaoCertidoesAcao>()));

            Assert.Contains("EmissaoCertidoes 555", ex.Message);
        }
    }
}