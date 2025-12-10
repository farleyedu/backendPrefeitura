using Microsoft.EntityFrameworkCore;
using PortalTCMSP.Domain.Entities.FiscalizacaoEntity.PlanoAnualFiscalizacao;
using PortalTCMSP.Infra.Data.Context;
using PortalTCMSP.Infra.Data.Repositories.Fiscalizacao.PlanoAnualFiscalizacao;

namespace PortalTCMSP.Unit.Tests.Repository.Fiscalizacao
{
    public class FiscalizacaoPlanoAnualFiscalizacaoRepositoryTests
    {
        private static PortalTCMSPContext CreateContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<PortalTCMSPContext>()
                .UseInMemoryDatabase(dbName)
                .Options;

            return new PortalTCMSPContext(options);
        }

        [Fact]
        public async Task FindBySlugAsync_ReturnsEntity_WhenExists()
        {
            var dbName = Guid.NewGuid().ToString();
            await using var ctx = CreateContext(dbName);
            var repo = new FiscalizacaoPlanoAnualFiscalizacaoRepository(ctx);

            var entity = new FiscalizacaoPlanoAnualFiscalizacaoResolucao { Id = 1, Slug = "my-slug" };
            ctx.Add(entity);
            await ctx.SaveChangesAsync();

            var res = await repo.FindBySlugAsync("my-slug");

            Assert.NotNull(res);
            Assert.Equal(1, res!.Id);
            Assert.Equal("my-slug", res.Slug);
        }

        [Fact]
        public async Task GetWithChildrenByIdAsync_IncludesChildren()
        {
            var dbName = Guid.NewGuid().ToString();
            await using var ctx = CreateContext(dbName);
            var repo = new FiscalizacaoPlanoAnualFiscalizacaoRepository(ctx);

            var resolucao = new FiscalizacaoPlanoAnualFiscalizacaoResolucao
            {
                Id = 10,
                Slug = "s-10",
                Ementa = new FiscalizacaoResolucaoEmenta
                {
                    Descritivo = "d",
                    LinksArtigos = new List<FiscalizacaoResolucaoEmentaLink> { new() }
                },
                Dispositivos = new List<FiscalizacaoResolucaoDispositivo>
                {
                    new()
                    {
                        Ordem = 1,
                        Paragrafo = new List<FiscalizacaoResolucaoParagrafo> { new() },
                        Incisos = new List<FiscalizacaoResolucaoInciso> { new() }
                    }
                },
                Anexos = new List<FiscalizacaoResolucaoAnexo>
                {
                    new()
                    {
                        Numero = 1,
                        TemasPrioritarios = new List<FiscalizacaoResolucaoTemaPrioritario> { new() },
                        Atividades = new List<FiscalizacaoResolucaoAtividade>
                        {
                            new() { AtividadeItem = new List<FiscalizacaoResolucaoAtividadeItem>() }
                        },
                        Distribuicao = new List<FiscalizacaoResolucaoDistribuicao>()
                    }
                },
                Atas = new List<FiscalizacaoResolucaoAta> { new() }
            };

            ctx.Add(resolucao);
            await ctx.SaveChangesAsync();

            var loaded = await repo.GetWithChildrenByIdAsync(10);

            Assert.NotNull(loaded);
            Assert.NotNull(loaded!.Ementa);
            Assert.NotNull(loaded.Ementa!.LinksArtigos);
            Assert.NotEmpty(loaded.Ementa.LinksArtigos);

            Assert.NotNull(loaded.Dispositivos);
            Assert.NotEmpty(loaded.Dispositivos);
            Assert.NotNull(loaded.Dispositivos.First().Paragrafo);
            Assert.NotNull(loaded.Dispositivos.First().Incisos);

            Assert.NotNull(loaded.Anexos);
            Assert.NotEmpty(loaded.Anexos);
            var firstAnexo = loaded.Anexos.First();
            Assert.NotNull(firstAnexo.TemasPrioritarios);
            Assert.NotNull(firstAnexo.Atividades);
            Assert.NotNull(firstAnexo.Distribuicao);

            Assert.NotNull(loaded.Atas);
            Assert.NotEmpty(loaded.Atas);
        }

        [Fact]
        public async Task GetWithChildrenBySlugAsync_IncludesChildren()
        {
            var dbName = Guid.NewGuid().ToString();
            await using var ctx = CreateContext(dbName);
            var repo = new FiscalizacaoPlanoAnualFiscalizacaoRepository(ctx);

            var resolucao = new FiscalizacaoPlanoAnualFiscalizacaoResolucao
            {
                Id = 11,
                Slug = "s-11",
                Ementa = new FiscalizacaoResolucaoEmenta
                {
                    LinksArtigos = new List<FiscalizacaoResolucaoEmentaLink> { new() }
                },
                Dispositivos = new List<FiscalizacaoResolucaoDispositivo> { new() },
                Anexos = new List<FiscalizacaoResolucaoAnexo> { new() },
                Atas = new List<FiscalizacaoResolucaoAta> { new() }
            };

            ctx.Add(resolucao);
            await ctx.SaveChangesAsync();

            var loaded = await repo.GetWithChildrenBySlugAsync("s-11");

            Assert.NotNull(loaded);
            Assert.Equal(11, loaded!.Id);
            Assert.NotNull(loaded.Ementa);
            Assert.NotNull(loaded.Anexos);
            Assert.NotNull(loaded.Atas);
        }

        [Fact]
        public async Task AllWithChildrenAsync_ReturnsAll_WithChildren()
        {
            var dbName = Guid.NewGuid().ToString();
            await using var ctx = CreateContext(dbName);
            var repo = new FiscalizacaoPlanoAnualFiscalizacaoRepository(ctx);

            var r1 = new FiscalizacaoPlanoAnualFiscalizacaoResolucao { Id = 21, Slug = "a", Ementa = new FiscalizacaoResolucaoEmenta { LinksArtigos = new List<FiscalizacaoResolucaoEmentaLink> { new() } } };
            var r2 = new FiscalizacaoPlanoAnualFiscalizacaoResolucao { Id = 22, Slug = "b", Ementa = new FiscalizacaoResolucaoEmenta { LinksArtigos = new List<FiscalizacaoResolucaoEmentaLink> { new() } } };

            ctx.AddRange(r1, r2);
            await ctx.SaveChangesAsync();

            var all = await repo.AllWithChildrenAsync();

            Assert.NotNull(all);
            Assert.Equal(2, all.Count);
            Assert.All(all, item => Assert.NotNull(item.Ementa));
            Assert.All(all, item => Assert.NotNull(item.Ementa!.LinksArtigos));
        }

        [Fact]
        public async Task ReplaceDispositivosAsync_ReplacesOld_WithNew()
        {
            var dbName = Guid.NewGuid().ToString();
            await using var ctx = CreateContext(dbName);
            var repo = new FiscalizacaoPlanoAnualFiscalizacaoRepository(ctx);

            var resolucaoId = 31L;
            ctx.Add(new FiscalizacaoResolucaoDispositivo { Id = 100, ResolucaoId = resolucaoId });
            await ctx.SaveChangesAsync();

            var novos = new List<FiscalizacaoResolucaoDispositivo>
            {
                new FiscalizacaoResolucaoDispositivo { Ordem = 1 }
            };

            await repo.ReplaceDispositivosAsync(resolucaoId, novos);
            await ctx.SaveChangesAsync();

            var dispositivos = await ctx.Set<FiscalizacaoResolucaoDispositivo>().Where(d => d.ResolucaoId == resolucaoId).ToListAsync();
            Assert.Single(dispositivos);
            Assert.Equal(resolucaoId, dispositivos.First().ResolucaoId);
        }

        [Fact]
        public async Task ReplaceAnexosAsync_ReplacesOld_WithNew()
        {
            var dbName = Guid.NewGuid().ToString();
            await using var ctx = CreateContext(dbName);
            var repo = new FiscalizacaoPlanoAnualFiscalizacaoRepository(ctx);

            var resolucaoId = 41L;
            ctx.Add(new FiscalizacaoResolucaoAnexo { Id = 200, ResolucaoId = resolucaoId });
            await ctx.SaveChangesAsync();

            var novos = new List<FiscalizacaoResolucaoAnexo> { new() { Numero = 1 } };

            await repo.ReplaceAnexosAsync(resolucaoId, novos);
            await ctx.SaveChangesAsync();

            var anexos = await ctx.Set<FiscalizacaoResolucaoAnexo>().Where(a => a.ResolucaoId == resolucaoId).ToListAsync();
            Assert.Single(anexos);
            Assert.Equal(resolucaoId, anexos.First().ResolucaoId);
        }

        [Fact]
        public async Task ReplaceAtasAsync_ReplacesOld_WithNew()
        {
            var dbName = Guid.NewGuid().ToString();
            await using var ctx = CreateContext(dbName);
            var repo = new FiscalizacaoPlanoAnualFiscalizacaoRepository(ctx);

            var resolucaoId = 51L;
            ctx.Add(new FiscalizacaoResolucaoAta { Id = 300, ResolucaoId = resolucaoId });
            await ctx.SaveChangesAsync();

            var novos = new List<FiscalizacaoResolucaoAta> { new() { Ordem = 1 } };

            await repo.ReplaceAtasAsync(resolucaoId, novos);
            await ctx.SaveChangesAsync();

            var atas = await ctx.Set<FiscalizacaoResolucaoAta>().Where(a => a.ResolucaoId == resolucaoId).ToListAsync();
            Assert.Single(atas);
            Assert.Equal(resolucaoId, atas.First().ResolucaoId);
        }

        [Fact]
        public async Task ReplaceEmentaAsync_RemovesOld_AndAddsNew()
        {
            var dbName = Guid.NewGuid().ToString();
            await using var ctx = CreateContext(dbName);
            var repo = new FiscalizacaoPlanoAnualFiscalizacaoRepository(ctx);

            var resolucaoId = 61L;

            var antiga = new FiscalizacaoResolucaoEmenta
            {
                Id = 400,
                ResolucaoId = resolucaoId,
                LinksArtigos = new List<FiscalizacaoResolucaoEmentaLink> { new() }
            };
            ctx.Add(antiga);
            await ctx.SaveChangesAsync();

            await repo.ReplaceEmentaAsync(resolucaoId, null);
            await ctx.SaveChangesAsync();

            var remaining = await ctx.Set<FiscalizacaoResolucaoEmenta>().Where(e => e.ResolucaoId == resolucaoId).ToListAsync();
            Assert.Empty(remaining);
            var remainingLinks = await ctx.Set<FiscalizacaoResolucaoEmentaLink>().Where(l => l.EmentaId == antiga.Id).ToListAsync();
            Assert.Empty(remainingLinks);

            var nova = new FiscalizacaoResolucaoEmenta
            {
                Descritivo = "new",
                LinksArtigos = new List<FiscalizacaoResolucaoEmentaLink> { new() }
            };

            await repo.ReplaceEmentaAsync(resolucaoId, nova);
            await ctx.SaveChangesAsync();

            var ementas = await ctx.Set<FiscalizacaoResolucaoEmenta>().Where(e => e.ResolucaoId == resolucaoId).ToListAsync();
            Assert.Single(ementas);
            Assert.Equal(resolucaoId, ementas.First().ResolucaoId);

            var linksAfter = await ctx.Set<FiscalizacaoResolucaoEmentaLink>().Where(l => l.EmentaId == ementas.First().Id).ToListAsync();
            Assert.NotEmpty(linksAfter);
        }
    }
}