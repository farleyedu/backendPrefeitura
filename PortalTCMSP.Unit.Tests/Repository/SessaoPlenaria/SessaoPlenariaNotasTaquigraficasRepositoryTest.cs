using Microsoft.EntityFrameworkCore;
using PortalTCMSP.Infra.Data.Context;
using PortalTCMSP.Unit.Tests.Repository.SessaoPlenaria.FixFeature;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Unit.Tests.Repository.SessaoPlenaria;

[ExcludeFromCodeCoverage]
public class SessaoPlenariaNotasTaquigraficasRepositoryTest
{
    private PortalTCMSPContext GetInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<PortalTCMSPContext>()
            .UseInMemoryDatabase("SessaoPlenariaRepoTestDB_" + Guid.NewGuid())
            .Options;
        return new PortalTCMSPContext(options);
    }

    [Fact]
    public async Task Add_SessaoPlenariaNotasTaquigraficas_Com_Anexos()
    {
        var context = GetInMemoryContext();
        var repo = context.Set<Domain.Entities.SessaoPlenariaEntity.SessaoPlenariaNotasTaquigraficas>();
        var notas = SessaoPlenariaNotasTaquigraficasRepositoryFixture.GetSessaoPlenariaNotasTaquigraficas();
        
        await repo.AddAsync(notas);
        await context.SaveChangesAsync();
        
        var notasFromDb = await repo.Include(a => a.Anexos).FirstOrDefaultAsync(a => a.Id == notas.Id);
        Assert.NotNull(notasFromDb);
        Assert.Equal(2, notasFromDb?.Anexos.Count);
    }

    [Fact]
    public async Task Add_SessaoPlenariaNotasTaquigraficas_Sem_Anexos()
    {
        var context = GetInMemoryContext();
        var repo = context.Set<Domain.Entities.SessaoPlenariaEntity.SessaoPlenariaNotasTaquigraficas>();
        var notas = SessaoPlenariaNotasTaquigraficasRepositoryFixture.GetSessaoPlenariaNotasTaquigraficas();
        notas.Anexos.Clear();

        await repo.AddAsync(notas);
        await context.SaveChangesAsync();

        var notasFromDb = await repo.Include(a => a.Anexos).FirstOrDefaultAsync(a => a.Id == notas.Id);
        Assert.NotNull(notasFromDb);
        Assert.Empty(notasFromDb?.Anexos ?? []);
    }

    [Fact]
    public async Task Update_SessaoPlenariaNotasTaquigraficas_Adiciona_Anexo()
    {
        var context = GetInMemoryContext();
        var repo = context.Set<Domain.Entities.SessaoPlenariaEntity.SessaoPlenariaNotasTaquigraficas>();
        var notas = SessaoPlenariaNotasTaquigraficasRepositoryFixture.GetSessaoPlenariaNotasTaquigraficas();
        notas.Anexos.Clear();

        await repo.AddAsync(notas);
        await context.SaveChangesAsync();

        notas.Anexos.Add(new Domain.Entities.SessaoPlenariaEntity.SessaoPlenariaNotasTaquigraficasAnexos
        {
            IdSessaoPlenariaNotasTaquigraficas = notas.Id
        });

        repo.Update(notas);
        await context.SaveChangesAsync();

        var notasFromDb = await repo.Include(a => a.Anexos).FirstOrDefaultAsync(a => a.Id == notas.Id);
        Assert.NotNull(notasFromDb);
        Assert.Single(notasFromDb?.Anexos ?? []);
    }

    [Fact]
    public async Task Remove_SessaoPlenariaNotasTaquigraficas()
    {
        var context = GetInMemoryContext();
        var repo = context.Set<Domain.Entities.SessaoPlenariaEntity.SessaoPlenariaNotasTaquigraficas>();
        var notas = SessaoPlenariaNotasTaquigraficasRepositoryFixture.GetSessaoPlenariaNotasTaquigraficas();

        await repo.AddAsync(notas);
        await context.SaveChangesAsync();

        repo.Remove(notas);
        await context.SaveChangesAsync();

        var notasFromDb = await repo.FirstOrDefaultAsync(a => a.Id == notas.Id);
        Assert.Null(notasFromDb);
    }

    [Fact]
    public async Task Get_SessaoPlenariaNotasTaquigraficas_Por_Id()
    {
        var context = GetInMemoryContext();
        var repo = context.Set<Domain.Entities.SessaoPlenariaEntity.SessaoPlenariaNotasTaquigraficas>();
        var notas = SessaoPlenariaNotasTaquigraficasRepositoryFixture.GetSessaoPlenariaNotasTaquigraficas();

        await repo.AddAsync(notas);
        await context.SaveChangesAsync();

        var notasFromDb = await repo.FindAsync(notas.Id);
        Assert.NotNull(notasFromDb);
        Assert.Equal(notas.Id, notasFromDb?.Id);
    }
}
