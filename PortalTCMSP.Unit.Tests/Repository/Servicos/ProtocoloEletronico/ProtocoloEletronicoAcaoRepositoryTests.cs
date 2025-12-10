using Microsoft.EntityFrameworkCore;
using PortalTCMSP.Domain.Entities.ServicosEntity.ProtocoloEletronicoEntity;
using PortalTCMSP.Infra.Data.Context;
using PortalTCMSP.Infra.Data.Repositories.Servicos.ProtocoloEletronico;

namespace PortalTCMSP.Tests.Infra.Data.Repositories.Servicos.ProtocoloEletronico
{
    public class ProtocoloEletronicoAcaoRepositoryTests
    {
        private static PortalTCMSPContext CreateContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<PortalTCMSPContext>()
                .UseInMemoryDatabase(dbName)
                .Options;
            return new PortalTCMSPContext(options);
        }

        [Fact]
        public async Task CreateAcoesAsync_AddsActionsToExistingProtocol()
        {
            var dbName = Guid.NewGuid().ToString();
            await using (var context = CreateContext(dbName))
            {
                var protocolo = new Domain.Entities.ServicosEntity.ProtocoloEletronicoEntity.ProtocoloEletronico { Id = 1, Acoes = null };
                context.ProtocoloEletronico.Add(protocolo);
                await context.SaveChangesAsync();
            }

            await using (var context = CreateContext(dbName))
            {
                // Act
                var repo = new ProtocoloEletronicoAcaoRepository(context);
                var novas = new[]
                {
                    new ProtocoloEletronicoAcao { IdProtocoloEletronico = 1, UrlAcao = "https://a" },
                    new ProtocoloEletronicoAcao { IdProtocoloEletronico = 1, UrlAcao = "https://b" }
                };
                await repo.CreateAcoesAsync(1, novas);

                var persisted = await context.ProtocoloEletronico.Include(p => p.Acoes).FirstAsync(p => p.Id == 1);
                Assert.NotNull(persisted.Acoes);
                Assert.Equal(2, persisted.Acoes.Count);
                Assert.Contains(persisted.Acoes, a => a.UrlAcao == "https://a");
                Assert.Contains(persisted.Acoes, a => a.UrlAcao == "https://b");
            }
        }

        [Fact]
        public async Task CreateAcoesAsync_ThrowsWhenProtocolNotFound()
        {
            await using var context = CreateContext(Guid.NewGuid().ToString());
            var repo = new ProtocoloEletronicoAcaoRepository(context);
            var novas = new[] { new ProtocoloEletronicoAcao { IdProtocoloEletronico = 999, UrlAcao = "x" } };

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => repo.CreateAcoesAsync(999, novas));
            Assert.Equal("ProtocoloEletronico 999 não encontrado.", ex.Message);
        }

        [Fact]
        public async Task UpdateAcoesAsync_InitializesAcoesWhenNullAndDoesNotAddNewItems()
        {
            var dbName = Guid.NewGuid().ToString();
            await using (var context = CreateContext(dbName))
            {
                var protocolo = new Domain.Entities.ServicosEntity.ProtocoloEletronicoEntity.ProtocoloEletronico { Id = 2, Acoes = null };
                context.ProtocoloEletronico.Add(protocolo);
                await context.SaveChangesAsync();
            }

            await using (var context = CreateContext(dbName))
            {
                // Act
                var repo = new ProtocoloEletronicoAcaoRepository(context);
                var novas = new[]
                {
                    new ProtocoloEletronicoAcao { IdProtocoloEletronico = 2, UrlAcao = "should-not-be-added" }
                };
                await repo.UpdateAcoesAsync(2, novas);

                var persisted = await context.ProtocoloEletronico.Include(p => p.Acoes).FirstAsync(p => p.Id == 2);
                Assert.NotNull(persisted.Acoes);
                Assert.Empty(persisted.Acoes);
            }
        }

        [Fact]
        public async Task UpdateAcoesAsync_ThrowsWhenProtocolNotFound()
        {
            await using var context = CreateContext(Guid.NewGuid().ToString());
            var repo = new ProtocoloEletronicoAcaoRepository(context);
            var novas = Array.Empty<ProtocoloEletronicoAcao>();

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => repo.UpdateAcoesAsync(12345, novas));
            Assert.Equal("ProtocoloEletronico 12345 não encontrado.", ex.Message);
        }
    }
}