using Moq;
using PortalTCMSP.Domain.DTOs.Requests.Servicos.CartaServicosUsuario;
using PortalTCMSP.Domain.DTOs.Requests.Servicos.CartaServicosUsuario.Application.DTO.Servicos.CartaServicosUsuarioDto;
using PortalTCMSP.Domain.Entities.ServicosEntity.CartaServicosUsuarioEntity;
using PortalTCMSP.Domain.Repositories.Servicos.CartaServicosUsuario;
using PortalTCMSP.Domain.Repositories.Servicos.CartaServicosUsuario.Domain.Interfaces.Servicos.CartaServicosUsuarioInterface;
using PortalTCMSP.Infra.Services.Servicos.CartaServicosUsuario;

namespace PortalTCMSP.Unit.Tests.Services.Servicos.CartaServicosUsuario
{
    public class CartaServicosUsuarioServiceTests
    {
        private readonly Mock<ICartaServicosUsuarioRepository> _repoMock = new();
        private readonly Mock<ICartaServicosUsuarioServicoRepository> _repoServicosMock = new();
        private readonly Mock<ICartaServicosUsuarioServicoItemRepository> _repoServicosItensMock = new();
        private readonly Mock<ICartaServicosUsuarioServicoItemDetalheRepository> _repoItemDetalheMock = new();
        private readonly Mock<ICartaServicosUsuarioDescritivoItemDetalheRepository> _repoDescritivoMock = new();

        private CartaServicosUsuarioService CreateService()
            => new CartaServicosUsuarioService(
                _repoMock.Object,
                _repoServicosMock.Object,
                _repoServicosItensMock.Object,
                _repoItemDetalheMock.Object,
                _repoDescritivoMock.Object);

        [Fact]
        public async Task GetAllAsync_ReturnsMappedResponses_AndCallsRepo()
        {
            var entities = new List<Domain.Entities.ServicosEntity.CartaServicosUsuarioEntity.CartaServicosUsuario>
            {
                new Domain.Entities.ServicosEntity.CartaServicosUsuarioEntity.CartaServicosUsuario { Id = 1, Servicos = new List<CartaServicosUsuarioServico>() }
            };
            _repoMock.Setup(r => r.AllWithChildrenAsync()).ReturnsAsync(entities);

            var svc = CreateService();
            var result = await svc.GetAllAsync();

            Assert.NotNull(result);
            Assert.Single(result);
            _repoMock.Verify(r => r.AllWithChildrenAsync(), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsResponse_WhenFound()
        {
            var entity = new Domain.Entities.ServicosEntity.CartaServicosUsuarioEntity.CartaServicosUsuario { Id = 5, Servicos = new List<CartaServicosUsuarioServico>() };
            _repoMock.Setup(r => r.GetWithChildrenByIdAsync(5)).ReturnsAsync(entity);

            var svc = CreateService();
            var response = await svc.GetByIdAsync(5);

            Assert.NotNull(response);
            Assert.Equal(5, response!.Id);
            _repoMock.Verify(r => r.GetWithChildrenByIdAsync(5), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNull_WhenNotFound()
        {
            _repoMock.Setup(r => r.GetWithChildrenByIdAsync(10)).ReturnsAsync((Domain.Entities.ServicosEntity.CartaServicosUsuarioEntity.CartaServicosUsuario?)null);

            var svc = CreateService();
            var response = await svc.GetByIdAsync(10);

            Assert.Null(response);
        }

        [Fact]
        public async Task CreateAsync_WhenSlugExists_DisablesOld_InsertsAndCommits()
        {
            var existing = new Domain.Entities.ServicosEntity.CartaServicosUsuarioEntity.CartaServicosUsuario { Id = 2 };
            _repoMock.Setup(r => r.GetBySlugAtivoAsync("s1")).ReturnsAsync(existing);
            _repoMock.Setup(r => r.DisableAsync(existing.Id)).ReturnsAsync(true);
            _repoMock.Setup(r => r.InsertAsync(It.IsAny<Domain.Entities.ServicosEntity.CartaServicosUsuarioEntity.CartaServicosUsuario>())).Returns(Task.CompletedTask);
            _repoMock.Setup(r => r.CommitAsync()).ReturnsAsync(true);

            var svc = CreateService();
            var request = new CartaServicosUsuarioRequest { Slug = "s1" };

            var response = await svc.CreateAsync(request);

            _repoMock.Verify(r => r.GetBySlugAtivoAsync("s1"), Times.Once);
            _repoMock.Verify(r => r.DisableAsync(existing.Id), Times.Once);
            _repoMock.Verify(r => r.InsertAsync(It.IsAny<Domain.Entities.ServicosEntity.CartaServicosUsuarioEntity.CartaServicosUsuario>()), Times.Once);
            _repoMock.Verify(r => r.CommitAsync(), Times.Once);
            Assert.NotNull(response);
        }

        [Fact]
        public async Task CreateAsync_WhenSlugNotExists_InsertsAndCommits()
        {
            _repoMock.Setup(r => r.GetBySlugAtivoAsync("new-slug")).ReturnsAsync((Domain.Entities.ServicosEntity.CartaServicosUsuarioEntity.CartaServicosUsuario?)null);
            _repoMock.Setup(r => r.InsertAsync(It.IsAny<Domain.Entities.ServicosEntity.CartaServicosUsuarioEntity.CartaServicosUsuario>())).Returns(Task.CompletedTask);
            _repoMock.Setup(r => r.CommitAsync()).ReturnsAsync(true);

            var svc = CreateService();
            var request = new CartaServicosUsuarioRequest { Slug = "new-slug" };

            var response = await svc.CreateAsync(request);

            _repoMock.Verify(r => r.GetBySlugAtivoAsync("new-slug"), Times.Once);
            _repoMock.Verify(r => r.DisableAsync(It.IsAny<long>()), Times.Never);
            _repoMock.Verify(r => r.InsertAsync(It.IsAny<Domain.Entities.ServicosEntity.CartaServicosUsuarioEntity.CartaServicosUsuario>()), Times.Once);
            _repoMock.Verify(r => r.CommitAsync(), Times.Once);

            Assert.NotNull(response);
        }

        [Fact]
        public async Task CreateServicosAsync_CallsRepoAndCommits()
        {
            _repoServicosMock.Setup(r => r.CreateServicosAsync(1, It.IsAny<IEnumerable<CartaServicosUsuarioServico>>()))
                .Returns(Task.CompletedTask);
            _repoMock.Setup(r => r.CommitAsync()).ReturnsAsync(true);

            var svc = CreateService();

            var novos = new List<CartaServicosUsuarioServicoRequest> { new() };

            var ok = await svc.CreateServicosAsync(1, novos);

            _repoServicosMock.Verify(r => r.CreateServicosAsync(1, It.IsAny<IEnumerable<CartaServicosUsuarioServico>>()), Times.Once);
            _repoMock.Verify(r => r.CommitAsync(), Times.Once);
            //Assert.True(ok);
        }

        [Fact]
        public async Task CreateServicosItensAsync_CallsRepoAndCommits()
        {
            _repoServicosItensMock.Setup(r => r.CreateServicosItensAsync(10, It.IsAny<IEnumerable<CartaServicosUsuarioServicoItem>>()))
                .Returns(Task.CompletedTask);
            _repoMock.Setup(r => r.CommitAsync()).ReturnsAsync(true);

            var svc = CreateService();

            var novos = new List<CartaServicosUsuarioServicoItemRequest> { new() };

            var ok = await svc.CreateServicosItensAsync(10, novos);

            _repoServicosItensMock.Verify(r => r.CreateServicosItensAsync(10, It.IsAny<IEnumerable<CartaServicosUsuarioServicoItem>>()), Times.Once);
            _repoMock.Verify(r => r.CommitAsync(), Times.Once);
            //Assert.True(ok);
        }

        [Fact]
        public async Task CreateServicosItensDetalhesAsync_CallsRepoAndCommits()
        {
            _repoItemDetalheMock.Setup(r => r.CreateServicosItensDetalhesAsync(20, It.IsAny<IEnumerable<CartaServicosUsuarioItemDetalhe>>()))
                .Returns(Task.CompletedTask);
            _repoMock.Setup(r => r.CommitAsync()).ReturnsAsync(true);

            var svc = CreateService();

            var novos = new List<CartaServicosUsuarioItemDetalheRequest> { new() };

            var ok = await svc.CreateServicosItensDetalhesAsync(20, novos);

            _repoItemDetalheMock.Verify(r => r.CreateServicosItensDetalhesAsync(20, It.IsAny<IEnumerable<CartaServicosUsuarioItemDetalhe>>()), Times.Once);
            _repoMock.Verify(r => r.CommitAsync(), Times.Once);
            //Assert.True(ok);
        }

        [Fact]
        public async Task CreateDescritivoItemDetalheAsync_CallsRepoAndCommits()
        {
            _repoDescritivoMock.Setup(r => r.CreateDescritivoItemDetalheAsync(30, It.IsAny<IEnumerable<CartaServicosUsuarioDescritivoItemDetalhe>>()))
                .Returns(Task.CompletedTask);
            _repoMock.Setup(r => r.CommitAsync()).ReturnsAsync(true);

            var svc = CreateService();

            var novos = new List<CartaServicosUsuarioDescritivoItemDetalheRequest> { new() };

            var ok = await svc.CreateDescritivoItemDetalheAsync(30, novos);

            _repoDescritivoMock.Verify(r => r.CreateDescritivoItemDetalheAsync(30, It.IsAny<IEnumerable<CartaServicosUsuarioDescritivoItemDetalhe>>()), Times.Once);
            _repoMock.Verify(r => r.CommitAsync(), Times.Once);
            //Assert.True(ok);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsFalse_WhenNotFound()
        {
            _repoMock.Setup(r => r.GetWithChildrenByIdAsync(50)).ReturnsAsync((Domain.Entities.ServicosEntity.CartaServicosUsuarioEntity.CartaServicosUsuario?)null);

            var svc = CreateService();
            var ok = await svc.UpdateAsync(50, new CartaServicosUsuarioRequest());

            Assert.False(ok);
            _repoMock.Verify(r => r.UpdateAsync(It.IsAny<Domain.Entities.ServicosEntity.CartaServicosUsuarioEntity.CartaServicosUsuario>()), Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesAndCommits_WhenFound()
        {
            var entity = new Domain.Entities.ServicosEntity.CartaServicosUsuarioEntity.CartaServicosUsuario { Id = 6, Servicos = new List<CartaServicosUsuarioServico>() };
            _repoMock.Setup(r => r.GetWithChildrenByIdAsync(6)).ReturnsAsync(entity);
            _repoMock.Setup(r => r.UpdateAsync(entity)).Returns(Task.CompletedTask);
            _repoMock.Setup(r => r.CommitAsync()).ReturnsAsync(true);

            var svc = CreateService();
            var ok = await svc.UpdateAsync(6, new CartaServicosUsuarioRequest { Slug = "x" });

            Assert.True(ok);
            _repoMock.Verify(r => r.UpdateAsync(entity), Times.Once);
            _repoMock.Verify(r => r.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateServicosAsync_Throws_WhenParentNotFound()
        {
            _repoMock.Setup(r => r.GetWithChildrenByIdAsync(100)).ReturnsAsync((Domain.Entities.ServicosEntity.CartaServicosUsuarioEntity.CartaServicosUsuario?)null);

            var svc = CreateService();

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                svc.UpdateServicosAsync(100, new List<CartaServicosUsuarioServicoUpdate>()));
        }

        [Fact]
        public async Task UpdateServicosAsync_UpdatesAndCommits()
        {
            var servico = new CartaServicosUsuarioServico { Id = 5, Ordem = 1, Titulo = "orig", Ativo = true };
            var parent = new Domain.Entities.ServicosEntity.CartaServicosUsuarioEntity.CartaServicosUsuario { Id = 7, Servicos = new List<CartaServicosUsuarioServico> { servico } };
            _repoMock.Setup(r => r.GetWithChildrenByIdAsync(7)).ReturnsAsync(parent);
            _repoServicosMock.Setup(r => r.UpdateServicosAsync(7, It.IsAny<IEnumerable<CartaServicosUsuarioServico>>()))
                .Returns(Task.CompletedTask);
            _repoMock.Setup(r => r.CommitAsync()).ReturnsAsync(true);

            var svc = CreateService();

            var novos = new List<CartaServicosUsuarioServicoUpdate> { new() { Id = 5, Ordem = 2, Titulo = " updated ", Ativo = false } };

            var ok = await svc.UpdateServicosAsync(7, novos);

            //Assert.True(ok);
            _repoServicosMock.Verify(r => r.UpdateServicosAsync(7, It.IsAny<IEnumerable<CartaServicosUsuarioServico>>()), Times.Once);
            _repoMock.Verify(r => r.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateServicosItensAsync_CallsRepoAndCommits()
        {
            _repoServicosItensMock.Setup(r => r.UpdateServicosItensAsync(11, It.IsAny<IEnumerable<CartaServicosUsuarioServicoItem>>()))
                .Returns(Task.CompletedTask);
            _repoMock.Setup(r => r.CommitAsync()).ReturnsAsync(true);

            var svc = CreateService();

            var novos = new List<CartaServicosUsuarioServicoItemUpdate> { new() { Id = 1, Ativo = true, Ordem = 1, Titulo = "t", LinkItem = "l", Acao = "a" } };

            var ok = await svc.UpdateServicosItensAsync(11, novos);

            //Assert.True(ok);
            _repoServicosItensMock.Verify(r => r.UpdateServicosItensAsync(11, It.IsAny<IEnumerable<CartaServicosUsuarioServicoItem>>()), Times.Once);
            _repoMock.Verify(r => r.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateServicosItensDetalhesAsync_CallsRepoAndCommits()
        {
            _repoItemDetalheMock.Setup(r => r.UpdateServicosItensDetalhesAsync(12, It.IsAny<IEnumerable<CartaServicosUsuarioItemDetalhe>>()))
                .Returns(Task.CompletedTask);
            _repoMock.Setup(r => r.CommitAsync()).ReturnsAsync(true);

            var svc = CreateService();

            var novos = new List<CartaServicosUsuarioItemDetalheUpdate> { new() { Id = 1, Ativo = true, Ordem = 1, TituloDetalhe = "td" } };

            var ok = await svc.UpdateServicosItensDetalhesAsync(12, novos);

            //Assert.True(ok);
            _repoItemDetalheMock.Verify(r => r.UpdateServicosItensDetalhesAsync(12, It.IsAny<IEnumerable<CartaServicosUsuarioItemDetalhe>>()), Times.Once);
            _repoMock.Verify(r => r.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateDescritivoItemDetalheAsync_CallsRepoAndCommits()
        {
            _repoDescritivoMock.Setup(r => r.UpdateDescritivoItemDetalheAsync(13, It.IsAny<IEnumerable<CartaServicosUsuarioDescritivoItemDetalhe>>()))
                .Returns(Task.CompletedTask);
            _repoMock.Setup(r => r.CommitAsync()).ReturnsAsync(true);

            var svc = CreateService();

            var novos = new List<CartaServicosUsuarioDescritivoItemDetalheUpdate> { new() { Id = 1, Ordem = 1, Descritivo = "d" } };

            var ok = await svc.UpdateDescritivoItemDetalheAsync(13, novos);

            //Assert.True(ok);
            _repoDescritivoMock.Verify(r => r.UpdateDescritivoItemDetalheAsync(13, It.IsAny<IEnumerable<CartaServicosUsuarioDescritivoItemDetalhe>>()), Times.Once);
            _repoMock.Verify(r => r.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsFalse_WhenNotFound()
        {
            _repoMock.Setup(r => r.FindByIdAsync(99)).ReturnsAsync((Domain.Entities.ServicosEntity.CartaServicosUsuarioEntity.CartaServicosUsuario?)null);

            var svc = CreateService();
            var ok = await svc.DeleteAsync(99);

            Assert.False(ok);
            _repoMock.Verify(r => r.DeleteAsync(It.IsAny<Domain.Entities.ServicosEntity.CartaServicosUsuarioEntity.CartaServicosUsuario>()), Times.Never);
        }

        [Fact]
        public async Task DeleteAsync_DeletesAndCommits_WhenFound()
        {
            var entity = new Domain.Entities.ServicosEntity.CartaServicosUsuarioEntity.CartaServicosUsuario { Id = 3 };
            _repoMock.Setup(r => r.FindByIdAsync(3)).ReturnsAsync(entity);
            _repoMock.Setup(r => r.DeleteAsync(entity)).Returns(Task.CompletedTask);
            _repoMock.Setup(r => r.CommitAsync()).ReturnsAsync(true);

            var svc = CreateService();
            var ok = await svc.DeleteAsync(3);

            Assert.True(ok);
            _repoMock.Verify(r => r.DeleteAsync(entity), Times.Once);
            _repoMock.Verify(r => r.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task GetWithChildrenBySlugAtivoAsync_DelegatesToRepo()
        {
            var entity = new Domain.Entities.ServicosEntity.CartaServicosUsuarioEntity.CartaServicosUsuario { Id = 8 };
            _repoMock.Setup(r => r.GetWithChildrenBySlugAtivoAsync("slug")).ReturnsAsync(entity);

            var svc = CreateService();
            var response = await svc.GetWithChildrenBySlugAtivoAsync("slug");

            Assert.NotNull(response);
            _repoMock.Verify(r => r.GetWithChildrenBySlugAtivoAsync("slug"), Times.Once);
        }

        [Fact]
        public async Task DisableAsync_DelegatesToRepository()
        {
            _repoMock.Setup(r => r.DisableAsync(4)).ReturnsAsync(true);

            var svc = CreateService();
            var ok = await svc.DisableAsync(4);

            Assert.True(ok);
            _repoMock.Verify(r => r.DisableAsync(4), Times.Once);
        }
    }
}