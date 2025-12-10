using Microsoft.EntityFrameworkCore;
using PortalTCMSP.Domain.Repositories.SessaoPlenaria;
using PortalTCMSP.Infra.Data.Context;
using PortalTCMSP.Unit.Tests.Repository.SessaoPlenaria.FixFeature;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Unit.Tests.Repository.SessaoPlenaria;

[ExcludeFromCodeCoverage]
public class SessaoPlenariaRepositoryTest
{
    private PortalTCMSPContext GetInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<PortalTCMSPContext>()
            .UseInMemoryDatabase("SessaoPlenariaRepoTestDB_" + Guid.NewGuid())
            .Options;
        return new PortalTCMSPContext(options);
    }

    [Fact]
    public async Task FindBySlugAsync_Deve_Retornar_Categoria_Quando_Existir()
    {
        using var context = GetInMemoryContext();
        var repo = new SessaoPlenariaRepository(context);

        var sessoes = SessaoPlenariaRepositoryFixture.GetSessoes();
        context.SessaoPlenaria.AddRange(sessoes);
        await context.SaveChangesAsync();

        var result = await repo.FindBySlugAsync("sessao-plenaria-1");
        Assert.NotNull(result);
        Assert.Equal("Sessão 1", result?.Titulo);

    }

    [Fact]
    public async Task FindBySlugAsync_Deve_Retornar_Null_Quando_Nao_Existir()
    {
        using var context = GetInMemoryContext();
        var repo = new SessaoPlenariaRepository(context);

        var sessoes = SessaoPlenariaRepositoryFixture.GetSessoes();
        context.SessaoPlenaria.AddRange(sessoes);
        await context.SaveChangesAsync();

        var result = await repo.FindBySlugAsync("slug-inexistente");
        Assert.Null(result);
    }

    [Fact]
    public async Task GetWithChildrenByIdAsync_Deve_Retornar_Sessao_Com_Filhos_Quando_Existir()
    {
        using var context = GetInMemoryContext();
        var sessoes = SessaoPlenariaRepositoryFixture.GetSessoes();
        context.SessaoPlenaria.AddRange(sessoes);
        await context.SaveChangesAsync();

        var repo = new SessaoPlenariaRepository(context);
        var sessao = sessoes.First();
        var result = await repo.GetWithChildrenByIdAsync(sessao.Id);

        Assert.NotNull(result);
        Assert.NotEmpty(result.Pautas);
        Assert.NotEmpty(result.Atas);
        Assert.NotEmpty(result.NotasTaquigraficas);
    }

    [Fact]
    public async Task GetWithChildrenBySlugAsync_Deve_Retornar_Sessao_Com_Filhos_Quando_Existir()
    {
        using var context = GetInMemoryContext();
        var sessoes = SessaoPlenariaRepositoryFixture.GetSessoes();
        context.SessaoPlenaria.AddRange(sessoes);
        await context.SaveChangesAsync();

        var repo = new SessaoPlenariaRepository(context);
        var sessao = sessoes.First();
        var result = await repo.GetWithChildrenBySlugAsync(sessao.Slug);

        Assert.NotNull(result);
        Assert.NotEmpty(result.Pautas);
        Assert.NotEmpty(result.Atas);
        Assert.NotEmpty(result.NotasTaquigraficas);
    }

    [Fact]
    public async Task AllWithChildrenAsync_Deve_Retornar_Todas_Sessoes_Com_Filhos()
    {
        using var context = GetInMemoryContext();
        var sessoes = SessaoPlenariaRepositoryFixture.GetSessoes();
        context.SessaoPlenaria.AddRange(sessoes);
        await context.SaveChangesAsync();

        var repo = new SessaoPlenariaRepository(context);
        var result = await repo.AllWithChildrenAsync();

        Assert.NotEmpty(result);
        Assert.All(result, s =>
        {
            Assert.NotEmpty(s.Pautas);
            Assert.NotEmpty(s.Atas);
            Assert.NotEmpty(s.NotasTaquigraficas);
        });
    }

    [Fact]
    public async Task DeactivateAllAsync_Deve_Desativar_Todas_Sessoes()
    {
        using var context = GetInMemoryContext();
        var sessoes = SessaoPlenariaRepositoryFixture.GetSessoes();
        context.SessaoPlenaria.AddRange(sessoes);
        await context.SaveChangesAsync();

        var repo = new SessaoPlenariaRepository(context);
        await repo.DeactivateAllAsync();
        await context.SaveChangesAsync();

        var ativos = await repo.GetAtivosAsync();
        Assert.Empty(ativos);
    }

    [Fact]
    public async Task DeactivateAllExceptAsync_Deve_Desativar_Todas_Menos_Uma()
    {
        using var context = GetInMemoryContext();
        var sessoes = SessaoPlenariaRepositoryFixture.GetSessoes();
        context.SessaoPlenaria.AddRange(sessoes);
        await context.SaveChangesAsync();

        var repo = new SessaoPlenariaRepository(context);
        var exceptId = sessoes.First().Id;
        await repo.DeactivateAllExceptAsync(exceptId);
        await context.SaveChangesAsync();

        var ativos = await repo.GetAtivosAsync();
        Assert.Single(ativos);
        Assert.Equal(exceptId, ativos.First().Id);
    }

    [Fact]
    public async Task GetAtivosAsync_Deve_Retornar_Apenas_Sessoes_Ativas()
    {
        using var context = GetInMemoryContext();
        var sessoes = SessaoPlenariaRepositoryFixture.GetSessoes();
        context.SessaoPlenaria.AddRange(sessoes);
        await context.SaveChangesAsync();

        var repo = new SessaoPlenariaRepository(context);
        var ativos = await repo.GetAtivosAsync();

        Assert.True(ativos.Count == sessoes.Count);
    }
}
