using Microsoft.EntityFrameworkCore;
using PortalTCMSP.Domain.Entities.ServicosEntity.CartorioEntity;
using PortalTCMSP.Infra.Data.Context;
using PortalTCMSP.Infra.Data.Repositories.Servicos.Cartorio;

namespace PortalTCMSP.Infra.Data.Tests.Repositories.Servicos.Cartorio
{
    public class CartorioRepositoryTests
    {
        private static PortalTCMSPContext CreateContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<PortalTCMSPContext>()
                .UseInMemoryDatabase(dbName)
                .Options;
            var ctx = new PortalTCMSPContext(options);
            return ctx;
        }

        [Fact]
        public async Task GetWithChildrenByIdAsync_ReturnsCartorioWithAtendimentos()
        {
            var db = CreateContext(nameof(GetWithChildrenByIdAsync_ReturnsCartorioWithAtendimentos));
            var cart = new Domain.Entities.ServicosEntity.CartorioEntity.Cartorio
            {
                Id = 100,
                Slug = "c-100",
                Ativo = true,
                TituloPagina = "C100",
                Atendimentos = new List<CartorioAtendimento>
                {
                    new CartorioAtendimento { Id = 1, IdCartorio = 100, Titulo = "A1", Descricao = "D1", Ordem = 1 }
                }
            };
            db.Cartorio.Add(cart);
            await db.SaveChangesAsync();

            var repo = new CartorioRepository(db);
            var result = await repo.GetWithChildrenByIdAsync(100);

            Assert.NotNull(result);
            Assert.Equal(100, result!.Id);
            Assert.NotNull(result.Atendimentos);
            Assert.Single(result.Atendimentos);
            Assert.Equal("A1", result.Atendimentos.First().Titulo);
        }

        [Fact]
        public async Task AllWithChildrenAsync_ReturnsAllWithAtendimentos()
        {
            var db = CreateContext(nameof(AllWithChildrenAsync_ReturnsAllWithAtendimentos));
            var c1 = new Domain.Entities.ServicosEntity.CartorioEntity.Cartorio
            {
                Id = 201,
                Slug = "c-201",
                Ativo = true,
                TituloPagina = "C201",
                Atendimentos = new List<CartorioAtendimento>
                {
                    new CartorioAtendimento { Id = 11, IdCartorio = 201, Titulo = "T11", Descricao = "D11", Ordem = 1 }
                }
            };
            var c2 = new Domain.Entities.ServicosEntity.CartorioEntity.Cartorio
            {
                Id = 202,
                Slug = "c-202",
                Ativo = true,
                TituloPagina = "C202",
                Atendimentos = new List<CartorioAtendimento>
                {
                    new CartorioAtendimento { Id = 12, IdCartorio = 202, Titulo = "T12", Descricao = "D12", Ordem = 1 },
                    new CartorioAtendimento { Id = 13, IdCartorio = 202, Titulo = "T13", Descricao = "D13", Ordem = 2 }
                }
            };
            db.Cartorio.AddRange(c1, c2);
            await db.SaveChangesAsync();

            var repo = new CartorioRepository(db);
            var all = await repo.AllWithChildrenAsync();

            Assert.NotNull(all);
            Assert.Equal(2, all.Count);
            Assert.All(all, c => Assert.NotNull(c.Atendimentos));
            Assert.Contains(all, c => c.Id == 202 && c.Atendimentos.Count == 2);
        }

        [Fact]
        public async Task DisableAsync_ReturnsFalse_WhenNotFound()
        {
            var db = CreateContext(nameof(DisableAsync_ReturnsFalse_WhenNotFound));
            var repo = new CartorioRepository(db);

            var result = await repo.DisableAsync(9999);

            Assert.False(result);
        }

        [Fact]
        public async Task DisableAsync_DisablesEntity_WhenFound()
        {
            var db = CreateContext(nameof(DisableAsync_DisablesEntity_WhenFound));
            var cart = new Domain.Entities.ServicosEntity.CartorioEntity.Cartorio
            {
                Id = 301,
                Slug = "c-301",
                Ativo = true,
                TituloPagina = "C301"
            };
            db.Cartorio.Add(cart);
            await db.SaveChangesAsync();

            var repo = new CartorioRepository(db);
            var result = await repo.DisableAsync(301);

            Assert.True(result);

            var reloaded = await db.Cartorio.FindAsync(301L);
            Assert.NotNull(reloaded);
            Assert.False(reloaded!.Ativo);
        }

        [Fact]
        public async Task GetWithChildrenBySlugAtivoAsync_ReturnsWhenActive_OtherwiseNull()
        {
            var db = CreateContext(nameof(GetWithChildrenBySlugAtivoAsync_ReturnsWhenActive_OtherwiseNull));
            var active = new Domain.Entities.ServicosEntity.CartorioEntity.Cartorio
            {
                Id = 401,
                Slug = "active-slug",
                Ativo = true,
                TituloPagina = "Active",
                Atendimentos = new List<CartorioAtendimento>
                {
                    new CartorioAtendimento { Id = 21, IdCartorio = 401, Titulo = "TA", Descricao = "DA", Ordem = 1 }
                }
            };
            var inactive = new Domain.Entities.ServicosEntity.CartorioEntity.Cartorio
            {
                Id = 402,
                Slug = "inactive-slug",
                Ativo = false,
                TituloPagina = "Inactive",
                Atendimentos = new List<CartorioAtendimento>
                {
                    new CartorioAtendimento { Id = 22, IdCartorio = 402, Titulo = "TI", Descricao = "DI", Ordem = 1 }
                }
            };
            db.Cartorio.AddRange(active, inactive);
            await db.SaveChangesAsync();

            var repo = new CartorioRepository(db);

            var foundActive = await repo.GetWithChildrenBySlugAtivoAsync("active-slug");
            Assert.NotNull(foundActive);
            Assert.Equal(401, foundActive!.Id);
            Assert.NotEmpty(foundActive.Atendimentos);

            var foundInactive = await repo.GetWithChildrenBySlugAtivoAsync("inactive-slug");
            Assert.Null(foundInactive);

            var foundMissing = await repo.GetWithChildrenBySlugAtivoAsync("missing");
            Assert.Null(foundMissing);
        }

        [Fact]
        public async Task GetBySlugAtivoAsync_ReturnsDetachedEntity_WhenActive()
        {
            var db = CreateContext(nameof(GetBySlugAtivoAsync_ReturnsDetachedEntity_WhenActive));
            var cart = new Domain.Entities.ServicosEntity.CartorioEntity.Cartorio
            {
                Id = 501,
                Slug = "detached-slug",
                Ativo = true,
                TituloPagina = "Detached"
            };
            db.Cartorio.Add(cart);
            await db.SaveChangesAsync();

            var repo = new CartorioRepository(db);
            var result = await repo.GetBySlugAtivoAsync("detached-slug");

            Assert.NotNull(result);
            Assert.Equal(501, result!.Id);

            var tracked = db.ChangeTracker.Entries<Domain.Entities.ServicosEntity.CartorioEntity.Cartorio>().FirstOrDefault(e => e.Entity.Id == 501);
            Assert.NotNull(tracked);
        }

        [Fact]
        public async Task ReplaceAtendimentosAsync_ReplacesExistingAndSetsIdCartorio()
        {
            var db = CreateContext(nameof(ReplaceAtendimentosAsync_ReplacesExistingAndSetsIdCartorio));
            var cart = new Domain.Entities.ServicosEntity.CartorioEntity.Cartorio
            {
                Id = 601,
                Slug = "rpl-slug",
                Ativo = true,
                TituloPagina = "Rpl"
            };
            var old1 = new CartorioAtendimento { Id = 31, IdCartorio = 601, Titulo = "Old1", Descricao = "o1", Ordem = 1 };
            var old2 = new CartorioAtendimento { Id = 32, IdCartorio = 601, Titulo = "Old2", Descricao = "o2", Ordem = 2 };
            db.Cartorio.Add(cart);
            db.CartorioAtendimento.AddRange(old1, old2);
            await db.SaveChangesAsync();

            var repo = new CartorioRepository(db);

            var novos = new List<CartorioAtendimento>
            {
                new CartorioAtendimento { Titulo = "New1", Descricao = "n1", Ordem = 1 },
                new CartorioAtendimento { Titulo = "New2", Descricao = "n2", Ordem = 2 }
            };

            await repo.ReplaceAtendimentosAsync(601, novos);

            var current = await db.CartorioAtendimento.Where(a => a.IdCartorio == 601).ToListAsync();

            Assert.Equal(2, current.Count);
            Assert.All(current, a => Assert.Equal(601, a.IdCartorio));
        }
    }
}