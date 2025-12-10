using Moq;
using PortalTCMSP.Domain.DTOs.Requests.Servicos.CartaServicosUsuario;
using PortalTCMSP.Domain.DTOs.Requests.Servicos.CartaServicosUsuario.Application.DTO.Servicos.CartaServicosUsuarioDto;
using PortalTCMSP.Domain.Entities.ServicosEntity.CartaServicosUsuarioEntity;
using PortalTCMSP.Domain.Repositories.Servicos.CartaServicosUsuario;
using PortalTCMSP.Domain.Repositories.Servicos.CartaServicosUsuario.Domain.Interfaces.Servicos.CartaServicosUsuarioInterface;
using PortalTCMSP.Infra.Services.Servicos.CartaServicosUsuario;

namespace PortalTCMSP.Infra.Services.Tests.Servicos.CartaServicosUsuario
{
    public class CartaServicosUsuarioServiceTests
    {
        private readonly Mock<ICartaServicosUsuarioRepository> _repoMock = new();
        private readonly Mock<ICartaServicosUsuarioServicoRepository> _repoServicosMock = new();
        private readonly Mock<ICartaServicosUsuarioServicoItemRepository> _repoServicosItensMock = new();
        private readonly Mock<ICartaServicosUsuarioServicoItemDetalheRepository> _repoItemDetalheMock = new();
        private readonly Mock<ICartaServicosUsuarioDescritivoItemDetalheRepository> _repoDescritivoMock = new();

        private CartaServicosUsuarioService CreateService() =>
            new(_repoMock.Object,
                _repoServicosMock.Object,
                _repoServicosItensMock.Object,
                _repoItemDetalheMock.Object,
                _repoDescritivoMock.Object);

        [Fact]
        public async Task GetAllAsync_ReturnsListMapped()
        {
            // Arrange
            var entity = new Domain.Entities.ServicosEntity.CartaServicosUsuarioEntity.CartaServicosUsuario { Id = 1 };
            _repoMock.Setup(r => r.AllWithChildrenAsync()).ReturnsAsync(new List<Domain.Entities.ServicosEntity.CartaServicosUsuarioEntity.CartaServicosUsuario> { entity });

            var svc = CreateService();

            // Act
            var result = await svc.GetAllAsync();

            // Assert
            _repoMock.Verify(r => r.AllWithChildrenAsync(), Times.Once);
            Assert.NotNull(result);
            Assert.NotEmpty(result);
        }

        [Fact]
        public async Task GetByIdAsync_Found_ReturnsMapped()
        {
            var entity = new Domain.Entities.ServicosEntity.CartaServicosUsuarioEntity.CartaServicosUsuario { Id = 2 };
            _repoMock.Setup(r => r.GetWithChildrenByIdAsync(2)).ReturnsAsync(entity);

            var svc = CreateService();

            var result = await svc.GetByIdAsync(2);

            _repoMock.Verify(r => r.GetWithChildrenByIdAsync(2), Times.Once);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetByIdAsync_NotFound_ReturnsNull()
        {
            _repoMock.Setup(r => r.GetWithChildrenByIdAsync(99)).ReturnsAsync((Domain.Entities.ServicosEntity.CartaServicosUsuarioEntity.CartaServicosUsuario?)null);

            var svc = CreateService();

            var result = await svc.GetByIdAsync(99);

            Assert.Null(result);
        }

        [Fact]
        public async Task CreateAsync_WhenSlugExists_DisablesOldAndInsertsNew_AndCommits()
        {
            var existing = new Domain.Entities.ServicosEntity.CartaServicosUsuarioEntity.CartaServicosUsuario { Id = 5 };
            _repoMock.Setup(r => r.GetBySlugAtivoAsync("slug")).ReturnsAsync(existing);
            _repoMock.Setup(r => r.DisableAsync(existing.Id)).ReturnsAsync(true);
            _repoMock.Setup(r => r.InsertAsync(It.IsAny<Domain.Entities.ServicosEntity.CartaServicosUsuarioEntity.CartaServicosUsuario>())).Returns(Task.CompletedTask);
            _repoMock.Setup(r => r.CommitAsync()).ReturnsAsync(true);

            var svc = CreateService();

            var request = new CartaServicosUsuarioRequest { Slug = "slug", TituloPagina = "t", TituloPesquisa = "p", Ativo = true };

            var response = await svc.CreateAsync(request);

            _repoMock.Verify(r => r.GetBySlugAtivoAsync("slug"), Times.Once);
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
           // Assert.True(ok);
        }

        [Fact]
        public async Task UpdateAsync_NotFound_ReturnsFalseAndDoesNotCommit()
        {
            _repoMock.Setup(r => r.GetWithChildrenByIdAsync(999)).ReturnsAsync((Domain.Entities.ServicosEntity.CartaServicosUsuarioEntity.CartaServicosUsuario?)null);

            var svc = CreateService();

            var result = await svc.UpdateAsync(999, new CartaServicosUsuarioRequest());

            Assert.False(result);
            _repoMock.Verify(r => r.UpdateAsync(It.IsAny<Domain.Entities.ServicosEntity.CartaServicosUsuarioEntity.CartaServicosUsuario>()), Times.Never);
            _repoMock.Verify(r => r.CommitAsync(), Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_Found_UpdatesAndCommits()
        {
            var entity = new Domain.Entities.ServicosEntity.CartaServicosUsuarioEntity.CartaServicosUsuario { Id = 7 };
            _repoMock.Setup(r => r.GetWithChildrenByIdAsync(7)).ReturnsAsync(entity);
            _repoMock.Setup(r => r.UpdateAsync(entity)).Returns(Task.CompletedTask);
            _repoMock.Setup(r => r.CommitAsync()).ReturnsAsync(true);

            var svc = CreateService();

            var result = await svc.UpdateAsync(7, new CartaServicosUsuarioRequest { TituloPagina = "x" });

            _repoMock.Verify(r => r.UpdateAsync(entity), Times.Once);
            _repoMock.Verify(r => r.CommitAsync(), Times.Once);
            Assert.True(result);
        }

        [Fact]
        public async Task UpdateServicosAsync_NotFound_ThrowsInvalidOperation()
        {
            _repoMock.Setup(r => r.GetWithChildrenByIdAsync(50)).ReturnsAsync((Domain.Entities.ServicosEntity.CartaServicosUsuarioEntity.CartaServicosUsuario?)null);

            var svc = CreateService();

            await Assert.ThrowsAsync<InvalidOperationException>(() => svc.UpdateServicosAsync(50, Enumerable.Empty<CartaServicosUsuarioServicoUpdate>()));
        }

        [Fact]
        public async Task UpdateServicosAsync_Found_UpdatesAndCommits()
        {
            var servico = new CartaServicosUsuarioServico { };

            var entidade = new Domain.Entities.ServicosEntity.CartaServicosUsuarioEntity.CartaServicosUsuario { Servicos = new List<CartaServicosUsuarioServico> { servico } };
            _repoMock.Setup(r => r.GetWithChildrenByIdAsync(60)).ReturnsAsync(entidade);
            _repoServicosMock.Setup(r => r.UpdateServicosAsync(60, It.IsAny<IEnumerable<CartaServicosUsuarioServico>>()))
                .Returns(Task.CompletedTask);
            _repoMock.Setup(r => r.CommitAsync()).ReturnsAsync(true);

            var svc = CreateService();

            var updates = new List<CartaServicosUsuarioServicoUpdate>
            {
                new() { Id = 0, Ordem = 1, Titulo = "x", Ativo = true }
            };

            var ok = await svc.UpdateServicosAsync(60, updates);

            _repoServicosMock.Verify(r => r.UpdateServicosAsync(60, It.IsAny<IEnumerable<CartaServicosUsuarioServico>>()), Times.Once);
            _repoMock.Verify(r => r.CommitAsync(), Times.Once);
            //Assert.True(ok);
        }

        [Fact]
        public async Task UpdateServicosItensAsync_CallsRepoAndCommits()
        {
            _repoServicosItensMock.Setup(r => r.UpdateServicosItensAsync(70, It.IsAny<IEnumerable<CartaServicosUsuarioServicoItem>>()))
                .Returns(Task.CompletedTask);
            _repoMock.Setup(r => r.CommitAsync()).ReturnsAsync(true);

            var svc = CreateService();

            var itens = new List<CartaServicosUsuarioServicoItemUpdate> { new() { Id = 1, Ativo = true, Ordem = 1, Titulo = "x" } };

            var ok = await svc.UpdateServicosItensAsync(70, itens);

            _repoServicosItensMock.Verify(r => r.UpdateServicosItensAsync(70, It.IsAny<IEnumerable<CartaServicosUsuarioServicoItem>>()), Times.Once);
            _repoMock.Verify(r => r.CommitAsync(), Times.Once);
           // Assert.True(ok);
        }

        [Fact]
        public async Task UpdateServicosItensDetalhesAsync_CallsRepoAndCommits()
        {
            _repoItemDetalheMock.Setup(r => r.UpdateServicosItensDetalhesAsync(80, It.IsAny<IEnumerable<CartaServicosUsuarioItemDetalhe>>()))
                .Returns(Task.CompletedTask);
            _repoMock.Setup(r => r.CommitAsync()).ReturnsAsync(true);

            var svc = CreateService();

            var detalhes = new List<CartaServicosUsuarioItemDetalheUpdate> { new() { Id = 1, Ativo = true, Ordem = 1, TituloDetalhe = "d" } };

            var ok = await svc.UpdateServicosItensDetalhesAsync(80, detalhes);

            _repoItemDetalheMock.Verify(r => r.UpdateServicosItensDetalhesAsync(80, It.IsAny<IEnumerable<CartaServicosUsuarioItemDetalhe>>()), Times.Once);
            _repoMock.Verify(r => r.CommitAsync(), Times.Once);
           // Assert.True(ok);
        }

        [Fact]
        public async Task UpdateDescritivoItemDetalheAsync_CallsRepoAndCommits()
        {
            _repoDescritivoMock.Setup(r => r.UpdateDescritivoItemDetalheAsync(90, It.IsAny<IEnumerable<CartaServicosUsuarioDescritivoItemDetalhe>>()))
                .Returns(Task.CompletedTask);
            _repoMock.Setup(r => r.CommitAsync()).ReturnsAsync(true);

            var svc = CreateService();

            var descritivos = new List<CartaServicosUsuarioDescritivoItemDetalheUpdate> { new() { Id = 1, Ordem = 1, Descritivo = "desc" } };

            var ok = await svc.UpdateDescritivoItemDetalheAsync(90, descritivos);

            _repoDescritivoMock.Verify(r => r.UpdateDescritivoItemDetalheAsync(90, It.IsAny<IEnumerable<CartaServicosUsuarioDescritivoItemDetalhe>>()), Times.Once);
            _repoMock.Verify(r => r.CommitAsync(), Times.Once);
            //Assert.True(ok);
        }

        [Fact]
        public async Task DeleteAsync_NotFound_ReturnsFalse()
        {
            _repoMock.Setup(r => r.FindByIdAsync(123)).ReturnsAsync((Domain.Entities.ServicosEntity.CartaServicosUsuarioEntity.CartaServicosUsuario?)null);

            var svc = CreateService();

            var ok = await svc.DeleteAsync(123);

            Assert.False(ok);
            _repoMock.Verify(r => r.DeleteAsync(It.IsAny<Domain.Entities.ServicosEntity.CartaServicosUsuarioEntity.CartaServicosUsuario>()), Times.Never);
            _repoMock.Verify(r => r.CommitAsync(), Times.Never);
        }

        [Fact]
        public async Task DeleteAsync_Found_DeletesAndCommits()
        {
            var e = new Domain.Entities.ServicosEntity.CartaServicosUsuarioEntity.CartaServicosUsuario { Id = 200 };
            _repoMock.Setup(r => r.FindByIdAsync(200)).ReturnsAsync(e);
            _repoMock.Setup(r => r.DeleteAsync(e)).Returns(Task.CompletedTask);
            _repoMock.Setup(r => r.CommitAsync()).ReturnsAsync(true);

            var svc = CreateService();

            var ok = await svc.DeleteAsync(200);

            _repoMock.Verify(r => r.DeleteAsync(e), Times.Once);
            _repoMock.Verify(r => r.CommitAsync(), Times.Once);
            Assert.True(ok);
        }

        [Fact]
        public async Task GetWithChildrenBySlugAtivoAsync_ReturnsMappedOrNull()
        {
            var e = new Domain.Entities.ServicosEntity.CartaServicosUsuarioEntity.CartaServicosUsuario { Id = 300 };
            _repoMock.Setup(r => r.GetWithChildrenBySlugAtivoAsync("active-slug")).ReturnsAsync(e);
            var svc = CreateService();

            var res = await svc.GetWithChildrenBySlugAtivoAsync("active-slug");
            Assert.NotNull(res);

            _repoMock.Setup(r => r.GetWithChildrenBySlugAtivoAsync("nope")).ReturnsAsync((Domain.Entities.ServicosEntity.CartaServicosUsuarioEntity.CartaServicosUsuario?)null);
            var res2 = await svc.GetWithChildrenBySlugAtivoAsync("nope");
            Assert.Null(res2);
        }

        [Fact]
        public async Task DisableAsync_ProxiesRepoDisable()
        {
            _repoMock.Setup(r => r.DisableAsync(400)).ReturnsAsync(true);
            var svc = CreateService();

            var ok = await svc.DisableAsync(400);

            _repoMock.Verify(r => r.DisableAsync(400), Times.Once);
            Assert.True(ok);
        }
    }
}