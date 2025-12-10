using Microsoft.EntityFrameworkCore;
using PortalTCMSP.Infra.Data.Context;
using PortalTCMSP.Unit.Tests.Repository.SessaoPlenaria.FixFeature;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Unit.Tests.Repository.SessaoPlenaria;

[ExcludeFromCodeCoverage]
public class SessaoPlenariaJurisprudenciaRepositoryTest
{
    private PortalTCMSPContext GetInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<PortalTCMSPContext>()
            .UseInMemoryDatabase("SessaoPlenariaRepoTestDB_" + Guid.NewGuid())
            .Options;
        return new PortalTCMSPContext(options);
    }

    [Fact]
    public async Task Add_SessaoPlenariaJurisprudencia()
    {
        var context = GetInMemoryContext();
        var repo = context.Set<Domain.Entities.SessaoPlenariaEntity.SessaoPlenariaJurispudencia>();
        var jurisprudencia = SessaoPlenariaJurisprudenciaRepositoryFixture.GetSessaoPlenariaJurisprudencia();
        
        await repo.AddAsync(jurisprudencia);
        await context.SaveChangesAsync();
        
        var jurisprudenciaFromDb = await repo.FirstOrDefaultAsync(j => j.Id == jurisprudencia.Id);
        Assert.NotNull(jurisprudenciaFromDb);
        Assert.Equal(jurisprudencia.Titulo, jurisprudenciaFromDb?.Titulo);
    }

    [Fact]
    public async Task Get_SessaoPlenariaJurisprudencia_ById()
    {
        var context = GetInMemoryContext();
        var repo = context.Set<Domain.Entities.SessaoPlenariaEntity.SessaoPlenariaJurispudencia>();
        var jurisprudencia = SessaoPlenariaJurisprudenciaRepositoryFixture.GetSessaoPlenariaJurisprudencia();

        await repo.AddAsync(jurisprudencia);
        await context.SaveChangesAsync();

        var found = await repo.FindAsync(jurisprudencia.Id);
        Assert.NotNull(found);
        Assert.Equal(jurisprudencia.Titulo, found?.Titulo);
    }

    [Fact]
    public async Task Update_SessaoPlenariaJurisprudencia()
    {
        var context = GetInMemoryContext();
        var repo = context.Set<Domain.Entities.SessaoPlenariaEntity.SessaoPlenariaJurispudencia>();
        var jurisprudencia = SessaoPlenariaJurisprudenciaRepositoryFixture.GetSessaoPlenariaJurisprudencia();

        await repo.AddAsync(jurisprudencia);
        await context.SaveChangesAsync();

        jurisprudencia.Titulo = "Novo Título";
        repo.Update(jurisprudencia);
        await context.SaveChangesAsync();

        var updated = await repo.FindAsync(jurisprudencia.Id);
        Assert.NotNull(updated);
        Assert.Equal("Novo Título", updated?.Titulo);
    }

    [Fact]
    public async Task Delete_SessaoPlenariaJurisprudencia()
    {
        var context = GetInMemoryContext();
        var repo = context.Set<Domain.Entities.SessaoPlenariaEntity.SessaoPlenariaJurispudencia>();
        var jurisprudencia = SessaoPlenariaJurisprudenciaRepositoryFixture.GetSessaoPlenariaJurisprudencia();

        await repo.AddAsync(jurisprudencia);
        await context.SaveChangesAsync();

        repo.Remove(jurisprudencia);
        await context.SaveChangesAsync();

        var deleted = await repo.FindAsync(jurisprudencia.Id);
        Assert.Null(deleted);
    }
}
