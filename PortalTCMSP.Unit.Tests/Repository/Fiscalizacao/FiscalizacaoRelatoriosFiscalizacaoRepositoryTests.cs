using Microsoft.EntityFrameworkCore;
using PortalTCMSP.Domain.Entities.FiscalizacaoEntity.RelatorioFiscalizacao;
using PortalTCMSP.Infra.Data.Context;
using PortalTCMSP.Infra.Data.Repositories.Fiscalizacao.RelatorioFiscalizacao;

namespace PortalTCMSP.Unit.Tests.Repository.Fiscalizacao
{
    public class FiscalizacaoRelatoriosFiscalizacaoRepositoryTests
    {
        private PortalTCMSPContext GetInMemoryContext(string? dbName = null)
        {
            var options = new DbContextOptionsBuilder<PortalTCMSPContext>()
                .UseInMemoryDatabase(dbName ?? "FiscalizacaoRelatoriosFiscalizacaoRepoTestDB_" + Guid.NewGuid())
                .Options;
            return new PortalTCMSPContext(options);
        }

        [Fact]
        public async Task FindBySlugAsync_ReturnsEntity_WhenExists()
        {
            using var context = GetInMemoryContext();
            var repo = new FiscalizacaoRelatoriosFiscalizacaoRepository(context);

            var entity = new FiscalizacaoRelatorioFiscalizacao
            {
                Slug = "slug-1",
                Carrocel = new List<FiscalizacaoRelatorioFiscalizacaoCarrossel>()
            };

            context.FiscalizacaoRelatorioFiscalizacao.Add(entity);
            await context.SaveChangesAsync();

            var result = await repo.FindBySlugAsync("slug-1");

            Assert.NotNull(result);
            Assert.Equal("slug-1", result!.Slug);
        }

        [Fact]
        public async Task FindBySlugAsync_ReturnsNull_WhenNotExists()
        {
            using var context = GetInMemoryContext();
            var repo = new FiscalizacaoRelatoriosFiscalizacaoRepository(context);

            var entity = new FiscalizacaoRelatorioFiscalizacao
            {
                Slug = "slug-exists",
                Carrocel = new List<FiscalizacaoRelatorioFiscalizacaoCarrossel>()
            };

            context.FiscalizacaoRelatorioFiscalizacao.Add(entity);
            await context.SaveChangesAsync();

            var result = await repo.FindBySlugAsync("slug-not-found");

            Assert.Null(result);
        }

        [Fact]
        public async Task GetWithChildrenByIdAsync_ReturnsEntityWithChildren_WhenExists()
        {
            using var context = GetInMemoryContext();
            var repo = new FiscalizacaoRelatoriosFiscalizacaoRepository(context);

            var parent = new FiscalizacaoRelatorioFiscalizacao
            {
                Slug = "parent-1",
                Carrocel = new List<FiscalizacaoRelatorioFiscalizacaoCarrossel>()
            };

            context.FiscalizacaoRelatorioFiscalizacao.Add(parent);
            await context.SaveChangesAsync();

            var conteudoLink = new FiscalizacaoRelatorioFiscalizacaoConteudoLink
            {
                ConteudoDestaque = new List<FiscalizacaoRelatorioFiscalizacaoConteudoDestaque>
                {
                    new FiscalizacaoRelatorioFiscalizacaoConteudoDestaque
                    {
                        Descricoes = new List<FiscalizacaoRelatorioFiscalizacaoDescricao>
                        {
                            new FiscalizacaoRelatorioFiscalizacaoDescricao()
                        }
                    }
                },
                DocumentosAnexos = new List<FiscalizacaoRelatorioFiscalizacaoDocumentoAnexo>(),
                ImagensAnexas = new List<FiscalizacaoRelatorioFiscalizacaoImagemAnexa>(),
                TcRelacionados = new List<FiscalizacaoRelatorioFiscalizacaoTcRelacionado>()
            };

            var conteudoCarrocel = new FiscalizacaoRelatorioFiscalizacaoConteudoCarrocel
            {
                ConteudoLink = conteudoLink
            };

            var carrossel = new FiscalizacaoRelatorioFiscalizacaoCarrossel
            {
                IdConteudo = parent.Id,
                ConteudoCarrocel = new List<FiscalizacaoRelatorioFiscalizacaoConteudoCarrocel> { conteudoCarrocel }
            };

            context.Set<FiscalizacaoRelatorioFiscalizacaoCarrossel>().Add(carrossel);
            await context.SaveChangesAsync();

            var result = await repo.GetWithChildrenByIdAsync(parent.Id);

            Assert.NotNull(result);
            Assert.Equal(parent.Id, result!.Id);
            Assert.NotNull(result.Carrocel);
            Assert.Single(result.Carrocel);
            var first = result.Carrocel.First();
            Assert.NotNull(first.ConteudoCarrocel);
            Assert.Single(first.ConteudoCarrocel);
            var firstConteudo = first.ConteudoCarrocel.First();
            Assert.NotNull(firstConteudo.ConteudoLink);
            Assert.NotNull(firstConteudo.ConteudoLink.ConteudoDestaque);
            Assert.Single(firstConteudo.ConteudoLink.ConteudoDestaque);
            Assert.NotNull(firstConteudo.ConteudoLink.ConteudoDestaque.First().Descricoes);
            Assert.Single(firstConteudo.ConteudoLink.ConteudoDestaque.First().Descricoes);
        }

        [Fact]
        public async Task GetWithChildrenBySlugAsync_ReturnsEntityWithChildren_WhenExists()
        {
            using var context = GetInMemoryContext();
            var repo = new FiscalizacaoRelatoriosFiscalizacaoRepository(context);

            var parent = new FiscalizacaoRelatorioFiscalizacao
            {
                Slug = "parent-slug",
                Carrocel = new List<FiscalizacaoRelatorioFiscalizacaoCarrossel>()
            };

            context.FiscalizacaoRelatorioFiscalizacao.Add(parent);
            await context.SaveChangesAsync();

            var conteudoLink = new FiscalizacaoRelatorioFiscalizacaoConteudoLink
            {
                ConteudoDestaque = new List<FiscalizacaoRelatorioFiscalizacaoConteudoDestaque>
                {
                    new FiscalizacaoRelatorioFiscalizacaoConteudoDestaque
                    {
                        Descricoes = new List<FiscalizacaoRelatorioFiscalizacaoDescricao>
                        {
                            new FiscalizacaoRelatorioFiscalizacaoDescricao()
                        }
                    }
                },
                DocumentosAnexos = new List<FiscalizacaoRelatorioFiscalizacaoDocumentoAnexo>(),
                ImagensAnexas = new List<FiscalizacaoRelatorioFiscalizacaoImagemAnexa>(),
                TcRelacionados = new List<FiscalizacaoRelatorioFiscalizacaoTcRelacionado>()
            };

            var conteudoCarrocel = new FiscalizacaoRelatorioFiscalizacaoConteudoCarrocel
            {
                ConteudoLink = conteudoLink
            };

            var carrossel = new FiscalizacaoRelatorioFiscalizacaoCarrossel
            {
                IdConteudo = parent.Id,
                ConteudoCarrocel = new List<FiscalizacaoRelatorioFiscalizacaoConteudoCarrocel> { conteudoCarrocel }
            };

            context.Set<FiscalizacaoRelatorioFiscalizacaoCarrossel>().Add(carrossel);
            await context.SaveChangesAsync();

            var result = await repo.GetWithChildrenBySlugAsync("parent-slug");

            Assert.NotNull(result);
            Assert.Equal("parent-slug", result!.Slug);
            Assert.NotNull(result.Carrocel);
            Assert.Single(result.Carrocel);
        }

        [Fact]
        public async Task AllWithChildrenAsync_ReturnsAllEntitiesWithChildren()
        {
            using var context = GetInMemoryContext();
            var repo = new FiscalizacaoRelatoriosFiscalizacaoRepository(context);

            var p1 = new FiscalizacaoRelatorioFiscalizacao
            {
                Slug = "a",
                Carrocel = new List<FiscalizacaoRelatorioFiscalizacaoCarrossel>()
            };
            var p2 = new FiscalizacaoRelatorioFiscalizacao
            {
                Slug = "b",
                Carrocel = new List<FiscalizacaoRelatorioFiscalizacaoCarrossel>()
            };

            context.FiscalizacaoRelatorioFiscalizacao.AddRange(p1, p2);
            await context.SaveChangesAsync();

            var carrossel = new FiscalizacaoRelatorioFiscalizacaoCarrossel
            {
                IdConteudo = p1.Id,
                ConteudoCarrocel = new List<FiscalizacaoRelatorioFiscalizacaoConteudoCarrocel>()
            };
            context.Set<FiscalizacaoRelatorioFiscalizacaoCarrossel>().Add(carrossel);
            await context.SaveChangesAsync();

            var results = await repo.AllWithChildrenAsync();

            Assert.NotNull(results);
            Assert.Equal(2, results.Count);
            var itemWithChild = results.First(r => r.Id == p1.Id);
            Assert.NotNull(itemWithChild.Carrocel);
            Assert.Single(itemWithChild.Carrocel);
        }

        [Fact]
        public async Task ReplaceCarrosselAsync_ReplacesOldWithNew()
        {
            using var context = GetInMemoryContext();
            var repo = new FiscalizacaoRelatoriosFiscalizacaoRepository(context);

            var parent = new FiscalizacaoRelatorioFiscalizacao
            {
                Slug = "to-replace",
                Carrocel = new List<FiscalizacaoRelatorioFiscalizacaoCarrossel>()
            };

            context.FiscalizacaoRelatorioFiscalizacao.Add(parent);
            await context.SaveChangesAsync();

            var old1 = new FiscalizacaoRelatorioFiscalizacaoCarrossel { IdConteudo = parent.Id };
            var old2 = new FiscalizacaoRelatorioFiscalizacaoCarrossel { IdConteudo = parent.Id };
            context.Set<FiscalizacaoRelatorioFiscalizacaoCarrossel>().AddRange(old1, old2);
            await context.SaveChangesAsync();

            var novo1 = new FiscalizacaoRelatorioFiscalizacaoCarrossel { ConteudoCarrocel = new List<FiscalizacaoRelatorioFiscalizacaoConteudoCarrocel>() };
            var novo2 = new FiscalizacaoRelatorioFiscalizacaoCarrossel { ConteudoCarrocel = new List<FiscalizacaoRelatorioFiscalizacaoConteudoCarrocel>() };

            await repo.ReplaceCarrosselAsync(parent.Id, new[] { novo1, novo2 });
            var committed = await repo.CommitAsync();
            Assert.True(committed);

            var items = context.Set<FiscalizacaoRelatorioFiscalizacaoCarrossel>().Where(c => c.IdConteudo == parent.Id).ToList();
            Assert.Equal(2, items.Count);
            Assert.All(items, i => Assert.Equal(parent.Id, i.IdConteudo));
        }
    }
}