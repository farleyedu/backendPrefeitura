using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using PortalTCMSP.Domain.Entities.ServicosEntity.MultasProcedimentosEntity;
using PortalTCMSP.Infra.Data.Context;
using PortalTCMSP.Infra.Data.Repositories.Servicos.MultasProcedimentos;

namespace PortalTCMSP.Infra.Data.Tests.Repositories.Servicos.MultasProcedimentos
{
    public class MultasProcedimentosPortariaRelacionadaRepositoryTests
    {
        private static PortalTCMSPContext CreateInMemoryContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<PortalTCMSPContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;
            return new PortalTCMSPContext(options);
        }

        [Fact]
        public async Task CreatePortariaRelacionadaAsync_Adds_New_Items_To_Existing_Collection()
        {
            var dbName = Guid.NewGuid().ToString();
            await using var context = CreateInMemoryContext(dbName);
            var mp = new Domain.Entities.ServicosEntity.MultasProcedimentosEntity.MultasProcedimentos
            {
                Id = 1,
                PortariasRelacionadas = new List<MultasProcedimentosPortariaRelacionada>()
            };
            context.MultasProcedimentos.Add(mp);
            await context.SaveChangesAsync();

            var repo = new MultasProcedimentosPortariaRelacionadaRepository(context);

            var novos = new[]
            {
                new MultasProcedimentosPortariaRelacionada { Titulo = "P1", Url = "u1", Ordem = 1, IdMultasProcedimentos = 1 },
                new MultasProcedimentosPortariaRelacionada { Titulo = "P2", Url = "u2", Ordem = 2, IdMultasProcedimentos = 1 }
            };

            await repo.CreatePortariaRelacionadaAsync(1, novos);

            var entity = await context.MultasProcedimentos
                .Include(m => m.PortariasRelacionadas)
                .FirstAsync(m => m.Id == 1);

            entity.PortariasRelacionadas.Should().HaveCount(2);
            entity.PortariasRelacionadas.Select(p => p.Titulo).Should().Contain(new[] { "P1", "P2" });
        }

        [Fact]
        public async Task CreatePortariaRelacionadaAsync_Initializes_Null_Collection_And_Adds_Items()
        {
            var dbName = Guid.NewGuid().ToString();
            await using var context = CreateInMemoryContext(dbName);
            var mp = new Domain.Entities.ServicosEntity.MultasProcedimentosEntity.MultasProcedimentos
            {
                Id = 2,
                PortariasRelacionadas = null 
            };
            context.MultasProcedimentos.Add(mp);
            await context.SaveChangesAsync();

            var repo = new MultasProcedimentosPortariaRelacionadaRepository(context);

            var novos = new[]
            {
                new MultasProcedimentosPortariaRelacionada { Titulo = "P3", Url = "u3", Ordem = 1, IdMultasProcedimentos = 2 }
            };

            await repo.CreatePortariaRelacionadaAsync(2, novos);

            var entity = await context.MultasProcedimentos
                .Include(m => m.PortariasRelacionadas)
                .FirstAsync(m => m.Id == 2);

            entity.PortariasRelacionadas.Should().HaveCount(1);
            entity.PortariasRelacionadas.First().Titulo.Should().Be("P3");
        }

        [Fact]
        public async Task CreatePortariaRelacionadaAsync_Throws_When_Id_Not_Found()
        {
            var dbName = Guid.NewGuid().ToString();
            await using var context = CreateInMemoryContext(dbName);
            var repo = new MultasProcedimentosPortariaRelacionadaRepository(context);

            var novos = Array.Empty<MultasProcedimentosPortariaRelacionada>();

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                repo.CreatePortariaRelacionadaAsync(999, novos));

            ex.Message.Should().Contain("PortariaRelacionada with Id 999 not found.");
        }

        [Fact]
        public async Task UpdatePortariaRelacionadaAsync_Throws_When_Id_Not_Found()
        {
            var dbName = Guid.NewGuid().ToString();
            await using var context = CreateInMemoryContext(dbName);
            var repo = new MultasProcedimentosPortariaRelacionadaRepository(context);

            var novos = Array.Empty<MultasProcedimentosPortariaRelacionada>();

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                repo.UpdatePortariaRelacionadaAsync(888, novos));

            ex.Message.Should().Contain("MultasProcedimentos with Id 888 not found.");
        }

        [Fact]
        public async Task UpdatePortariaRelacionadaAsync_Does_Not_Modify_Collection_As_Implemented()
        {
            var dbName = Guid.NewGuid().ToString();
            await using var context = CreateInMemoryContext(dbName);

            var existing = new Domain.Entities.ServicosEntity.MultasProcedimentosEntity.MultasProcedimentos
            {
                Id = 3,
                PortariasRelacionadas = new List<MultasProcedimentosPortariaRelacionada>
                {
                    new MultasProcedimentosPortariaRelacionada { Titulo = "Existing", Url = "ex", Ordem = 1, IdMultasProcedimentos = 3 }
                }
            };
            context.MultasProcedimentos.Add(existing);
            await context.SaveChangesAsync();

            var repo = new MultasProcedimentosPortariaRelacionadaRepository(context);

            var novos = new[]
            {
                new MultasProcedimentosPortariaRelacionada { Titulo = "New1", Url = "n1", Ordem = 2, IdMultasProcedimentos = 3 },
                new MultasProcedimentosPortariaRelacionada { Titulo = "New2", Url = "n2", Ordem = 3, IdMultasProcedimentos = 3 }
            };

            await repo.UpdatePortariaRelacionadaAsync(3, novos);

            var entity = await context.MultasProcedimentos
                .Include(m => m.PortariasRelacionadas)
                .FirstAsync(m => m.Id == 3);

            entity.PortariasRelacionadas.Should().HaveCount(1);
            entity.PortariasRelacionadas.First().Titulo.Should().Be("Existing");
        }
    }
}