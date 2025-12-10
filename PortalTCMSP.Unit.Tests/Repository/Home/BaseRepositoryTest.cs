using Microsoft.EntityFrameworkCore;
using PortalTCMSP.Infra.Data.Context;
using PortalTCMSP.Infra.Data.Repositories.Home;
using PortalTCMSP.Unit.Tests.Repository.Home.FixFeature;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Unit.Tests.Repository.Home
{
    [ExcludeFromCodeCoverage]
    public class BaseRepositoryTest
    {
        private PortalTCMSPContext GetInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<PortalTCMSPContext>()
                .UseInMemoryDatabase("BaseRepositoryTestDB_" + Guid.NewGuid())
                .Options;
            return new PortalTCMSPContext(options);
        }

        [Fact]
        public async Task InsertAsync_FindByIdAsync_CommitAsync_DeveInserirEBuscarEntidade()
        {
            using var context = GetInMemoryContext();
            var repo = new CategoriaRepository(context);

            var fixture = new BaseRepositoryFixture();
            var cat = fixture.GetCategoria();

            await repo.InsertAsync(cat);
            var saved = await repo.CommitAsync();

            Assert.True(saved);

            var found = await repo.FindByIdAsync(cat.Id);
            Assert.NotNull(found);
            Assert.Equal(cat.Nome, found.Nome);
        }

        [Fact]
        public async Task InsertManyAsync_AllAsync_CountAllAsync_DeveInserirBuscarEContar()
        {
            using var context = GetInMemoryContext();
            var repo = new CategoriaRepository(context);

            var fixture = new BaseRepositoryFixture();
            var cats = fixture.GetCategorias();

            await repo.InsertManyAsync(cats);
            await repo.CommitAsync();

            var all = await repo.AllAsync();
            Assert.Equal(3, all.Count());

            var count = await repo.CountAllAsync();
            Assert.Equal(3, count);
        }

        [Fact]
        public async Task WhereAsync_AnyAsync_FindAsync_DeveFiltrarEntidades()
        {
            using var context = GetInMemoryContext();
            var repo = new CategoriaRepository(context);

            var fixture = new BaseRepositoryFixture();
            var cats = fixture.GetCategorias();

            await repo.InsertManyAsync(cats);
            await repo.CommitAsync();

            var resultado = await repo.WhereAsync(c => c.Nome.Contains("tec", StringComparison.OrdinalIgnoreCase));
            Assert.Single(resultado);

            var existe = await repo.AnyAsync(c => c.Nome == "Esportes");
            Assert.True(existe);

            var find = await repo.FindAsync(c => c.Slug == "cultura");
            Assert.NotNull(find);
            Assert.Equal("Cultura", find.Nome);
        }

        [Fact]
        public async Task UpdateAsync_UpdateManyAsync_DeveAtualizarEntidade()
        {
            using var context = GetInMemoryContext();
            var repo = new CategoriaRepository(context);

            var fixture = new BaseRepositoryFixture();
            var cats = fixture.GetCategorias();
            await repo.InsertManyAsync(cats);
            await repo.CommitAsync();

            // Atualiza um
            var um = await repo.FindAsync(c => c.Slug == "esportes");
            Assert.NotNull(um);             
            um!.Nome = "Atualizado";        

            await repo.UpdateAsync(um);
            await repo.CommitAsync();

            var atualizado = await repo.FindByIdAsync(um.Id);
            Assert.NotNull(atualizado);
            Assert.Equal("Atualizado", atualizado!.Nome);

            // Atualiza muitos
            var todos = (await repo.AllAsync()).ToArray();
            foreach (var c in todos) c.Slug += "-edit";
            await repo.UpdateManyAsync(todos);
            await repo.CommitAsync();

            var todosEditados = await repo.AllAsync();
            Assert.All(todosEditados, c => Assert.EndsWith("-edit", c.Slug));
        }

        [Fact]
        public async Task DeleteAsync_PorIdEPorEntidade_DeveRemover()
        {
            using var context = GetInMemoryContext();
            var repo = new CategoriaRepository(context);

            var fixture = new BaseRepositoryFixture();
            var cat = fixture.GetCategoria();

            await repo.InsertAsync(cat);
            await repo.CommitAsync();

            // Remover por entidade
            await repo.DeleteAsync(cat);
            await repo.CommitAsync();

            var removido = await repo.FindByIdAsync(cat.Id);
            Assert.Null(removido);

            await repo.InsertAsync(cat);
            await repo.CommitAsync();

            await repo.DeleteAsync(cat.Id);
            await repo.CommitAsync();

            var removidoPorId = await repo.FindByIdAsync(cat.Id);
            Assert.Null(removidoPorId);
        }
    }
}
