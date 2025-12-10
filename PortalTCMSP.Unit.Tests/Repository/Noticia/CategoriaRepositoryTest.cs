using Microsoft.EntityFrameworkCore;
using PortalTCMSP.Infra.Data.Context;
using PortalTCMSP.Infra.Data.Repositories.Home;
using PortalTCMSP.Unit.Tests.Repository.Noticia.FixFeature;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Unit.Tests.Repository
{
    [ExcludeFromCodeCoverage]
    public class CategoriaRepositoryTest
    {
        private PortalTCMSPContext GetInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<PortalTCMSPContext>()
                .UseInMemoryDatabase("PortalTCMSPTestDB_" + System.Guid.NewGuid())
                .Options;
            return new PortalTCMSPContext(options);
        }

        [Fact]
        public async Task ListarTodasAsync_DeveRetornarTodasCategorias_OrdenadasPorNome()
        {
            // Arrange
            var fixture = new CategoriaRepositoryFixture();
            var categorias = fixture.GetCategorias();

            using var context = GetInMemoryContext();
            context.Categoria.AddRange(categorias);
            await context.SaveChangesAsync();

            var repo = new CategoriaRepository(context);

            // Act
            var resultado = await repo.ListarTodasAsync();

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(3, resultado.Count());
            Assert.Equal(new[] { "Artes", "Esportes", "Tecnologia" }, resultado.Select(c => c.Nome).ToArray());
        }

        [Fact]
        public async Task ListarTodasAsync_DeveRetornarListaVazia_QuandoNaoHaCategorias()
        {
            using var context = GetInMemoryContext();
            var repo = new CategoriaRepository(context);

            var resultado = await repo.ListarTodasAsync();

            Assert.NotNull(resultado);
            Assert.Empty(resultado);
        }
    }
}
