using Microsoft.EntityFrameworkCore;
using PortalTCMSP.Domain.Entities.ServicosEntity.PrazosProcessuaisEntity;
using PortalTCMSP.Infra.Data.Context;
using PortalTCMSP.Infra.Data.Repositories.Servicos.PrazosProcessuais;

namespace PortalTCMSP.Infra.Data.Tests.Repositories
{
    public class PrazosProcessuaisRepositoryTests
    {
        private static PortalTCMSPContext CreateContext()
        {
            var options = new DbContextOptionsBuilder<PortalTCMSPContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new PortalTCMSPContext(options);
        }

        [Fact]
        public async Task GetWithChildrenByIdAsync_ReturnsEntityWithChildrenAndAnexos()
        {
            await using var context = CreateContext();
            var entity = new PrazosProcessuais
            {
                Id = 1,
                Ativo = true,
                Slug = "s1",
                DataCriacao = DateTime.UtcNow,
                PrazosProcessuaisItens = new List<PrazosProcessuaisItem>
                {
                    new PrazosProcessuaisItem
                    {
                        Id = 10,
                        IdPrazosProcessuais = 1,
                        Ativo = true,
                        Anexos = new List<PrazosProcessuaisItemAnexo>
                        {
                            new PrazosProcessuaisItemAnexo
                            {
                                Id = 100,
                                IdPrazosProcessuaisItem = 10,
                                Ativo = true
                            }
                        }
                    }
                }
            };
            context.PrazosProcessuais.Add(entity);
            await context.SaveChangesAsync();

            var repo = new PrazosProcessuaisRepository(context);
            var result = await repo.GetWithChildrenByIdAsync(1);

            Assert.NotNull(result);
            Assert.NotNull(result!.PrazosProcessuaisItens);
            Assert.Single(result.PrazosProcessuaisItens);
            var item = result.PrazosProcessuaisItens.First();
            Assert.NotNull(item.Anexos);
            Assert.Single(item.Anexos);
        }

        [Fact]
        public async Task AllWithChildrenAsync_ReturnsAllOrderedByDataCriacaoDescending()
        {
            await using var context = CreateContext();
            var older = new PrazosProcessuais
            {
                Id = 1,
                Ativo = true,
                Slug = "old",
                DataCriacao = DateTime.UtcNow.AddDays(-1),
                PrazosProcessuaisItens = new List<PrazosProcessuaisItem>()
            };
            var newer = new PrazosProcessuais
            {
                Id = 2,
                Ativo = true,
                Slug = "new",
                DataCriacao = DateTime.UtcNow,
                PrazosProcessuaisItens = new List<PrazosProcessuaisItem>()
            };
            context.PrazosProcessuais.AddRange(older, newer);
            await context.SaveChangesAsync();

            var repo = new PrazosProcessuaisRepository(context);
            var list = await repo.AllWithChildrenAsync();

            Assert.Equal(2, list.Count);
            Assert.Equal(newer.Id, list[0].Id);
            Assert.Equal(older.Id, list[1].Id);
        }

        [Fact]
        public async Task DisableAsync_WhenEntityExists_DisablesAndReturnsTrue()
        {
            await using var context = CreateContext();
            var entity = new PrazosProcessuais { Id = 1, Ativo = true, Slug = "s1", DataCriacao = DateTime.UtcNow };
            context.PrazosProcessuais.Add(entity);
            await context.SaveChangesAsync();

            var repo = new PrazosProcessuaisRepository(context);
            var result = await repo.DisableAsync(1);

            Assert.True(result);
            var fromDb = await context.PrazosProcessuais.FindAsync(1L);
            Assert.False(fromDb!.Ativo);
        }

        [Fact]
        public async Task DisableAsync_WhenEntityDoesNotExist_ReturnsFalse()
        {
            await using var context = CreateContext();
            var repo = new PrazosProcessuaisRepository(context);
            var result = await repo.DisableAsync(999);
            Assert.False(result);
        }

        [Fact]
        public async Task GetBySlugAtivoAsync_ReturnsOnlyIfAtivo()
        {
            await using var context = CreateContext();
            var active = new PrazosProcessuais { Id = 1, Slug = "s1", Ativo = true, DataCriacao = DateTime.UtcNow };
            var inactive = new PrazosProcessuais { Id = 2, Slug = "s2", Ativo = false, DataCriacao = DateTime.UtcNow };
            context.PrazosProcessuais.AddRange(active, inactive);
            await context.SaveChangesAsync();

            var repo = new PrazosProcessuaisRepository(context);
            var gotActive = await repo.GetBySlugAtivoAsync("s1");
            var gotInactive = await repo.GetBySlugAtivoAsync("s2");

            Assert.NotNull(gotActive);
            Assert.Equal(active.Id, gotActive!.Id);
            Assert.Null(gotInactive);
        }

        [Fact]
        public async Task GetWithChildrenBySlugAtivoAsync_FiltersOnlyActiveItemsAndAnexos()
        {
            await using var context = CreateContext();
            var itemActive = new PrazosProcessuaisItem
            {
                Id = 10,
                IdPrazosProcessuais = 1,
                Ativo = true,
                Anexos = new List<PrazosProcessuaisItemAnexo>
                {
                    new PrazosProcessuaisItemAnexo { Id = 100, IdPrazosProcessuaisItem = 10, Ativo = true },
                    new PrazosProcessuaisItemAnexo { Id = 101, IdPrazosProcessuaisItem = 10, Ativo = false }
                }
            };
            var itemInactive = new PrazosProcessuaisItem
            {
                Id = 11,
                IdPrazosProcessuais = 1,
                Ativo = false,
                Anexos = new List<PrazosProcessuaisItemAnexo>
                {
                    new PrazosProcessuaisItemAnexo { Id = 102, IdPrazosProcessuaisItem = 11, Ativo = true }
                }
            };
            var entity = new PrazosProcessuais
            {
                Id = 1,
                Slug = "s1",
                Ativo = true,
                DataCriacao = DateTime.UtcNow,
                PrazosProcessuaisItens = new List<PrazosProcessuaisItem> { itemActive, itemInactive }
            };
            context.PrazosProcessuais.Add(entity);
            await context.SaveChangesAsync();

            var repo = new PrazosProcessuaisRepository(context);
            var result = await repo.GetWithChildrenBySlugAtivoAsync("s1");

            Assert.NotNull(result);
            Assert.NotNull(result!.PrazosProcessuaisItens);
            Assert.Single(result.PrazosProcessuaisItens);
            var item = result.PrazosProcessuaisItens.First();
            Assert.Single(item.Anexos);
            Assert.Equal(100, item.Anexos.First().Id);
        }

        [Fact]
        public async Task ReplacePrazosProcessuaisItensAsync_ReplacesOldItemsWithNewOnes()
        {
            await using var context = CreateContext();
            var old1 = new PrazosProcessuaisItem { Id = 10, IdPrazosProcessuais = 1, Ativo = true };
            var old2 = new PrazosProcessuaisItem { Id = 11, IdPrazosProcessuais = 1, Ativo = true };
            context.Set<PrazosProcessuaisItem>().AddRange(old1, old2);
            await context.SaveChangesAsync();

            var repo = new PrazosProcessuaisRepository(context);
            var novos = new[]
            {
                new PrazosProcessuaisItem { Id = 20, Ativo = true }, 
                new PrazosProcessuaisItem { Id = 21, Ativo = true }
            };

            await repo.ReplacePrazosProcessuaisItensAsync(1, novos);
            await context.SaveChangesAsync();

            var remaining = await context.Set<PrazosProcessuaisItem>().Where(x => x.IdPrazosProcessuais == 1).ToListAsync();
            Assert.Equal(2, remaining.Count);
            Assert.All(remaining, r => Assert.Equal(1, r.IdPrazosProcessuais));
            Assert.Contains(remaining, r => r.Id == 20);
            Assert.Contains(remaining, r => r.Id == 21);
        }
    }
}