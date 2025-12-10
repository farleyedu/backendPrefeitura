using Microsoft.EntityFrameworkCore;
using PortalTCMSP.Domain.Entities.ServicosEntity.MultasProcedimentosEntity;
using PortalTCMSP.Infra.Data.Context;
using PortalTCMSP.Infra.Data.Repositories.Servicos.MultasProcedimentos;

namespace PortalTCMSP.Tests.Repositories
{
    public class MultasProcedimentosRepositoryTests
    {
        private static PortalTCMSPContext CreateContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<PortalTCMSPContext>()
                .UseInMemoryDatabase(dbName)
                .Options;
            return new PortalTCMSPContext(options);
        }

        [Fact]
        public async Task GetWithChildrenByIdAsync_ReturnsEntityWithChildren()
        {
            var dbName = Guid.NewGuid().ToString();
            await using var context = CreateContext(dbName);
            var mp = new MultasProcedimentos
            {
                Slug = "mp-1",
                Ativo = true,
                DataCriacao = DateTime.UtcNow,
                Procedimentos = new List<MultasProcedimentosProcedimento>
                {
                    new() { Ordem = 1, Texto = "P1" },
                    new() { Ordem = 2, Texto = "P2" }
                },
                PortariasRelacionadas = new List<MultasProcedimentosPortariaRelacionada>
                {
                    new() { Ordem = 1, Titulo = "T1", Url = "u1" }
                }
            };
            context.MultasProcedimentos.Add(mp);
            await context.SaveChangesAsync();

            var repo = new MultasProcedimentosRepository(context);
            var result = await repo.GetWithChildrenByIdAsync(mp.Id);

            Assert.NotNull(result);
            Assert.Equal(2, result!.Procedimentos.Count);
            Assert.Single(result.PortariasRelacionadas);
        }

        [Fact]
        public async Task AllWithChildrenAsync_ReturnsAllEntitiesWithChildren()
        {
            var dbName = Guid.NewGuid().ToString();
            await using var context = CreateContext(dbName);

            var mp1 = new MultasProcedimentos
            {
                Slug = "mp-1",
                Ativo = true,
                DataCriacao = DateTime.UtcNow,
                Procedimentos = new List<MultasProcedimentosProcedimento> { new() { Texto = "p1" } },
                PortariasRelacionadas = new List<MultasProcedimentosPortariaRelacionada> { new() { Titulo = "t1", Url = "u1" } }
            };
            var mp2 = new MultasProcedimentos
            {
                Slug = "mp-2",
                Ativo = true,
                DataCriacao = DateTime.UtcNow,
                Procedimentos = new List<MultasProcedimentosProcedimento> { new() { Texto = "p2" } },
                PortariasRelacionadas = new List<MultasProcedimentosPortariaRelacionada> { new() { Titulo = "t2", Url = "u2" } }
            };

            context.MultasProcedimentos.AddRange(mp1, mp2);
            await context.SaveChangesAsync();

            var repo = new MultasProcedimentosRepository(context);
            var all = await repo.AllWithChildrenAsync();

            Assert.Equal(2, all.Count);
            Assert.All(all, item => Assert.NotNull(item.Procedimentos));
            Assert.All(all, item => Assert.NotNull(item.PortariasRelacionadas));
        }

        [Fact]
        public async Task DisableAsync_ReturnsTrueAndSetsAtivoFalse_WhenEntityExists()
        {
            var dbName = Guid.NewGuid().ToString();
            await using var context = CreateContext(dbName);

            var mp = new MultasProcedimentos { Slug = "mp-disable", Ativo = true, DataCriacao = DateTime.UtcNow };
            context.MultasProcedimentos.Add(mp);
            await context.SaveChangesAsync();

            var repo = new MultasProcedimentosRepository(context);
            var result = await repo.DisableAsync(mp.Id);

            Assert.True(result);

            var reloaded = await context.MultasProcedimentos.FindAsync(mp.Id);
            Assert.NotNull(reloaded);
            Assert.False(reloaded!.Ativo);
        }

        [Fact]
        public async Task DisableAsync_ReturnsFalse_WhenEntityDoesNotExist()
        {
            var dbName = Guid.NewGuid().ToString();
            await using var context = CreateContext(dbName);

            var repo = new MultasProcedimentosRepository(context);
            var result = await repo.DisableAsync(9999);

            Assert.False(result);
        }

        [Fact]
        public async Task GetBySlugAtivoAsync_ReturnsEntity_WhenActive()
        {
            var dbName = Guid.NewGuid().ToString();
            await using var context = CreateContext(dbName);

            var active = new MultasProcedimentos { Slug = "active-slug", Ativo = true, DataCriacao = DateTime.UtcNow };
            var inactive = new MultasProcedimentos { Slug = "inactive-slug", Ativo = false, DataCriacao = DateTime.UtcNow };

            context.MultasProcedimentos.AddRange(active, inactive);
            await context.SaveChangesAsync();

            var repo = new MultasProcedimentosRepository(context);
            var found = await repo.GetBySlugAtivoAsync("active-slug");
            var notFound = await repo.GetBySlugAtivoAsync("inactive-slug");

            Assert.NotNull(found);
            Assert.Equal(active.Slug, found!.Slug);
            Assert.Null(notFound);
        }

        [Fact]
        public async Task GetWithChildrenBySlugAtivoAsync_ReturnsEntityWithChildren_WhenActive()
        {
            var dbName = Guid.NewGuid().ToString();
            await using var context = CreateContext(dbName);

            var mp = new MultasProcedimentos
            {
                Slug = "with-children",
                Ativo = true,
                DataCriacao = DateTime.UtcNow,
                Procedimentos = new List<MultasProcedimentosProcedimento> { new() { Texto = "p" } },
                PortariasRelacionadas = new List<MultasProcedimentosPortariaRelacionada> { new() { Titulo = "t", Url = "u" } }
            };

            context.MultasProcedimentos.Add(mp);
            await context.SaveChangesAsync();

            var repo = new MultasProcedimentosRepository(context);
            var result = await repo.GetWithChildrenBySlugAtivoAsync("with-children");

            Assert.NotNull(result);
            Assert.Single(result!.Procedimentos);
            Assert.Single(result.PortariasRelacionadas);
        }

        [Fact]
        public async Task ReplaceProcedimentosAsync_ReplacesExistingProcedimentos()
        {
            var dbName = Guid.NewGuid().ToString();
            await using var context = CreateContext(dbName);

            var mp = new MultasProcedimentos { Slug = "replace-proc", Ativo = true, DataCriacao = DateTime.UtcNow };
            context.MultasProcedimentos.Add(mp);
            await context.SaveChangesAsync();

            // add existing procedimentos
            var old1 = new MultasProcedimentosProcedimento { IdMultasProcedimentos = mp.Id, Ordem = 1, Texto = "old1" };
            var old2 = new MultasProcedimentosProcedimento { IdMultasProcedimentos = mp.Id, Ordem = 2, Texto = "old2" };
            context.Set<MultasProcedimentosProcedimento>().AddRange(old1, old2);
            await context.SaveChangesAsync();

            var repo = new MultasProcedimentosRepository(context);

            var nov = new List<MultasProcedimentosProcedimento>
            {
                new MultasProcedimentosProcedimento { Ordem = 1, Texto = "new1" }
            };

            await repo.ReplaceProcedimentosAsync(mp.Id, nov);
            // method doesn't call SaveChangesAsync, so call it to persist
            await context.SaveChangesAsync();

            var procFromDb = await context.Set<MultasProcedimentosProcedimento>()
                .Where(p => p.IdMultasProcedimentos == mp.Id)
                .ToListAsync();

            Assert.Single(procFromDb);
            Assert.Equal(mp.Id, procFromDb[0].IdMultasProcedimentos);
            Assert.Equal("new1", procFromDb[0].Texto);
        }

        [Fact]
        public async Task ReplaceProcedimentosAsync_Works_WhenNoExistingProcedimentos()
        {
            var dbName = Guid.NewGuid().ToString();
            await using var context = CreateContext(dbName);

            var mp = new MultasProcedimentos { Slug = "replace-proc-empty", Ativo = true, DataCriacao = DateTime.UtcNow };
            context.MultasProcedimentos.Add(mp);
            await context.SaveChangesAsync();

            var repo = new MultasProcedimentosRepository(context);

            var novos = new List<MultasProcedimentosProcedimento>
            {
                new MultasProcedimentosProcedimento { Ordem = 1, Texto = "novo1" },
                new MultasProcedimentosProcedimento { Ordem = 2, Texto = "novo2" }
            };

            await repo.ReplaceProcedimentosAsync(mp.Id, novos);
            await context.SaveChangesAsync();

            var procFromDb = await context.Set<MultasProcedimentosProcedimento>()
                .Where(p => p.IdMultasProcedimentos == mp.Id)
                .ToListAsync();

            Assert.Equal(2, procFromDb.Count);
            Assert.All(procFromDb, p => Assert.Equal(mp.Id, p.IdMultasProcedimentos));
        }

        [Fact]
        public async Task ReplacePortariasAsync_ReplacesExistingPortarias()
        {
            var dbName = Guid.NewGuid().ToString();
            await using var context = CreateContext(dbName);

            var mp = new MultasProcedimentos { Slug = "replace-port", Ativo = true, DataCriacao = DateTime.UtcNow };
            context.MultasProcedimentos.Add(mp);
            await context.SaveChangesAsync();

            var old = new MultasProcedimentosPortariaRelacionada { IdMultasProcedimentos = mp.Id, Ordem = 1, Titulo = "old", Url = "u" };
            context.Set<MultasProcedimentosPortariaRelacionada>().Add(old);
            await context.SaveChangesAsync();

            var repo = new MultasProcedimentosRepository(context);

            var novas = new List<MultasProcedimentosPortariaRelacionada>
            {
                new MultasProcedimentosPortariaRelacionada { Ordem = 1, Titulo = "new", Url = "nu" }
            };

            await repo.ReplacePortariasAsync(mp.Id, novas);
            await context.SaveChangesAsync();

            var portarias = await context.Set<MultasProcedimentosPortariaRelacionada>()
                .Where(p => p.IdMultasProcedimentos == mp.Id)
                .ToListAsync();

            Assert.Single(portarias);
            Assert.Equal(mp.Id, portarias[0].IdMultasProcedimentos);
            Assert.Equal("new", portarias[0].Titulo);
        }

        [Fact]
        public async Task ReplacePortariasAsync_Works_WhenNoExistingPortarias()
        {
            var dbName = Guid.NewGuid().ToString();
            await using var context = CreateContext(dbName);

            var mp = new MultasProcedimentos { Slug = "replace-port-empty", Ativo = true, DataCriacao = DateTime.UtcNow };
            context.MultasProcedimentos.Add(mp);
            await context.SaveChangesAsync();

            var repo = new MultasProcedimentosRepository(context);

            var novas = new List<MultasProcedimentosPortariaRelacionada>
            {
                new MultasProcedimentosPortariaRelacionada { Ordem = 1, Titulo = "p1", Url = "u1" },
                new MultasProcedimentosPortariaRelacionada { Ordem = 2, Titulo = "p2", Url = "u2" }
            };

            await repo.ReplacePortariasAsync(mp.Id, novas);
            await context.SaveChangesAsync();

            var portarias = await context.Set<MultasProcedimentosPortariaRelacionada>()
                .Where(p => p.IdMultasProcedimentos == mp.Id)
                .ToListAsync();

            Assert.Equal(2, portarias.Count);
            Assert.All(portarias, p => Assert.Equal(mp.Id, p.IdMultasProcedimentos));
        }
    }
}