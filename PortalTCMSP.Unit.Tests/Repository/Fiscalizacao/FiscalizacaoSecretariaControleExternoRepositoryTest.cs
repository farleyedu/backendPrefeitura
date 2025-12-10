using Microsoft.EntityFrameworkCore;
using PortalTCMSP.Domain.Entities.FiscalizacaoEntity.SecretariaControleExterno;
using PortalTCMSP.Infra.Data.Context;
using PortalTCMSP.Infra.Data.Repositories.Fiscalizacao;

namespace PortalTCMSP.Unit.Tests.Repository.Fiscalizacao;

public class FiscalizacaoSecretariaControleExternoRepositoryTest
{
    private PortalTCMSPContext GetInMemoryContext(string? dbName = null)
    {
        var options = new DbContextOptionsBuilder<PortalTCMSPContext>()
            .UseInMemoryDatabase(dbName ?? "FiscalizacaoSecretariaControleExternoRepoTestDB_" + Guid.NewGuid())
            .Options;
        return new PortalTCMSPContext(options);
    }

    [Fact]
    public async Task FindBySlugAsync_ReturnsEntity_WhenExists()
    {
        using var context = GetInMemoryContext();
        var repo = new FiscalizacaoSecretariaControleExternoRepository(context);

        var entity = new FiscalizacaoSecretariaControleExterno
        {
            Slug = "slug-1",
            Titulos = new List<FiscalizacaoSecretariaSecaoConteudoTitulo>(),
            Carrossel = new List<FiscalizacaoSecretariaSecaoConteudoCarrosselItem>()
        };

        context.FiscalizacaoSecretariaControleExterno.Add(entity);
        await context.SaveChangesAsync();

        var result = await repo.FindBySlugAsync("slug-1");

        Assert.NotNull(result);
        Assert.Equal("slug-1", result!.Slug);
    }

    [Fact]
    public async Task FindBySlugAsync_ReturnsNull_WhenNotExists()
    {
        using var context = GetInMemoryContext();
        var repo = new FiscalizacaoSecretariaControleExternoRepository(context);

        var entity = new FiscalizacaoSecretariaControleExterno
        {
            Slug = "slug-exists",
            Titulos = new List<FiscalizacaoSecretariaSecaoConteudoTitulo>(),
            Carrossel = new List<FiscalizacaoSecretariaSecaoConteudoCarrosselItem>()
        };

        context.FiscalizacaoSecretariaControleExterno.Add(entity);
        await context.SaveChangesAsync();

        var result = await repo.FindBySlugAsync("slug-not-found");

        Assert.Null(result);
    }

    [Fact]
    public async Task GetWithChildrenByIdAsync_ReturnsEntityWithChildren_WhenExists()
    {
        using var context = GetInMemoryContext();
        var repo = new FiscalizacaoSecretariaControleExternoRepository(context);

        var parent = new FiscalizacaoSecretariaControleExterno
        {
            Slug = "parent-1",
            Titulos = new List<FiscalizacaoSecretariaSecaoConteudoTitulo>(),
            Carrossel = new List<FiscalizacaoSecretariaSecaoConteudoCarrosselItem>()
        };

        context.FiscalizacaoSecretariaControleExterno.Add(parent);
        await context.SaveChangesAsync();

        var titulo1 = new FiscalizacaoSecretariaSecaoConteudoTitulo { IdSecaoConteudo = parent.Id, Conteudo = parent };
        var titulo2 = new FiscalizacaoSecretariaSecaoConteudoTitulo { IdSecaoConteudo = parent.Id, Conteudo = parent };
        var car1 = new FiscalizacaoSecretariaSecaoConteudoCarrosselItem { IdSecaoConteudo = parent.Id, Conteudo = parent };

        context.FiscalizacaoSecretariaSecaoConteudoTitulo.AddRange(titulo1, titulo2);
        context.FiscalizacaoSecretariaSecaoConteudoCarrosselItem.Add(car1);
        await context.SaveChangesAsync();

        var result = await repo.GetWithChildrenByIdAsync(parent.Id);

        Assert.NotNull(result);
        Assert.NotEmpty(result!.Titulos);
        Assert.NotEmpty(result.Carrossel);
        Assert.Equal(2, result.Titulos.Count);
        Assert.Single(result.Carrossel);
    }

    [Fact]
    public async Task GetWithChildrenBySlugAsync_ReturnsEntityWithChildren_WhenExists()
    {
        using var context = GetInMemoryContext();
        var repo = new FiscalizacaoSecretariaControleExternoRepository(context);

        var parent = new FiscalizacaoSecretariaControleExterno
        {
            Slug = "parent-slug",
            Titulos = new List<FiscalizacaoSecretariaSecaoConteudoTitulo>(),
            Carrossel = new List<FiscalizacaoSecretariaSecaoConteudoCarrosselItem>()
        };

        context.FiscalizacaoSecretariaControleExterno.Add(parent);
        await context.SaveChangesAsync();

        var titulo = new FiscalizacaoSecretariaSecaoConteudoTitulo { IdSecaoConteudo = parent.Id, Conteudo = parent };
        var car = new FiscalizacaoSecretariaSecaoConteudoCarrosselItem { IdSecaoConteudo = parent.Id, Conteudo = parent };

        context.FiscalizacaoSecretariaSecaoConteudoTitulo.Add(titulo);
        context.FiscalizacaoSecretariaSecaoConteudoCarrosselItem.Add(car);
        await context.SaveChangesAsync();

        var result = await repo.GetWithChildrenBySlugAsync("parent-slug");

        Assert.NotNull(result);
        Assert.NotEmpty(result!.Titulos);
        Assert.NotEmpty(result.Carrossel);
    }

    [Fact]
    public async Task AllWithChildrenAsync_ReturnsAllWithChildren()
    {
        using var context = GetInMemoryContext();
        var repo = new FiscalizacaoSecretariaControleExternoRepository(context);

        var p1 = new FiscalizacaoSecretariaControleExterno
        {
            Slug = "p1",
            Titulos = new List<FiscalizacaoSecretariaSecaoConteudoTitulo>(),
            Carrossel = new List<FiscalizacaoSecretariaSecaoConteudoCarrosselItem>()
        };
        var p2 = new FiscalizacaoSecretariaControleExterno
        {
            Slug = "p2",
            Titulos = new List<FiscalizacaoSecretariaSecaoConteudoTitulo>(),
            Carrossel = new List<FiscalizacaoSecretariaSecaoConteudoCarrosselItem>()
        };

        context.FiscalizacaoSecretariaControleExterno.AddRange(p1, p2);
        await context.SaveChangesAsync();

        context.FiscalizacaoSecretariaSecaoConteudoTitulo.Add(new FiscalizacaoSecretariaSecaoConteudoTitulo { IdSecaoConteudo = p1.Id, Conteudo = p1 });
        context.FiscalizacaoSecretariaSecaoConteudoCarrosselItem.Add(new FiscalizacaoSecretariaSecaoConteudoCarrosselItem { IdSecaoConteudo = p2.Id, Conteudo = p2 });
        await context.SaveChangesAsync();

        var all = await repo.AllWithChildrenAsync();

        Assert.Equal(2, all.Count);
        Assert.True(all.Any(a => a.Titulos != null));
        Assert.True(all.Any(a => a.Carrossel != null));
    }

    [Fact]
    public async Task ReplaceTitulosAsync_RemovesOldAndAddsNew()
    {
        using var context = GetInMemoryContext();
        var repo = new FiscalizacaoSecretariaControleExternoRepository(context);

        var parent = new FiscalizacaoSecretariaControleExterno
        {
            Slug = "replace-titulos",
            Titulos = new List<FiscalizacaoSecretariaSecaoConteudoTitulo>(),
            Carrossel = new List<FiscalizacaoSecretariaSecaoConteudoCarrosselItem>()
        };

        context.FiscalizacaoSecretariaControleExterno.Add(parent);
        await context.SaveChangesAsync();

        var old1 = new FiscalizacaoSecretariaSecaoConteudoTitulo { IdSecaoConteudo = parent.Id, Conteudo = parent };
        var old2 = new FiscalizacaoSecretariaSecaoConteudoTitulo { IdSecaoConteudo = parent.Id, Conteudo = parent };
        context.FiscalizacaoSecretariaSecaoConteudoTitulo.AddRange(old1, old2);
        await context.SaveChangesAsync();

        var novos = new[]
        {
            new FiscalizacaoSecretariaSecaoConteudoTitulo(),
            new FiscalizacaoSecretariaSecaoConteudoTitulo()
        };

        await repo.ReplaceTitulosAsync(parent.Id, novos);
        // method does not commit; persist changes to verify repository behavior
        await context.SaveChangesAsync();

        var remaining = await context.FiscalizacaoSecretariaSecaoConteudoTitulo
            .Where(t => t.IdSecaoConteudo == parent.Id)
            .ToListAsync();

        Assert.Equal(2, remaining.Count);
        Assert.DoesNotContain(remaining, r => r.Id == old1.Id || r.Id == old2.Id);
        Assert.All(remaining, r => Assert.Equal(parent.Id, r.IdSecaoConteudo));
    }

    [Fact]
    public async Task ReplaceCarrosselAsync_RemovesOldAndAddsNew()
    {
        using var context = GetInMemoryContext();
        var repo = new FiscalizacaoSecretariaControleExternoRepository(context);

        var parent = new FiscalizacaoSecretariaControleExterno
        {
            Slug = "replace-carrossel",
            Titulos = new List<FiscalizacaoSecretariaSecaoConteudoTitulo>(),
            Carrossel = new List<FiscalizacaoSecretariaSecaoConteudoCarrosselItem>()
        };

        context.FiscalizacaoSecretariaControleExterno.Add(parent);
        await context.SaveChangesAsync();

        var old1 = new FiscalizacaoSecretariaSecaoConteudoCarrosselItem { IdSecaoConteudo = parent.Id, Conteudo = parent };
        var old2 = new FiscalizacaoSecretariaSecaoConteudoCarrosselItem { IdSecaoConteudo = parent.Id, Conteudo = parent };
        context.FiscalizacaoSecretariaSecaoConteudoCarrosselItem.AddRange(old1, old2);
        await context.SaveChangesAsync();

        var novos = new[]
        {
            new FiscalizacaoSecretariaSecaoConteudoCarrosselItem(),
            new FiscalizacaoSecretariaSecaoConteudoCarrosselItem()
        };

        await repo.ReplaceCarrosselAsync(parent.Id, novos);
        await context.SaveChangesAsync();

        var remaining = await context.FiscalizacaoSecretariaSecaoConteudoCarrosselItem
            .Where(c => c.IdSecaoConteudo == parent.Id)
            .ToListAsync();

        Assert.Equal(2, remaining.Count);
        Assert.DoesNotContain(remaining, r => r.Id == old1.Id || r.Id == old2.Id);
        Assert.All(remaining, r => Assert.Equal(parent.Id, r.IdSecaoConteudo));
    }
}