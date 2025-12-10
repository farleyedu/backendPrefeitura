using Microsoft.EntityFrameworkCore;
using PortalTCMSP.Infra.Data.Context;
using PortalTCMSP.Unit.Tests.Repository.SessaoPlenaria.FixFeature;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Unit.Tests.Repository.SessaoPlenaria;

[ExcludeFromCodeCoverage]
public class SessaoPlenariaPautaRepositoryTest
{
    private PortalTCMSPContext GetInMemoryContext()
    {
        var options = new DbContextOptionsBuilder<PortalTCMSPContext>()
            .UseInMemoryDatabase("SessaoPlenariaRepoTestDB_" + Guid.NewGuid())
            .Options;
        return new PortalTCMSPContext(options);
    }

    [Fact]
    public async Task Add_SessaoPlenariaPauta_Com_Anexos()
    {
        var context = GetInMemoryContext();
        var repo = context.Set<Domain.Entities.SessaoPlenariaEntity.SessaoPlenariaPauta>();
        var pauta = SessaoPlenariaPautaRepositoryFixture.GetSessaoPlenariaPauta();
        
        await repo.AddAsync(pauta);
        await context.SaveChangesAsync();
        
        var pautaFromDb = await repo.Include(a => a.Anexos).FirstOrDefaultAsync(a => a.Id == pauta.Id);
        Assert.NotNull(pautaFromDb);
        Assert.Equal(2, pautaFromDb?.Anexos.Count);
    }

    [Fact]
    public async Task Add_SessaoPlenariaPauta_Sem_Anexos()
    {
        var context = GetInMemoryContext();
        var repo = context.Set<Domain.Entities.SessaoPlenariaEntity.SessaoPlenariaPauta>();
        var pauta = SessaoPlenariaPautaRepositoryFixture.GetSessaoPlenariaPauta();
        pauta.Anexos.Clear();

        await repo.AddAsync(pauta);
        await context.SaveChangesAsync();

        var pautaFromDb = await repo.Include(a => a.Anexos).FirstOrDefaultAsync(a => a.Id == pauta.Id);
        Assert.NotNull(pautaFromDb);
        Assert.Empty(pautaFromDb?.Anexos ?? []);
    }

    [Fact]
    public async Task Update_SessaoPlenariaPauta_Adiciona_Anexo()
    {
        var context = GetInMemoryContext();
        var repo = context.Set<Domain.Entities.SessaoPlenariaEntity.SessaoPlenariaPauta>();
        var pauta = SessaoPlenariaPautaRepositoryFixture.GetSessaoPlenariaPauta();
        pauta.Anexos.Clear();

        await repo.AddAsync(pauta);
        await context.SaveChangesAsync();

        pauta.Anexos.Add(new Domain.Entities.SessaoPlenariaEntity.SessaoPlenariaPautaAnexo { IdSessaoPlenariaPauta = pauta.Id });
        repo.Update(pauta);
        await context.SaveChangesAsync();

        var pautaFromDb = await repo.Include(a => a.Anexos).FirstOrDefaultAsync(a => a.Id == pauta.Id);
        Assert.NotNull(pautaFromDb);
        Assert.Single(pautaFromDb?.Anexos ?? []);
    }

    [Fact]
    public async Task Remove_SessaoPlenariaPauta()
    {
        var context = GetInMemoryContext();
        var repo = context.Set<Domain.Entities.SessaoPlenariaEntity.SessaoPlenariaPauta>();
        var pauta = SessaoPlenariaPautaRepositoryFixture.GetSessaoPlenariaPauta();

        await repo.AddAsync(pauta);
        await context.SaveChangesAsync();

        repo.Remove(pauta);
        await context.SaveChangesAsync();

        var pautaFromDb = await repo.FirstOrDefaultAsync(a => a.Id == pauta.Id);
        Assert.Null(pautaFromDb);
    }
}
