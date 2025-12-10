using Microsoft.EntityFrameworkCore;
using PortalTCMSP.Domain.Entities.ServicosEntity.CartorioEntity;
using PortalTCMSP.Infra.Data.Context;
using PortalTCMSP.Infra.Data.Repositories.Servicos.Cartorio;

namespace PortalTCMSP.Infra.Data.Tests.Repositories.Servicos.Cartorio
{
    public class CartorioAtendimentoRepositoryTests
    {
        private static PortalTCMSPContext CreateContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<PortalTCMSPContext>()
                .UseInMemoryDatabase(dbName)
                .Options;
            return new PortalTCMSPContext(options);
        }

        [Fact]
        public async Task CreateAtendimentosAsync_AddsNewAtendimentos_AndMarksEntityModified()
        {
            var dbName = Guid.NewGuid().ToString();
            await using var context = CreateContext(dbName);

            var cartorio = new Domain.Entities.ServicosEntity.CartorioEntity.Cartorio
            {
                Id = 1,
                Atendimentos = new List<CartorioAtendimento>
                {
                    new CartorioAtendimento { Id = 10, IdCartorio = 1 }
                }
            };

            context.Cartorio.Add(cartorio);
            await context.SaveChangesAsync();

            var repo = new CartorioAtendimentoRepository(context);

            var novos = new[]
            {
                new CartorioAtendimento { Id = 11, IdCartorio = 1 },
                new CartorioAtendimento { Id = 12, IdCartorio = 1 }
            };

            await repo.CreateAtendimentosAsync(1, novos);

            var entity = await context.Cartorio.Include(c => c.Atendimentos).FirstAsync(c => c.Id == 1);

            Assert.Equal(3, entity.Atendimentos.Count);
            Assert.Equal(EntityState.Modified, context.Entry(entity).State);
        }

        [Fact]
        public async Task CreateAtendimentosAsync_WhenCartorioNotFound_ThrowsInvalidOperationException()
        {
            var dbName = Guid.NewGuid().ToString();
            await using var context = CreateContext(dbName);

            var repo = new CartorioAtendimentoRepository(context);

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                repo.CreateAtendimentosAsync(999, Array.Empty<CartorioAtendimento>()));

            Assert.Contains("Cartorio with Id 999 not found.", ex.Message);
        }

        [Fact]
        public async Task UpdateAtendimentosAsync_MarksEntityModified()
        {
            var dbName = Guid.NewGuid().ToString();
            await using var context = CreateContext(dbName);

            var cartorio = new Domain.Entities.ServicosEntity.CartorioEntity.Cartorio
            {
                Id = 2,
                Atendimentos = new List<CartorioAtendimento>()
            };

            context.Cartorio.Add(cartorio);
            await context.SaveChangesAsync();

            var repo = new CartorioAtendimentoRepository(context);

            await repo.UpdateAtendimentosAsync(2, new[] { new CartorioAtendimento { Id = 21, IdCartorio = 2 } });

            var entity = await context.Cartorio.Include(c => c.Atendimentos).FirstAsync(c => c.Id == 2);

            Assert.Equal(EntityState.Modified, context.Entry(entity).State);
        }

        [Fact]
        public async Task UpdateAtendimentosAsync_WhenCartorioNotFound_ThrowsInvalidOperationException()
        {
            var dbName = Guid.NewGuid().ToString();
            await using var context = CreateContext(dbName);

            var repo = new CartorioAtendimentoRepository(context);

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() =>
                repo.UpdateAtendimentosAsync(500, new[] { new CartorioAtendimento() }));

            Assert.Contains("Cartorio with Id 500 not found.", ex.Message);
        }
    }
}