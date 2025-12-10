using Moq;
using PortalTCMSP.Domain.Entities.BannerEntity;
using PortalTCMSP.Domain.Repositories.Home;
using PortalTCMSP.Infra.Services.Home;
using PortalTCMSP.Unit.Tests.Services.FixFeature;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Unit.Tests.Services
{
    [ExcludeFromCodeCoverage]
    public class BannerServiceTest
    {
        private readonly BannerService _service;
        private readonly Mock<IBannerRepository> _repoMock;
        private readonly BannerServiceFixture _fixture;

        public BannerServiceTest()
        {
            _repoMock = new Mock<IBannerRepository>(MockBehavior.Strict);
            _fixture = new BannerServiceFixture();
            _service = new BannerService(_repoMock.Object);
        }

        [Fact]
        public async Task ObterPorIdAsync_DeveRetornarBannerResponse_QuandoExiste()
        {
            var banner = _fixture.GetBannerEntity(10, true);
            _repoMock.Setup(r => r.FindByIdAsync(10)).ReturnsAsync(banner);

            var result = await _service.ObterPorIdAsync(10);

            Assert.NotNull(result);
            Assert.Equal(banner.Id, result!.Id);
            Assert.Equal(banner.Nome, result.Nome);
            Assert.Equal(banner.Imagem, result.ImagemUrl);

            _repoMock.Verify(r => r.FindByIdAsync(10), Times.Once);
            _repoMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ObterPorIdAsync_DeveRetornarNull_QuandoNaoExiste()
        {
            _repoMock.Setup(r => r.FindByIdAsync(999)).ReturnsAsync((Banner?)null);

            var result = await _service.ObterPorIdAsync(999);

            Assert.Null(result);

            _repoMock.Verify(r => r.FindByIdAsync(999), Times.Once);
            _repoMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ObterTodosAsync_DeveRetornarTodosMapeados()
        {
            var list = new List<Banner>
            {
                _fixture.GetBannerEntity(1, true),
                _fixture.GetBannerEntity(2, false)
            };
            _repoMock.Setup(r => r.AllAsync()).ReturnsAsync(list);

            var result = await _service.ObterTodosAsync();

            Assert.Equal(2, result.Count());

            _repoMock.Verify(r => r.AllAsync(), Times.Once);
            _repoMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ObterAtivosAsync_DeveRetornarApenasAtivos()
        {
            var ativos = new List<Banner>
            {
                _fixture.GetBannerEntity(1, true),
                _fixture.GetBannerEntity(2, true)
            };
            _repoMock.Setup(r => r.GetAtivosAsync()).ReturnsAsync(ativos);

            var result = await _service.ObterAtivosAsync();

            Assert.Equal(2, result.Count());
            Assert.All(result, r => Assert.True(r.Ativo));

            _repoMock.Verify(r => r.GetAtivosAsync(), Times.Once);
            _repoMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task CriarAsync_ComAtivoTrue_DeveDesativarTodosOsOutros()
        {
            var request = _fixture.GetBannerCreateRequest(true);
            _repoMock.Setup(r => r.DeactivateAllAsync()).Returns(Task.CompletedTask);
            _repoMock.Setup(r => r.InsertAsync(It.IsAny<Banner>()))
                .Callback<Banner>(b => b.Id = 123)
                .Returns(Task.CompletedTask);
            _repoMock.Setup(r => r.CommitAsync()).ReturnsAsync(true);

            var idGerado = await _service.CriarAsync(request);

            Assert.Equal(123, idGerado);
            _repoMock.Verify(r => r.DeactivateAllAsync(), Times.Once);
            _repoMock.Verify(r => r.InsertAsync(It.Is<Banner>(b => b.Nome == request.Nome && b.Imagem == request.Imagem && b.Ativo)), Times.Once);
            _repoMock.Verify(r => r.CommitAsync(), Times.Once);
            _repoMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task CriarAsync_ComAtivoFalse_NaoDeveDesativarOutros()
        {
            var request = _fixture.GetBannerCreateRequest(false);
            _repoMock.Setup(r => r.InsertAsync(It.IsAny<Banner>()))
                .Callback<Banner>(b => b.Id = 456)
                .Returns(Task.CompletedTask);
            _repoMock.Setup(r => r.CommitAsync()).ReturnsAsync(true);

            var idGerado = await _service.CriarAsync(request);

            Assert.Equal(456, idGerado);
            _repoMock.Verify(r => r.DeactivateAllAsync(), Times.Never);
            _repoMock.Verify(r => r.InsertAsync(It.Is<Banner>(b => !b.Ativo)), Times.Once);
            _repoMock.Verify(r => r.CommitAsync(), Times.Once);
            _repoMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task AtualizarAsync_AtivoTrue_DeveDesativarOutros()
        {
            var entity = _fixture.GetBannerEntity(5, false);
            var request = _fixture.GetBannerUpdateRequest(5, true);

            _repoMock.Setup(r => r.FindByIdAsync(5)).ReturnsAsync(entity);
            _repoMock.Setup(r => r.UpdateAsync(It.IsAny<Banner>())).Returns(Task.CompletedTask);
            _repoMock.Setup(r => r.DeactivateAllExceptAsync(5)).Returns(Task.CompletedTask);
            _repoMock.Setup(r => r.CommitAsync()).ReturnsAsync(true);

            var ok = await _service.AtualizarAsync(5, request);

            Assert.True(ok);
            _repoMock.Verify(r => r.FindByIdAsync(5), Times.Once);
            _repoMock.Verify(r => r.UpdateAsync(It.Is<Banner>(b => b.Nome == request.Nome && b.Imagem == request.Imagem && b.Ativo)), Times.Once);
            _repoMock.Verify(r => r.DeactivateAllExceptAsync(5), Times.Once);
            _repoMock.Verify(r => r.CommitAsync(), Times.Once);
            _repoMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task AtualizarAsync_AtivoFalse_NaoDeveDesativarOutros()
        {
            var entity = _fixture.GetBannerEntity(8, true);
            var request = _fixture.GetBannerUpdateRequest(8, false);

            _repoMock.Setup(r => r.FindByIdAsync(8)).ReturnsAsync(entity);
            _repoMock.Setup(r => r.UpdateAsync(It.IsAny<Banner>())).Returns(Task.CompletedTask);
            _repoMock.Setup(r => r.CommitAsync()).ReturnsAsync(true);

            var ok = await _service.AtualizarAsync(8, request);

            Assert.True(ok);
            _repoMock.Verify(r => r.FindByIdAsync(8), Times.Once);
            _repoMock.Verify(r => r.UpdateAsync(It.Is<Banner>(b => b.Nome == request.Nome && b.Imagem == request.Imagem && !b.Ativo)), Times.Once);
            _repoMock.Verify(r => r.DeactivateAllExceptAsync(It.IsAny<long>()), Times.Never);
            _repoMock.Verify(r => r.CommitAsync(), Times.Once);
            _repoMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task AtualizarAsync_DeveRetornarFalse_QuandoNaoExiste()
        {
            var request = _fixture.GetBannerUpdateRequest(999, true);
            _repoMock.Setup(r => r.FindByIdAsync(999)).ReturnsAsync((Banner?)null);

            var ok = await _service.AtualizarAsync(999, request);

            Assert.False(ok);
            _repoMock.Verify(r => r.FindByIdAsync(999), Times.Once);
            _repoMock.Verify(r => r.UpdateAsync(It.IsAny<Banner>()), Times.Never);
            _repoMock.Verify(r => r.CommitAsync(), Times.Never);
            _repoMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task RemoverAsync_DeveRetornarTrue_QuandoCommitOk()
        {
            _repoMock.Setup(r => r.DeleteAsync(3)).ReturnsAsync(true);
            _repoMock.Setup(r => r.CommitAsync()).ReturnsAsync(true);

            var ok = await _service.RemoverAsync(3);

            Assert.True(ok);
            _repoMock.Verify(r => r.DeleteAsync(3), Times.Once);
            _repoMock.Verify(r => r.CommitAsync(), Times.Once);
            _repoMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task RemoverAsync_DeveRetornarFalse_QuandoCommitFalha()
        {
            _repoMock.Setup(r => r.DeleteAsync(7)).ReturnsAsync(true);
            _repoMock.Setup(r => r.CommitAsync()).ReturnsAsync(false);

            var ok = await _service.RemoverAsync(7);

            Assert.False(ok);
            _repoMock.Verify(r => r.DeleteAsync(7), Times.Once);
            _repoMock.Verify(r => r.CommitAsync(), Times.Once);
            _repoMock.VerifyNoOtherCalls();
        }
    }
}
