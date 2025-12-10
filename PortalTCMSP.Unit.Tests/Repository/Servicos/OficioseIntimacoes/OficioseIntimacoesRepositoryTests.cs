using Microsoft.EntityFrameworkCore;
using PortalTCMSP.Domain.Entities.ServicosEntity.OficioseIntimacoesEntity;
using PortalTCMSP.Infra.Data.Context;
using PortalTCMSP.Infra.Data.Repositories.Servicos.OficioseIntimacoes;

namespace PortalTCMSP.Infra.Tests.Repositories.Servicos.OficioseIntimacoes
{
    public class OficioseIntimacoesRepositoryTests
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
            var entity = new Domain.Entities.ServicosEntity.OficioseIntimacoesEntity.OficioseIntimacoes
            {
                Titulo = "T",
                Slug = "s",
                Ativo = true,
                Secoes = new List<OficioseIntimacoesSecao>
                {
                    new OficioseIntimacoesSecao
                    {
                        Nome = "sec1",
                        Ordem = 1,
                        SecaoItem = new List<OficioseIntimacoesSecaoItem>
                        {
                            new OficioseIntimacoesSecaoItem { Descricao = "item1", Ordem = 1 }
                        }
                    }
                }
            };
            context.OficioseIntimacoes.Add(entity);
            await context.SaveChangesAsync();

            var repo = new OficioseIntimacoesRepository(context);
            var result = await repo.GetWithChildrenByIdAsync(entity.Id);

            Assert.NotNull(result);
            Assert.NotEmpty(result!.Secoes);
            Assert.Single(result.Secoes);
            Assert.NotEmpty(result.Secoes.First().SecaoItem);
            Assert.Single(result.Secoes.First().SecaoItem);
        }

        [Fact]
        public async Task AllWithChildrenAsync_ReturnsAllWithChildren()
        {
            var dbName = Guid.NewGuid().ToString();
            await using var context = CreateContext(dbName);
            var e1 = new Domain.Entities.ServicosEntity.OficioseIntimacoesEntity.OficioseIntimacoes
            {
                Titulo = "T1",
                Slug = "s1",
                Secoes = new List<OficioseIntimacoesSecao>
                {
                    new OficioseIntimacoesSecao { Nome = "s1", Ordem = 1, SecaoItem = new List<OficioseIntimacoesSecaoItem>{ new OficioseIntimacoesSecaoItem{ Descricao="i1", Ordem=1 } } }
                }
            };
            var e2 = new Domain.Entities.ServicosEntity.OficioseIntimacoesEntity.OficioseIntimacoes
            {
                Titulo = "T2",
                Slug = "s2",
                Secoes = new List<OficioseIntimacoesSecao>
                {
                    new OficioseIntimacoesSecao { Nome = "s2", Ordem = 1, SecaoItem = new List<OficioseIntimacoesSecaoItem>{ new OficioseIntimacoesSecaoItem{ Descricao="i2", Ordem=1 } } }
                }
            };
            context.OficioseIntimacoes.AddRange(e1, e2);
            await context.SaveChangesAsync();

            var repo = new OficioseIntimacoesRepository(context);
            var list = await repo.AllWithChildrenAsync();

            Assert.Equal(2, list.Count);
            Assert.All(list, l => Assert.NotEmpty(l.Secoes));
            Assert.All(list.SelectMany(l => l.Secoes), s => Assert.NotEmpty(s.SecaoItem));
        }

        [Fact]
        public async Task DisableAsync_NotFound_ReturnsFalse()
        {
            var dbName = Guid.NewGuid().ToString();
            await using var context = CreateContext(dbName);
            var repo = new OficioseIntimacoesRepository(context);

            var result = await repo.DisableAsync(9999);

            Assert.False(result);
        }

        [Fact]
        public async Task DisableAsync_Found_DisablesAndSaves()
        {
            var dbName = Guid.NewGuid().ToString();
            await using var context = CreateContext(dbName);
            var entity = new Domain.Entities.ServicosEntity.OficioseIntimacoesEntity.OficioseIntimacoes { Titulo = "T", Slug = "s", Ativo = true };
            context.OficioseIntimacoes.Add(entity);
            await context.SaveChangesAsync();

            var repo = new OficioseIntimacoesRepository(context);
            var result = await repo.DisableAsync(entity.Id);

            Assert.True(result);

            var fromDb = await context.OficioseIntimacoes.AsNoTracking().FirstOrDefaultAsync(o => o.Id == entity.Id);
            Assert.NotNull(fromDb);
            Assert.False(fromDb!.Ativo);
        }

        [Fact]
        public async Task GetBySlugAtivoAsync_ReturnsOnlyActive()
        {
            var dbName = Guid.NewGuid().ToString();
            await using var context = CreateContext(dbName);

            var active = new Domain.Entities.ServicosEntity.OficioseIntimacoesEntity.OficioseIntimacoes { Titulo = "A", Slug = "slug-x", Ativo = true };
            var inactive = new Domain.Entities.ServicosEntity.OficioseIntimacoesEntity.OficioseIntimacoes { Titulo = "I", Slug = "slug-x", Ativo = false };
            context.OficioseIntimacoes.AddRange(active, inactive);
            await context.SaveChangesAsync();

            var repo = new OficioseIntimacoesRepository(context);
            var found = await repo.GetBySlugAtivoAsync("slug-x");

            Assert.NotNull(found);
            Assert.Equal(active.Id, found!.Id);

            var notFound = await repo.GetBySlugAtivoAsync("no-such-slug");
            Assert.Null(notFound);
        }

        [Fact]
        public async Task GetWithChildrenBySlugAtivoAsync_ReturnsWithChildren()
        {
            var dbName = Guid.NewGuid().ToString();
            await using var context = CreateContext(dbName);

            var e = new Domain.Entities.ServicosEntity.OficioseIntimacoesEntity.OficioseIntimacoes
            {
                Titulo = "T",
                Slug = "slug-children",
                Ativo = true,
                Secoes = new List<OficioseIntimacoesSecao>
                {
                    new OficioseIntimacoesSecao
                    {
                        Nome = "sec",
                        Ordem = 1,
                        SecaoItem = new List<OficioseIntimacoesSecaoItem>
                        {
                            new OficioseIntimacoesSecaoItem { Descricao = "it", Ordem = 1 }
                        }
                    }
                }
            };
            context.OficioseIntimacoes.Add(e);
            await context.SaveChangesAsync();

            var repo = new OficioseIntimacoesRepository(context);
            var found = await repo.GetWithChildrenBySlugAtivoAsync("slug-children");

            Assert.NotNull(found);
            Assert.NotEmpty(found!.Secoes);
            Assert.Single(found.Secoes);
            Assert.Single(found.Secoes.First().SecaoItem);
        }

        [Fact]
        public async Task ReplaceSecoesAsync_ReplacesOldWithNew()
        {
            var dbName = Guid.NewGuid().ToString();
            await using var context = CreateContext(dbName);

            var master = new Domain.Entities.ServicosEntity.OficioseIntimacoesEntity.OficioseIntimacoes { Titulo = "master", Slug = "master-slug" };
            context.OficioseIntimacoes.Add(master);
            await context.SaveChangesAsync();

            var original = new OficioseIntimacoesSecao { IdOficioseIntimacoes = master.Id, Nome = "old", Ordem = 1 };
            context.OficioseIntimacoesSecao.Add(original);
            await context.SaveChangesAsync();

            var repo = new OficioseIntimacoesRepository(context);

            var novas = new[]
            {
                new OficioseIntimacoesSecao { Nome = "new1", Ordem = 1 },
                new OficioseIntimacoesSecao { Nome = "new2", Ordem = 2 }
            };

            await repo.ReplaceSecoesAsync(master.Id, novas);
            await context.SaveChangesAsync();

            var fromDb = await context.OficioseIntimacoesSecao.Where(s => s.IdOficioseIntimacoes == master.Id).ToListAsync();
            Assert.Equal(2, fromDb.Count);
            Assert.Contains(fromDb, s => s.Nome == "new1");
            Assert.Contains(fromDb, s => s.Nome == "new2");
            Assert.All(fromDb, s => Assert.Equal(master.Id, s.IdOficioseIntimacoes));
        }

        [Fact]
        public async Task ReplaceSecaoItensAsync_ReplacesOldWithNew()
        {
            var dbName = Guid.NewGuid().ToString();
            await using var context = CreateContext(dbName);

            var masterOf = new Domain.Entities.ServicosEntity.OficioseIntimacoesEntity.OficioseIntimacoes { Titulo = "m", Slug = "m" };
            context.OficioseIntimacoes.Add(masterOf);
            await context.SaveChangesAsync();

            var secao = new OficioseIntimacoesSecao { IdOficioseIntimacoes = masterOf.Id, Nome = "sec", Ordem = 1 };
            context.OficioseIntimacoesSecao.Add(secao);
            await context.SaveChangesAsync();

            var originalItem = new OficioseIntimacoesSecaoItem { IdOficioseIntimacoesSecao = secao.Id, Descricao = "old", Ordem = 1 };
            context.OficioseIntimacoesSecaoItem.Add(originalItem);
            await context.SaveChangesAsync();

            var repo = new OficioseIntimacoesRepository(context);

            var novos = new[]
            {
                new OficioseIntimacoesSecaoItem { Descricao = "n1", Ordem = 1 },
                new OficioseIntimacoesSecaoItem { Descricao = "n2", Ordem = 2 }
            };

            await repo.ReplaceSecaoItensAsync(secao.Id, novos);
            await context.SaveChangesAsync();

            var fromDb = await context.OficioseIntimacoesSecaoItem.Where(i => i.IdOficioseIntimacoesSecao == secao.Id).ToListAsync();
            Assert.Equal(2, fromDb.Count);
            Assert.Contains(fromDb, i => i.Descricao == "n1");
            Assert.Contains(fromDb, i => i.Descricao == "n2");
            Assert.All(fromDb, i => Assert.Equal(secao.Id, i.IdOficioseIntimacoesSecao));
        }
    }
}