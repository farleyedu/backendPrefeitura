using Microsoft.EntityFrameworkCore;
using PortalTCMSP.Infra.Data.Context;
using PortalTCMSP.Unit.Tests.Repository.SessaoPlenaria.FixFeature;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Unit.Tests.Repository.SessaoPlenaria;

[ExcludeFromCodeCoverage]
public class SessaoPlenariaAtaRepositoryTest
{
    private PortalTCMSPContext GetInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<PortalTCMSPContext>()
            .UseInMemoryDatabase("SessaoPlenariaRepoTestDB_" + Guid.NewGuid())
            .Options;
        return new PortalTCMSPContext(options);
    }

    [Fact]
    public async Task Add_SessaoPlenariaAta_Com_Anexos()
    {
        var context = GetInMemoryContext();
        var repo = context.Set<Domain.Entities.SessaoPlenariaEntity.SessaoPlenariaAta>();
        var ata = SessaoPlenariaAtaRepositoryFixture.GetSessaoPlenariaAta();

        await repo.AddAsync(ata);
        await context.SaveChangesAsync();

        var ataFromDb = await repo.Include(a => a.Anexos).FirstOrDefaultAsync(a => a.Id == ata.Id);
        Assert.NotNull(ataFromDb);
        Assert.Equal(2, ataFromDb?.Anexos.Count);
    }

    [Fact]
    public async Task Add_SessaoPlenariaAta_Sem_Anexos()
    {
        var context = GetInMemoryContext();
        var repo = context.Set<Domain.Entities.SessaoPlenariaEntity.SessaoPlenariaAta>();
        var ata = SessaoPlenariaAtaRepositoryFixture.GetSessaoPlenariaAta();
        ata.Anexos.Clear();

        await repo.AddAsync(ata);
        await context.SaveChangesAsync();

        var ataFromDb = await repo.Include(a => a.Anexos).FirstOrDefaultAsync(a => a.Id == ata.Id);
        Assert.NotNull(ataFromDb);
        Assert.Empty(ataFromDb?.Anexos ?? []);
    }

    [Fact]
    public async Task Atualiza_SessaoPlenariaAta_Adiciona_Anexo()
    {
        var context = GetInMemoryContext();
        var repo = context.Set<Domain.Entities.SessaoPlenariaEntity.SessaoPlenariaAta>();
        var ata = SessaoPlenariaAtaRepositoryFixture.GetSessaoPlenariaAta();
        ata.Anexos.Clear();
        await repo.AddAsync(ata);
        await context.SaveChangesAsync();

        var novoAnexo = new Domain.Entities.SessaoPlenariaEntity.SessaoPlenariaAtaAnexo { IdSessaoPlenariaAta = ata.Id };
        ata.Anexos.Add(novoAnexo);
        repo.Update(ata);
        await context.SaveChangesAsync();

        var ataFromDb = await repo.Include(a => a.Anexos).FirstOrDefaultAsync(a => a.Id == ata.Id);
        Assert.NotNull(ataFromDb);
        Assert.Single(ataFromDb?.Anexos ?? []);
    }

    [Fact]
    public async Task Remove_SessaoPlenariaAta()
    {
        var context = GetInMemoryContext();
        var repo = context.Set<Domain.Entities.SessaoPlenariaEntity.SessaoPlenariaAta>();
        var ata = SessaoPlenariaAtaRepositoryFixture.GetSessaoPlenariaAta();
        await repo.AddAsync(ata);
        await context.SaveChangesAsync();

        repo.Remove(ata);
        await context.SaveChangesAsync();

        var ataFromDb = await repo.FirstOrDefaultAsync(a => a.Id == ata.Id);
        Assert.Null(ataFromDb);
    }

    [Fact]
    public async Task Consulta_SessaoPlenariaAta_Por_Id()
    {
        var context = GetInMemoryContext();
        var repo = context.Set<Domain.Entities.SessaoPlenariaEntity.SessaoPlenariaAta>();
        var ata = SessaoPlenariaAtaRepositoryFixture.GetSessaoPlenariaAta();
        await repo.AddAsync(ata);
        await context.SaveChangesAsync();

        var ataFromDb = await repo.FindAsync(ata.Id);
        Assert.NotNull(ataFromDb);
        Assert.Equal(ata.Id, ataFromDb?.Id);
    }
}
