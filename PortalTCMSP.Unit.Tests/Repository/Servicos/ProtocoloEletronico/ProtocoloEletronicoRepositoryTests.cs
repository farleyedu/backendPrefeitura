using Microsoft.EntityFrameworkCore;
using PortalTCMSP.Domain.Entities.ServicosEntity.ProtocoloEletronicoEntity;
using PortalTCMSP.Infra.Data.Context;
using PortalTCMSP.Infra.Data.Repositories.Servicos.ProtocoloEletronico;

namespace PortalTCMSP.Unit.Tests.Data.Repositories.Servicos.ProtocoloEletronico
{
    public class ProtocoloEletronicoRepositoryTests
    {
        private static PortalTCMSPContext CreateContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<PortalTCMSPContext>()
                .UseInMemoryDatabase(dbName)
                .Options;
            return new PortalTCMSPContext(options);
        }

        [Fact]
        public async Task GetWithChildrenByIdAsync_ReturnsEntityWithAcoes()
        {
            var dbName = Guid.NewGuid().ToString();
            await using var ctx = CreateContext(dbName);
            var entity = new Domain.Entities.ServicosEntity.ProtocoloEletronicoEntity.ProtocoloEletronico
            {
                Id = 1,
                Slug = "s-1",
                Ativo = true,
                Acoes = new List<ProtocoloEletronicoAcao>
                {
                    new ProtocoloEletronicoAcao { Id = 10, IdProtocoloEletronico = 1 }
                }
            };
            ctx.ProtocoloEletronico.Add(entity);
            await ctx.SaveChangesAsync();

            var repo = new ProtocoloEletronicoRepository(ctx);
            var result = await repo.GetWithChildrenByIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal(1, result!.Id);
            Assert.NotNull(result.Acoes);
            Assert.Single(result.Acoes);
            Assert.Equal(10, result.Acoes.First().Id);
        }

        [Fact]
        public async Task AllWithChildrenAsync_ReturnsAllEntitiesWithAcoes()
        {
            var dbName = Guid.NewGuid().ToString();
            await using var ctx = CreateContext(dbName);
            var e1 = new Domain.Entities.ServicosEntity.ProtocoloEletronicoEntity.ProtocoloEletronico
            {
                Id = 1,
                Slug = "s-1",
                Ativo = true,
                Acoes = new List<ProtocoloEletronicoAcao> { new ProtocoloEletronicoAcao { Id = 11, IdProtocoloEletronico = 1 } }
            };
            var e2 = new Domain.Entities.ServicosEntity.ProtocoloEletronicoEntity.ProtocoloEletronico
            {
                Id = 2,
                Slug = "s-2",
                Ativo = true,
                Acoes = new List<ProtocoloEletronicoAcao> { new ProtocoloEletronicoAcao { Id = 12, IdProtocoloEletronico = 2 } }
            };
            ctx.ProtocoloEletronico.AddRange(e1, e2);
            await ctx.SaveChangesAsync();

            var repo = new ProtocoloEletronicoRepository(ctx);
            var list = await repo.AllWithChildrenAsync();

            Assert.NotNull(list);
            Assert.Equal(2, list.Count);
            Assert.All(list, p => Assert.NotNull(p.Acoes));
            Assert.Contains(list, p => p.Id == 1 && p.Acoes.Any(a => a.Id == 11));
            Assert.Contains(list, p => p.Id == 2 && p.Acoes.Any(a => a.Id == 12));
        }

        [Fact]
        public async Task DisableAsync_ReturnsTrueAndSetsAtivoFalse_WhenEntityExists()
        {
            var dbName = Guid.NewGuid().ToString();
            await using var ctx = CreateContext(dbName);
            var entity = new Domain.Entities.ServicosEntity.ProtocoloEletronicoEntity.ProtocoloEletronico { Id = 5, Slug = "disable-me", Ativo = true };
            ctx.ProtocoloEletronico.Add(entity);
            await ctx.SaveChangesAsync();

            var repo = new ProtocoloEletronicoRepository(ctx);
            var result = await repo.DisableAsync(5);

            Assert.True(result);

            var reloaded = await ctx.ProtocoloEletronico.FindAsync(5L);
            Assert.NotNull(reloaded);
            Assert.False(reloaded!.Ativo);
        }

        [Fact]
        public async Task DisableAsync_ReturnsFalse_WhenEntityDoesNotExist()
        {
            var dbName = Guid.NewGuid().ToString();
            await using var ctx = CreateContext(dbName);

            var repo = new ProtocoloEletronicoRepository(ctx);
            var result = await repo.DisableAsync(999);

            Assert.False(result);
        }

        [Fact]
        public async Task GetWithChildrenBySlugAtivoAsync_ReturnsEntity_WhenSlugMatchesAndAtivo()
        {
            var dbName = Guid.NewGuid().ToString();
            await using var ctx = CreateContext(dbName);
            var entity = new Domain.Entities.ServicosEntity.ProtocoloEletronicoEntity.ProtocoloEletronico
            {
                Id = 20,
                Slug = "active-slug",
                Ativo = true,
                Acoes = new List<ProtocoloEletronicoAcao> { new ProtocoloEletronicoAcao { Id = 21, IdProtocoloEletronico = 20 } }
            };
            ctx.ProtocoloEletronico.Add(entity);
            await ctx.SaveChangesAsync();

            var repo = new ProtocoloEletronicoRepository(ctx);
            var result = await repo.GetWithChildrenBySlugAtivoAsync("active-slug");

            Assert.NotNull(result);
            Assert.Equal(20, result!.Id);
            Assert.Single(result.Acoes);
        }

        [Fact]
        public async Task GetWithChildrenBySlugAtivoAsync_ReturnsNull_WhenNotActiveOrMissing()
        {
            var dbName = Guid.NewGuid().ToString();
            await using var ctx = CreateContext(dbName);
            ctx.ProtocoloEletronico.Add(new Domain.Entities.ServicosEntity.ProtocoloEletronicoEntity.ProtocoloEletronico { Id = 30, Slug = "inactive", Ativo = false });
            await ctx.SaveChangesAsync();

            var repo = new ProtocoloEletronicoRepository(ctx);
            var result1 = await repo.GetWithChildrenBySlugAtivoAsync("inactive");
            var result2 = await repo.GetWithChildrenBySlugAtivoAsync("does-not-exist");

            Assert.Null(result1);
            Assert.Null(result2);
        }

        [Fact]
        public async Task ReplaceAcoesAsync_ReplacesOldAcoesWithNew_OnSaveChanges()
        {
            var dbName = Guid.NewGuid().ToString();
            await using var ctx = CreateContext(dbName);

            var pe = new Domain.Entities.ServicosEntity.ProtocoloEletronicoEntity.ProtocoloEletronico
            {
                Id = 40,
                Slug = "r-a",
                Ativo = true
            };
            ctx.ProtocoloEletronico.Add(pe);
            await ctx.SaveChangesAsync();

            // initial actions
            var initial = new[]
            {
                new ProtocoloEletronicoAcao { Id = 100, IdProtocoloEletronico = 40 },
                new ProtocoloEletronicoAcao { Id = 101, IdProtocoloEletronico = 40 }
            };
            ctx.ProtocoloEletronicoAcao.AddRange(initial);
            await ctx.SaveChangesAsync();

            var repo = new ProtocoloEletronicoRepository(ctx);

            var novas = new List<ProtocoloEletronicoAcao>
            {
                new ProtocoloEletronicoAcao { IdProtocoloEletronico = 0, Ordem = 1 },
                new ProtocoloEletronicoAcao { IdProtocoloEletronico = 0, Ordem = 2 }
            };

            await repo.ReplaceAcoesAsync(40, novas);
            await ctx.SaveChangesAsync();

            var remained = await ctx.ProtocoloEletronicoAcao.Where(a => a.IdProtocoloEletronico == 40).ToListAsync();

            Assert.Equal(2, remained.Count);
            Assert.All(remained, a => Assert.Equal(40, a.IdProtocoloEletronico));
            Assert.Contains(remained, a => a.Ordem == 1);
            Assert.Contains(remained, a => a.Ordem == 2);
        }

        [Fact]
        public async Task GetBySlugAtivoAsync_ReturnsEntity_WhenActive()
        {
            var dbName = Guid.NewGuid().ToString();
            await using var ctx = CreateContext(dbName);
            ctx.ProtocoloEletronico.Add(new Domain.Entities.ServicosEntity.ProtocoloEletronicoEntity.ProtocoloEletronico { Id = 50, Slug = "by-slug", Ativo = true });
            await ctx.SaveChangesAsync();

            var repo = new ProtocoloEletronicoRepository(ctx);
            var result = await repo.GetBySlugAtivoAsync("by-slug");

            Assert.NotNull(result);
            Assert.Equal(50, result!.Id);
        }

        [Fact]
        public async Task GetBySlugAtivoAsync_ReturnsNull_WhenInactiveOrMissing()
        {
            var dbName = Guid.NewGuid().ToString();
            await using var ctx = CreateContext(dbName);
            ctx.ProtocoloEletronico.Add(new Domain.Entities.ServicosEntity.ProtocoloEletronicoEntity.ProtocoloEletronico { Id = 51, Slug = "by-slug-inactive", Ativo = false });
            await ctx.SaveChangesAsync();

            var repo = new ProtocoloEletronicoRepository(ctx);
            var result = await repo.GetBySlugAtivoAsync("by-slug-inactive");

            Assert.Null(result);
        }
    }
}