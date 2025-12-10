using Microsoft.EntityFrameworkCore;
using PortalTCMSP.Infra.Data.Context;
using PortalTCMSP.Unit.Tests.Repository.SessaoPlenaria.FixFeature;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Unit.Tests.Repository.SessaoPlenaria;

[ExcludeFromCodeCoverage]
public class SessaoPlenariaSustentacaoOralRepositoryTest
{
    private PortalTCMSPContext GetInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<PortalTCMSPContext>()
            .UseInMemoryDatabase("SessaoPlenariaRepoTestDB_" + Guid.NewGuid())
            .Options;
        return new PortalTCMSPContext(options);
    }

    [Fact]
    public async Task Add_SessaoPlenariaSustentacaoOral()
    {
        var context = GetInMemoryContext();
        var repo = context.Set<Domain.Entities.SessaoPlenariaEntity.SessaoPlenariaSustentacaoOral>();
        var sustentacao = SessaoPlenariaSustentacaoOralRepositoryFixture.GetSessaoPlenariaSustentacaoOral();
        
        await repo.AddAsync(sustentacao);
        await context.SaveChangesAsync();
        
        var sustentacaoFromDb = await repo.FirstOrDefaultAsync(s => s.Id == sustentacao.Id);
        Assert.NotNull(sustentacaoFromDb);
        Assert.Equal(sustentacao.Titulo, sustentacaoFromDb?.Titulo);
    }

    [Fact]
    public async Task Get_SessaoPlenariaSustentacaoOral_ById()
    {
        var context = GetInMemoryContext();
        var repo = context.Set<Domain.Entities.SessaoPlenariaEntity.SessaoPlenariaSustentacaoOral>();
        var sustentacao = SessaoPlenariaSustentacaoOralRepositoryFixture.GetSessaoPlenariaSustentacaoOral();

        await repo.AddAsync(sustentacao);
        await context.SaveChangesAsync();

        var found = await repo.FindAsync(sustentacao.Id);
        Assert.NotNull(found);
        Assert.Equal(sustentacao.Titulo, found?.Titulo);
    }

    [Fact]
    public async Task Update_SessaoPlenariaSustentacaoOral()
    {
        var context = GetInMemoryContext();
        var repo = context.Set<Domain.Entities.SessaoPlenariaEntity.SessaoPlenariaSustentacaoOral>();
        var sustentacao = SessaoPlenariaSustentacaoOralRepositoryFixture.GetSessaoPlenariaSustentacaoOral();

        await repo.AddAsync(sustentacao);
        await context.SaveChangesAsync();

        sustentacao.Titulo = "Novo Título";
        repo.Update(sustentacao);
        await context.SaveChangesAsync();

        var updated = await repo.FindAsync(sustentacao.Id);
        Assert.NotNull(updated);
        Assert.Equal("Novo Título", updated?.Titulo);
    }

    [Fact]
    public async Task Delete_SessaoPlenariaSustentacaoOral()
    {
        var context = GetInMemoryContext();
        var repo = context.Set<Domain.Entities.SessaoPlenariaEntity.SessaoPlenariaSustentacaoOral>();
        var sustentacao = SessaoPlenariaSustentacaoOralRepositoryFixture.GetSessaoPlenariaSustentacaoOral();

        await repo.AddAsync(sustentacao);
        await context.SaveChangesAsync();

        repo.Remove(sustentacao);
        await context.SaveChangesAsync();

        var deleted = await repo.FindAsync(sustentacao.Id);
        Assert.Null(deleted);
    }

    [Fact]
    public async Task List_SessaoPlenariaSustentacaoOral()
    {
        var context = GetInMemoryContext();
        var repo = context.Set<Domain.Entities.SessaoPlenariaEntity.SessaoPlenariaSustentacaoOral>();

        var sustentacao1 = SessaoPlenariaSustentacaoOralRepositoryFixture.GetSessaoPlenariaSustentacaoOral(1, 1);
        var sustentacao2 = SessaoPlenariaSustentacaoOralRepositoryFixture.GetSessaoPlenariaSustentacaoOral(2, 2);
        sustentacao2.Titulo = "Outro Título";

        await repo.AddAsync(sustentacao1);
        await repo.AddAsync(sustentacao2);
        await context.SaveChangesAsync();

        var all = await repo.ToListAsync();
        Assert.True(all.Count >= 2);
        Assert.Contains(all, s => s.Titulo == sustentacao1.Titulo);
        Assert.Contains(all, s => s.Titulo == sustentacao2.Titulo);
    }
}
