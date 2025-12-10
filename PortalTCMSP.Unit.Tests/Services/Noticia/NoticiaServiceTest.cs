//using Moq;
//using PortalTCMSP.Domain.DTOs.Requests.NoticiaRequest;
//using PortalTCMSP.Domain.Entities.NoticiaEntity;
//using PortalTCMSP.Domain.Repositories.Home;
//using PortalTCMSP.Domain.Repositories.Noticia;
//using PortalTCMSP.Infra.Services.Noticia;
//using PortalTCMSP.Unit.Tests.Services.FixFeature;
//using System.Diagnostics.CodeAnalysis;

//namespace PortalTCMSP.Unit.Tests.Services
//{
//    [ExcludeFromCodeCoverage]
//    public class NoticiaServiceTest
//    {
//        private readonly NoticiaService _service;
//        private readonly Mock<INoticiaRepository> _noticiaRepoMock;
//        private readonly Mock<ICategoriaRepository> _categoriaRepoMock;
//        private readonly NoticiaServiceFixture _fixture;

//        public NoticiaServiceTest()
//        {
//            _noticiaRepoMock = new Mock<INoticiaRepository>(MockBehavior.Strict);
//            _categoriaRepoMock = new Mock<ICategoriaRepository>(MockBehavior.Strict);
//            _fixture = new NoticiaServiceFixture();
//            _service = new NoticiaService(_noticiaRepoMock.Object, _categoriaRepoMock.Object);
//        }

//        [Fact]
//        public async Task ObterPorSlug_DeveRetornarNull()
//        {
//            _noticiaRepoMock.Setup(r => r.ObterPorSlugAsync("x")).ReturnsAsync((PortalTCMSP.Domain.Entities.NoticiaEntity.Noticia?)null);

//            var resp = await _service.ObterNoticiaPorSlugAsync("x");

//            Assert.Null(resp);
//            _noticiaRepoMock.Verify(r => r.ObterPorSlugAsync("x"), Times.Once);
//            _noticiaRepoMock.VerifyNoOtherCalls();
//            _categoriaRepoMock.VerifyNoOtherCalls();
//        }

//        [Fact]
//        public async Task Adicionar_CategoriaInexistente()
//        {
//            var request = _fixture.GetCreateRequest();
//            _categoriaRepoMock.Setup(r => r.FindByIdAsync((int)request.CategoriaId)).ReturnsAsync((Categoria?)null);

//            await Assert.ThrowsAsync<ArgumentException>(() => _service.AdicionarAsync(request));

//            _categoriaRepoMock.Verify(r => r.FindByIdAsync((int)request.CategoriaId), Times.Once);
//            _noticiaRepoMock.VerifyNoOtherCalls();
//            _categoriaRepoMock.VerifyNoOtherCalls();
//        }

//        [Fact]
//        public async Task Atualizar_Inexistente()
//        {
//            var request = _fixture.GetUpdateRequest();
//            _noticiaRepoMock.Setup(r => r.ObterPorIdComBlocosAsync((int)request.Id)).ReturnsAsync((PortalTCMSP.Domain.Entities.NoticiaEntity.Noticia?)null);

//            var ok = await _service.AtualizarAsync(request);

//            Assert.False(ok);
//            _noticiaRepoMock.Verify(r => r.ObterPorIdComBlocosAsync((int)request.Id), Times.Once);
//            _noticiaRepoMock.VerifyNoOtherCalls();
//            _categoriaRepoMock.VerifyNoOtherCalls();
//        }

//        [Fact]
//        public async Task Atualizar_CategoriaInexistente()
//        {
//            var request = _fixture.GetUpdateRequest();
//            var noticia = _fixture.GetNoticiaEntityWithBlocos(request.Id, request.CategoriaId);

//            _noticiaRepoMock.Setup(r => r.ObterPorIdComBlocosAsync((int)request.Id)).ReturnsAsync(noticia);
//            _categoriaRepoMock.Setup(r => r.FindByIdAsync((int)request.CategoriaId)).ReturnsAsync((Categoria?)null);

//            await Assert.ThrowsAsync<ArgumentException>(() => _service.AtualizarAsync(request));

//            _noticiaRepoMock.Verify(r => r.ObterPorIdComBlocosAsync((int)request.Id), Times.Once);
//            _categoriaRepoMock.Verify(r => r.FindByIdAsync((int)request.CategoriaId), Times.Once);
//            _noticiaRepoMock.VerifyNoOtherCalls();
//            _categoriaRepoMock.VerifyNoOtherCalls();
//        }

//        [Fact]
//        public async Task Deletar_Existe()
//        {
//            var noticia = _fixture.GetNoticiaEntity(5);
//            _noticiaRepoMock.Setup(r => r.FindByIdAsync(noticia.Id)).ReturnsAsync(noticia);
//            _noticiaRepoMock.Setup(r => r.DeleteAsync(noticia)).Returns(Task.CompletedTask);
//            _noticiaRepoMock.Setup(r => r.CommitAsync()).ReturnsAsync(true);

//            var ok = await _service.DeletarAsync((int)noticia.Id);

//            Assert.True(ok);
//            _noticiaRepoMock.Verify(r => r.FindByIdAsync(noticia.Id), Times.Once);
//            _noticiaRepoMock.Verify(r => r.DeleteAsync(noticia), Times.Once);
//            _noticiaRepoMock.Verify(r => r.CommitAsync(), Times.Once);
//            _noticiaRepoMock.VerifyNoOtherCalls();
//            _categoriaRepoMock.VerifyNoOtherCalls();
//        }

//        [Fact]
//        public async Task Deletar_Inexistente()
//        {
//            _noticiaRepoMock.Setup(r => r.FindByIdAsync(99)).ReturnsAsync((PortalTCMSP.Domain.Entities.NoticiaEntity.Noticia?)null);

//            var ok = await _service.DeletarAsync(99);

//            Assert.False(ok);
//            _noticiaRepoMock.Verify(r => r.FindByIdAsync(99), Times.Once);
//            _noticiaRepoMock.VerifyNoOtherCalls();
//            _categoriaRepoMock.VerifyNoOtherCalls();
//        }
//    }
//}
