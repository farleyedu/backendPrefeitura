using Moq;
using PortalTCMSP.Domain.Entities.NoticiaEntity;
using PortalTCMSP.Domain.Repositories.Home;
using PortalTCMSP.Infra.Services.Home;
using PortalTCMSP.Unit.Tests.Services.Noticia.FixFeature;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Unit.Tests.Services.Noticia
{
    [ExcludeFromCodeCoverage]
    public class CategoriaServiceTest
    {
        private readonly CategoriaService _service;
        private readonly Mock<ICategoriaRepository> _repoMock;
        private readonly CategoriaServiceFixture _fixture;

        public CategoriaServiceTest()
        {
            _repoMock = new Mock<ICategoriaRepository>();
            _fixture = new CategoriaServiceFixture();
            _service = new CategoriaService(_repoMock.Object);
        }

        [Fact]
        public async Task ListarAsync_DeveRetornarTodasCategorias()
        {
            var categorias = _fixture.GetCategoriaList();
            _repoMock.Setup(x => x.ListarTodasAsync()).ReturnsAsync(categorias);

            var result = await _service.ListarAsync();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Contains(result, c => c.Nome == "Tecnologia");
            Assert.Contains(result, c => c.Nome == "Esportes");
        }

        [Fact]
        public async Task ObterPorIdAsync_DeveRetornarCategoria_QuandoExiste()
        {
            var cat = _fixture.GetCategoriaEntity(1);
            _repoMock.Setup(x => x.FindByIdAsync(1)).ReturnsAsync(cat);

            var result = await _service.ObterPorIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal("Tecnologia", result.Nome);
        }

        [Fact]
        public async Task ObterPorIdAsync_DeveRetornarNull_QuandoNaoExiste()
        {
            _repoMock.Setup(x => x.FindByIdAsync(It.IsAny<int>())).ReturnsAsync((Categoria)null!);

            var result = await _service.ObterPorIdAsync(999);

            Assert.Null(result);
        }

        [Fact]
        public async Task CriarAsync_DeveInserirCategoriaERetornarId()
        {
            var request = _fixture.GetCreateRequest();
            var catEntity = new Categoria { Id = 1, Nome = request.Nome, Slug = request.Slug };

            _repoMock.Setup(x => x.InsertAsync(It.IsAny<Categoria>())).Callback<Categoria>(c => c.Id = catEntity.Id).Returns(Task.CompletedTask);
            _repoMock.Setup(x => x.CommitAsync()).ReturnsAsync(true);

            var result = await _service.CriarAsync(request);

            _repoMock.Verify(x => x.InsertAsync(It.IsAny<Categoria>()), Times.Once);
            _repoMock.Verify(x => x.CommitAsync(), Times.Once);
            Assert.Equal(catEntity.Id, result);
        }

        [Fact]
        public async Task AtualizarAsync_DeveAtualizarCategoria_QuandoExiste()
        {
            var request = _fixture.GetUpdateRequest(1);
            var cat = _fixture.GetCategoriaEntity(1);

            _repoMock.Setup(x => x.FindByIdAsync(request.Id)).ReturnsAsync(cat);
            _repoMock.Setup(x => x.UpdateAsync(It.IsAny<Categoria>())).Returns(Task.CompletedTask);
            _repoMock.Setup(x => x.CommitAsync()).ReturnsAsync(true);

            var result = await _service.AtualizarAsync(request);

            _repoMock.Verify(x => x.UpdateAsync(It.Is<Categoria>(c => c.Nome == request.Nome && c.Slug == request.Slug)), Times.Once);
            _repoMock.Verify(x => x.CommitAsync(), Times.Once);
            Assert.True(result);
        }

        [Fact]
        public async Task AtualizarAsync_DeveRetornarFalse_QuandoCategoriaNaoExiste()
        {
            var request = _fixture.GetUpdateRequest(999);
            _repoMock.Setup(x => x.FindByIdAsync(request.Id)).ReturnsAsync((Categoria)null!);

            var result = await _service.AtualizarAsync(request);

            _repoMock.Verify(x => x.UpdateAsync(It.IsAny<Categoria>()), Times.Never);
            _repoMock.Verify(x => x.CommitAsync(), Times.Never);
            Assert.False(result);
        }

        [Fact]
        public async Task DeletarAsync_DeveRemoverCategoria_QuandoExiste()
        {
            var cat = _fixture.GetCategoriaEntity(1);
            _repoMock.Setup(x => x.FindByIdAsync(1)).ReturnsAsync(cat);
            _repoMock.Setup(x => x.DeleteAsync(cat)).Returns(Task.CompletedTask);
            _repoMock.Setup(x => x.CommitAsync()).ReturnsAsync(true);

            var result = await _service.DeletarAsync(1);

            _repoMock.Verify(x => x.DeleteAsync(cat), Times.Once);
            _repoMock.Verify(x => x.CommitAsync(), Times.Once);
            Assert.True(result);
        }

        [Fact]
        public async Task DeletarAsync_DeveRetornarFalse_QuandoCategoriaNaoExiste()
        {
            _repoMock.Setup(x => x.FindByIdAsync(It.IsAny<int>())).ReturnsAsync((Categoria)null!);

            var result = await _service.DeletarAsync(999);

            _repoMock.Verify(x => x.DeleteAsync(It.IsAny<Categoria>()), Times.Never);
            _repoMock.Verify(x => x.CommitAsync(), Times.Never);
            Assert.False(result);
        }
    }
}
