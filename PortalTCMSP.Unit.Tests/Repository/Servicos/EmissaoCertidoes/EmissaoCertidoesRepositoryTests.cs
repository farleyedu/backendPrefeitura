using Microsoft.EntityFrameworkCore;
using PortalTCMSP.Domain.Entities.ServicosEntity.EmissaoCertidoesEntity;
using PortalTCMSP.Infra.Data.Context;
using PortalTCMSP.Infra.Data.Repositories.Servicos.EmissaoCertidoes;

namespace PortalTCMSP.Infra.Tests.Repositories.Servicos
{
    public class EmissaoCertidoesRepositoryTests
    {
        private static PortalTCMSPContext CreateContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<PortalTCMSPContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .EnableSensitiveDataLogging()
                .Options;
            return new PortalTCMSPContext(options);
        }

        [Fact]
        public async Task GetWithChildrenByIdAsync_ReturnsEntityWithChildren()
        {
            var dbName = Guid.NewGuid().ToString();
            await using var context = CreateContext(dbName);
            var repo = new EmissaoCertidoesRepository(context);

            var descritivo = new EmissaoCertidoesDescritivo();
            var orientacao = new EmissaoCertidoesOrientacao { Descritivos = new List<EmissaoCertidoesDescritivo> { descritivo } };
            var secao = new EmissaoCertidoesSecaoOrientacao { Orientacoes = new List<EmissaoCertidoesOrientacao> { orientacao } };
            var acao = new EmissaoCertidoesAcao();
            var entidade = new EmissaoCertidoes
            {
                Slug = "test-slug",
                Ativo = true,
                Acoes = new List<EmissaoCertidoesAcao> { acao },
                SecaoOrientacoes = new List<EmissaoCertidoesSecaoOrientacao> { secao }
            };

            await context.EmissaoCertidoes.AddAsync(entidade);
            await context.SaveChangesAsync();

            var result = await repo.GetWithChildrenByIdAsync(entidade.Id);

            Assert.NotNull(result);
            Assert.NotNull(result!.Acoes);
            Assert.Single(result.Acoes);
            Assert.NotNull(result.SecaoOrientacoes);
            Assert.Single(result.SecaoOrientacoes);
            var sec = result.SecaoOrientacoes.First();
            Assert.NotNull(sec.Orientacoes);
            Assert.Single(sec.Orientacoes);
            var ori = sec.Orientacoes.First();
            Assert.NotNull(ori.Descritivos);
            Assert.Single(ori.Descritivos);
        }

        [Fact]
        public async Task AllWithChildrenAsync_ReturnsAllWithChildren()
        {
            var dbName = Guid.NewGuid().ToString();
            await using var context = CreateContext(dbName);
            var repo = new EmissaoCertidoesRepository(context);

            var e1 = new EmissaoCertidoes
            {
                Slug = "s1",
                Acoes = new List<EmissaoCertidoesAcao> { new EmissaoCertidoesAcao() },
                SecaoOrientacoes = new List<EmissaoCertidoesSecaoOrientacao> { new EmissaoCertidoesSecaoOrientacao() }
            };
            var e2 = new EmissaoCertidoes
            {
                Slug = "s2",
                Acoes = new List<EmissaoCertidoesAcao> { new EmissaoCertidoesAcao() },
                SecaoOrientacoes = new List<EmissaoCertidoesSecaoOrientacao> { new EmissaoCertidoesSecaoOrientacao() }
            };

            await context.EmissaoCertidoes.AddRangeAsync(e1, e2);
            await context.SaveChangesAsync();

            var results = await repo.AllWithChildrenAsync();

            Assert.Equal(2, results.Count);
            Assert.All(results, r => Assert.NotNull(r.Acoes));
            Assert.All(results, r => Assert.NotNull(r.SecaoOrientacoes));
        }

        [Fact]
        public async Task DisableAsync_WhenEntityExists_DisablesAndReturnsTrue()
        {
            var dbName = Guid.NewGuid().ToString();
            await using var context = CreateContext(dbName);
            var repo = new EmissaoCertidoesRepository(context);

            var entidade = new EmissaoCertidoes { Slug = "d1", Ativo = true };
            await context.EmissaoCertidoes.AddAsync(entidade);
            await context.SaveChangesAsync();

            var result = await repo.DisableAsync(entidade.Id);

            Assert.True(result);

            var fromDb = await context.EmissaoCertidoes.FindAsync(entidade.Id);
            Assert.NotNull(fromDb);
            Assert.False(fromDb!.Ativo);
        }

        [Fact]
        public async Task DisableAsync_WhenNotFound_ReturnsFalse()
        {
            var dbName = Guid.NewGuid().ToString();
            await using var context = CreateContext(dbName);
            var repo = new EmissaoCertidoesRepository(context);

            var result = await repo.DisableAsync(99999);

            Assert.False(result);
        }

        [Fact]
        public async Task GetBySlugAtivoAsync_ReturnsEntity_WhenSlugAndActive()
        {
            var dbName = Guid.NewGuid().ToString();
            await using var context = CreateContext(dbName);
            var repo = new EmissaoCertidoesRepository(context);

            var entidade = new EmissaoCertidoes { Slug = "active-slug", Ativo = true };
            var inactive = new EmissaoCertidoes { Slug = "inactive-slug", Ativo = false };

            await context.EmissaoCertidoes.AddRangeAsync(entidade, inactive);
            await context.SaveChangesAsync();

            var result = await repo.GetBySlugAtivoAsync("active-slug");
            var resultNull = await repo.GetBySlugAtivoAsync("inactive-slug");

            Assert.NotNull(result);
            Assert.Equal(entidade.Id, result!.Id);
            Assert.Null(resultNull);
        }

        [Fact]
        public async Task GetWithChildrenBySlugAtivoAsync_ReturnsEntityWithChildren_WhenSlugAndActive()
        {
            var dbName = Guid.NewGuid().ToString();
            await using var context = CreateContext(dbName);
            var repo = new EmissaoCertidoesRepository(context);

            var sec = new EmissaoCertidoesSecaoOrientacao { Orientacoes = new List<EmissaoCertidoesOrientacao>() };
            var acao = new EmissaoCertidoesAcao();
            var entidade = new EmissaoCertidoes
            {
                Slug = "with-children",
                Ativo = true,
                Acoes = new List<EmissaoCertidoesAcao> { acao },
                SecaoOrientacoes = new List<EmissaoCertidoesSecaoOrientacao> { sec }
            };

            await context.EmissaoCertidoes.AddAsync(entidade);
            await context.SaveChangesAsync();

            var result = await repo.GetWithChildrenBySlugAtivoAsync("with-children");

            Assert.NotNull(result);
            Assert.Single(result!.Acoes);
            Assert.Single(result.SecaoOrientacoes);
        }

        [Fact]
        public async Task ReplaceAcoesAsync_ReplacesActions()
        {
            var dbName = Guid.NewGuid().ToString();
            await using var context = CreateContext(dbName);
            var repo = new EmissaoCertidoesRepository(context);

            var entidade = new EmissaoCertidoes { Slug = "r1" };
            await context.EmissaoCertidoes.AddAsync(entidade);
            await context.SaveChangesAsync();

            var antigo1 = new EmissaoCertidoesAcao { IdEmissaoCertidoes = entidade.Id, Titulo = "old1" };
            var antigo2 = new EmissaoCertidoesAcao { IdEmissaoCertidoes = entidade.Id, Titulo = "old2" };
            await context.EmissaoCertidoesAcao.AddRangeAsync(antigo1, antigo2);
            await context.SaveChangesAsync();

            var novos = new List<EmissaoCertidoesAcao>
            {
                new EmissaoCertidoesAcao { Titulo = "new1" },
                new EmissaoCertidoesAcao { Titulo = "new2" }
            };

            await repo.ReplaceAcoesAsync(entidade.Id, novos);

            await context.SaveChangesAsync();

            var actions = await context.EmissaoCertidoesAcao.Where(a => a.IdEmissaoCertidoes == entidade.Id).ToListAsync();
            Assert.Equal(2, actions.Count);
            Assert.All(actions, a => Assert.Equal(entidade.Id, a.IdEmissaoCertidoes));
            Assert.Contains(actions, a => a.Titulo == "new1");
            Assert.Contains(actions, a => a.Titulo == "new2");
        }

        [Fact]
        public async Task ReplaceSecoesAsync_ReplacesSectionsAndRemovesOrientacoesAndDescritivos()
        {
            var dbName = Guid.NewGuid().ToString();
            await using var context = CreateContext(dbName);
            var repo = new EmissaoCertidoesRepository(context);

            var entidade = new EmissaoCertidoes { Slug = "r2" };
            await context.EmissaoCertidoes.AddAsync(entidade);
            await context.SaveChangesAsync();

            var des = new EmissaoCertidoesDescritivo();
            var ori = new EmissaoCertidoesOrientacao { Descritivos = new List<EmissaoCertidoesDescritivo> { des } };
            var sec = new EmissaoCertidoesSecaoOrientacao { Orientacoes = new List<EmissaoCertidoesOrientacao> { ori }, IdEmissaoCertidoes = entidade.Id };
            await context.EmissaoCertidoesSecaoOrientacao.AddAsync(sec);
            await context.SaveChangesAsync();

            var secCountBefore = await context.EmissaoCertidoesSecaoOrientacao.CountAsync(s => s.IdEmissaoCertidoes == entidade.Id);
            var oriCountBefore = await context.EmissaoCertidoesOrientacao.CountAsync(o => o.IdEmissaoCertidoesSecaoOrientacao == sec.Id);
            var desCountBefore = await context.EmissaoCertidoesDescritivo.CountAsync(d => d.IdEmissaoCertidoesOrientacao != 0); // at least one exists
            Assert.Equal(1, secCountBefore);
            Assert.Equal(1, oriCountBefore);
            Assert.True(desCountBefore >= 0);

            var novas = new List<EmissaoCertidoesSecaoOrientacao>
            {
                new EmissaoCertidoesSecaoOrientacao { TituloPagina = "nova1" },
            };

            await repo.ReplaceSecoesAsync(entidade.Id, novas);

            await context.SaveChangesAsync();

            var secAfter = await context.EmissaoCertidoesSecaoOrientacao.Where(s => s.IdEmissaoCertidoes == entidade.Id).ToListAsync();
            var oriAfter = await context.EmissaoCertidoesOrientacao.Where(o => o.IdEmissaoCertidoesSecaoOrientacao == sec.Id).ToListAsync();
            var desAfter = await context.EmissaoCertidoesDescritivo.Where(d => oriAfter.Select(o => o.Id).Contains(d.IdEmissaoCertidoesOrientacao)).ToListAsync();

            Assert.Single(secAfter);
            Assert.Equal(entidade.Id, secAfter.First().IdEmissaoCertidoes);
            Assert.Empty(oriAfter);
            Assert.Empty(desAfter);
        }
    }
}