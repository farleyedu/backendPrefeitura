using Moq;
using PortalTCMSP.Domain.DTOs.Requests.Servicos.OficioseIntimacoes;
using PortalTCMSP.Domain.Entities.ServicosEntity.OficioseIntimacoesEntity;
using PortalTCMSP.Domain.Repositories.Servicos.OficioseIntimacoes;
using PortalTCMSP.Infra.Services.Servicos.OficioseIntimacoes;

namespace PortalTCMSP.Infra.Tests.Services
{
    public class OficioseIntimacoesServiceTests
    {
        private readonly Mock<IOficioseIntimacoesRepository> _repoMock = new();
        private readonly Mock<IOficioseIntimacoesSecaoRepository> _repoSecoesMock = new();
        private readonly Mock<IOficioseIntimacoesSecaoItemRepository> _repoSecaoItensMock = new();

        private OficioseIntimacoesService CreateService()
            => new OficioseIntimacoesService(_repoMock.Object, _repoSecoesMock.Object, _repoSecaoItensMock.Object);

        [Fact]
        public async Task GetAllAsync_InvokesRepoAndReturnsMapped()
        {
            // Arrange
            var entities = new List<OficioseIntimacoes>
            {
                new OficioseIntimacoes { Id = 100, Titulo = "T", Slug = "s", Ativo = true, DataCriacao = DateTime.UtcNow }
            };
            _repoMock.Setup(r => r.AllWithChildrenAsync()).ReturnsAsync(entities);

            var svc = CreateService();

            // Act
            var result = await svc.GetAllAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(100, result.First().Id);
            _repoMock.Verify(r => r.AllWithChildrenAsync(), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsResponse_WhenFound()
        {
            var entity = new OficioseIntimacoes { Id = 5, Titulo = "X", Slug = "x", Ativo = true, DataCriacao = DateTime.UtcNow };
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
            _repoMock.Setup(r => r.GetWithChildrenByIdAsync(10)).ReturnsAsync((OficioseIntimacoes?)null);

            var svc = CreateService();
            var response = await svc.GetByIdAsync(10);

            Assert.Null(response);
            _repoMock.Verify(r => r.GetWithChildrenByIdAsync(10), Times.Once);
        }

        [Fact]
        public async Task DisableAsync_DelegatesToRepository()
        {
            _repoMock.Setup(r => r.DisableAsync(7)).ReturnsAsync(true);

            var svc = CreateService();
            var result = await svc.DisableAsync(7);

            Assert.True(result);
            _repoMock.Verify(r => r.DisableAsync(7), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_DisablesExistingSlug_InsertsAndCommits()
        {
            // Arrange: an active entity with same slug exists
            var existing = new OficioseIntimacoes { Id = 77, Slug = "slug-x" };
            _repoMock.Setup(r => r.GetBySlugAtivoAsync("slug-x")).ReturnsAsync(existing);
            _repoMock.Setup(r => r.DisableAsync(existing.Id)).ReturnsAsync(true);
            _repoMock.Setup(r => r.InsertAsync(It.IsAny<OficioseIntimacoes>())).Returns(Task.CompletedTask);
            _repoMock.Setup(r => r.CommitAsync()).ReturnsAsync(true);

            var request = new OficioseIntimacoesCreateRequest { Titulo = "t", Slug = "slug-x", Ativo = true };

            var svc = CreateService();

            // Act
            var id = await svc.CreateAsync(request);

            // Assert: ensure disabling existing, insert and commit were invoked
            _repoMock.Verify(r => r.GetBySlugAtivoAsync("slug-x"), Times.Once);
            _repoMock.Verify(r => r.DisableAsync(existing.Id), Times.Once);
            _repoMock.Verify(r => r.InsertAsync(It.IsAny<OficioseIntimacoes>()), Times.Once);
            _repoMock.Verify(r => r.CommitAsync(), Times.Once);
            Assert.IsType<long>(id);
        }

        [Fact]
        public async Task CreateSecoesAsync_Throws_WhenParentNotFound()
        {
            _repoMock.Setup(r => r.FindByIdAsync(999)).ReturnsAsync((OficioseIntimacoes?)null);

            var svc = CreateService();

            await Assert.ThrowsAsync<InvalidOperationException>(() => svc.CreateSecoesAsync(999, new List<OficioseIntimacoesSecaoRequest>()));
            _repoMock.Verify(r => r.FindByIdAsync(999), Times.Once);
        }

        [Fact]
        public async Task CreateSecoesAsync_DelegatesToSecaoRepoAndCommits()
        {
            var parent = new OficioseIntimacoes { Id = 2 };
            _repoMock.Setup(r => r.FindByIdAsync(2)).ReturnsAsync(parent);
            _repoSecoesMock.Setup(r => r.CreateSecoesAsync(2, It.IsAny<IEnumerable<OficioseIntimacoesSecao>>())).Returns(Task.CompletedTask);
            _repoSecoesMock.Setup(r => r.CommitAsync()).ReturnsAsync(true);

            var reqs = new List<OficioseIntimacoesSecaoRequest>
            {
                new OficioseIntimacoesSecaoRequest { Ordem = 1, Nome = "One", SecaoItem = new List<OficioseIntimacoesSecaoItemRequest> { new() { Ordem = 1, Descricao = "a" } } }
            };

            var svc = CreateService();
            var result = await svc.CreateSecoesAsync(2, reqs);

            Assert.True(result);
            _repoSecoesMock.Verify(r => r.CreateSecoesAsync(2, It.IsAny<IEnumerable<OficioseIntimacoesSecao>>()), Times.Once);
            _repoSecoesMock.Verify(r => r.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task CreateSecaoItensAsync_Throws_WhenSecaoNotFound()
        {
            _repoSecoesMock.Setup(r => r.FindByIdAsync(55)).ReturnsAsync((OficioseIntimacoesSecao?)null);

            var svc = CreateService();

            await Assert.ThrowsAsync<InvalidOperationException>(() => svc.CreateSecaoItensAsync(55, new List<OficioseIntimacoesSecaoItemRequest>()));
            _repoSecoesMock.Verify(r => r.FindByIdAsync(55), Times.Once);
        }

        [Fact]
        public async Task CreateSecaoItensAsync_DelegatesToRepoAndCommits()
        {
            var secao = new OficioseIntimacoesSecao { Id = 10, IdOficioseIntimacoes = 3 };
            _repoSecoesMock.Setup(r => r.FindByIdAsync(10)).ReturnsAsync(secao);
            _repoSecaoItensMock.Setup(r => r.CreateSecaoItensAsync(10, It.IsAny<IEnumerable<OficioseIntimacoesSecaoItem>>())).Returns(Task.CompletedTask);
            _repoSecaoItensMock.Setup(r => r.CommitAsync()).ReturnsAsync(true);

            var itens = new List<OficioseIntimacoesSecaoItemRequest> { new() { Ordem = 1, Descricao = "X" } };

            var svc = CreateService();
            var result = await svc.CreateSecaoItensAsync(10, itens);

            Assert.True(result);
            _repoSecaoItensMock.Verify(r => r.CreateSecaoItensAsync(10, It.IsAny<IEnumerable<OficioseIntimacoesSecaoItem>>()), Times.Once);
            _repoSecaoItensMock.Verify(r => r.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task GetWithChildrenBySlugAtivoAsync_ReturnsResponse_WhenFound()
        {
            var entity = new OficioseIntimacoes { Id = 33, Slug = "abc" };
            _repoMock.Setup(r => r.GetWithChildrenBySlugAtivoAsync("abc")).ReturnsAsync(entity);

            var svc = CreateService();
            var resp = await svc.GetWithChildrenBySlugAtivoAsync("abc");

            Assert.NotNull(resp);
            Assert.Equal(33, resp!.Id);
            _repoMock.Verify(r => r.GetWithChildrenBySlugAtivoAsync("abc"), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsFalse_WhenNotFound()
        {
            _repoMock.Setup(r => r.GetWithChildrenByIdAsync(123)).ReturnsAsync((OficioseIntimacoes?)null);

            var svc = CreateService();
            var result = await svc.UpdateAsync(123, new OficioseIntimacoesUpdateRequest());

            Assert.False(result);
            _repoMock.Verify(r => r.GetWithChildrenByIdAsync(123), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_Success_ReplacesSecoes_UpdatesAndCommits()
        {
            var existing = new OficioseIntimacoes
            {
                Id = 50,
                Titulo = "old",
                Slug = "s",
                Ativo = true,
                DataCriacao = DateTime.UtcNow,
                Secoes = new List<OficioseIntimacoesSecao>()
            };
            _repoMock.Setup(r => r.GetWithChildrenByIdAsync(50)).ReturnsAsync(existing);
            _repoMock.Setup(r => r.ReplaceSecoesAsync(50, It.IsAny<IEnumerable<OficioseIntimacoesSecao>>())).Returns(Task.CompletedTask);
            _repoMock.Setup(r => r.UpdateAsync(existing)).Returns(Task.CompletedTask);
            _repoMock.Setup(r => r.CommitAsync()).ReturnsAsync(true);

            var request = new OficioseIntimacoesUpdateRequest
            {
                Titulo = "new",
                Slug = "s2",
                Ativo = false,
                Secoes = new List<OficioseIntimacoesSecaoRequest>
                {
                    new() { Ordem = 1, Nome = "ns", SecaoItem = new List<OficioseIntimacoesSecaoItemRequest>{ new(){ Ordem = 1, Descricao = "i" } } }
                }
            };

            var svc = CreateService();
            var result = await svc.UpdateAsync(50, request);

            Assert.True(result);
            _repoMock.Verify(r => r.ReplaceSecoesAsync(50, It.IsAny<IEnumerable<OficioseIntimacoesSecao>>()), Times.Once);
            _repoMock.Verify(r => r.UpdateAsync(existing), Times.Once);
            _repoMock.Verify(r => r.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateSecoesAsync_Throws_WhenParentNotFound()
        {
            _repoMock.Setup(r => r.FindByIdAsync(999)).ReturnsAsync((OficioseIntimacoes?)null);

            var svc = CreateService();

            await Assert.ThrowsAsync<InvalidOperationException>(() => svc.UpdateSecoesAsync(999, new List<OficioseIntimacoesSecaoUpdate>()));
            _repoMock.Verify(r => r.FindByIdAsync(999), Times.Once);
        }

        [Fact]
        public async Task UpdateSecoesAsync_Throws_WhenNoSecoes()
        {
            _repoMock.Setup(r => r.FindByIdAsync(7)).ReturnsAsync(new OficioseIntimacoes { Id = 7 });
            _repoMock.Setup(r => r.GetWithChildrenByIdAsync(7)).ReturnsAsync((OficioseIntimacoes?)null);

            var svc = CreateService();

            await Assert.ThrowsAsync<InvalidOperationException>(() => svc.UpdateSecoesAsync(7, new List<OficioseIntimacoesSecaoUpdate>()));
            _repoMock.Verify(r => r.GetWithChildrenByIdAsync(7), Times.Once);
        }

        [Fact]
        public async Task UpdateSecoesAsync_Success_UpdatesAndCommits()
        {
            var oldSecao = new OficioseIntimacoesSecao { Id = 21, Ordem = 1, Nome = "old" };
            var aggregate = new OficioseIntimacoes { Id = 3, Secoes = new List<OficioseIntimacoesSecao> { oldSecao } };

            _repoMock.Setup(r => r.FindByIdAsync(3)).ReturnsAsync(new OficioseIntimacoes { Id = 3 });
            _repoMock.Setup(r => r.GetWithChildrenByIdAsync(3)).ReturnsAsync(aggregate);
            _repoSecoesMock.Setup(r => r.FindByIdAsync(21)).ReturnsAsync(oldSecao);
            _repoSecoesMock.Setup(r => r.UpdateSecoesAsync(3, It.IsAny<IEnumerable<OficioseIntimacoesSecao>>())).Returns(Task.CompletedTask);
            _repoSecoesMock.Setup(r => r.CommitAsync()).ReturnsAsync(true);

            var updates = new List<OficioseIntimacoesSecaoUpdate> { new() { Id = 21, Ordem = 2, Nome = "new" } };

            var svc = CreateService();
            var result = await svc.UpdateSecoesAsync(3, updates);

            Assert.True(result);
            _repoSecoesMock.Verify(r => r.UpdateSecoesAsync(3, It.Is<IEnumerable<OficioseIntimacoesSecao>>(s => s.First().Nome == "new" || s.First().Ordem == 2)), Times.Once);
            _repoSecoesMock.Verify(r => r.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateSecaoItensAsync_Throws_WhenSecaoNotFound()
        {
            _repoSecoesMock.Setup(r => r.FindByIdAsync(999)).ReturnsAsync((OficioseIntimacoesSecao?)null);

            var svc = CreateService();
            await Assert.ThrowsAsync<InvalidOperationException>(() => svc.UpdateSecaoItensAsync(999, new List<OficioseIntimacoesSecaoItemUpdate>()));
            _repoSecoesMock.Verify(r => r.FindByIdAsync(999), Times.Once);
        }

        [Fact]
        public async Task UpdateSecaoItensAsync_Throws_WhenParentAggregateNotFound()
        {
            var secao = new OficioseIntimacoesSecao { Id = 11, IdOficioseIntimacoes = 77 };
            _repoSecoesMock.Setup(r => r.FindByIdAsync(11)).ReturnsAsync(secao);
            _repoMock.Setup(r => r.GetWithChildrenByIdAsync(secao.IdOficioseIntimacoes)).ReturnsAsync((OficioseIntimacoes?)null);

            var svc = CreateService();
            await Assert.ThrowsAsync<InvalidOperationException>(() => svc.UpdateSecaoItensAsync(11, new List<OficioseIntimacoesSecaoItemUpdate>()));
            _repoMock.Verify(r => r.GetWithChildrenByIdAsync(secao.IdOficioseIntimacoes), Times.Once);
        }

        [Fact]
        public async Task UpdateSecaoItensAsync_Success_UpdatesAndCommits()
        {
            var itemOld = new OficioseIntimacoesSecaoItem { Id = 301, Ordem = 1, Descricao = "o" };
            var secao = new OficioseIntimacoesSecao { Id = 30, IdOficioseIntimacoes = 9, SecaoItem = new List<OficioseIntimacoesSecaoItem> { itemOld } };
            var aggregate = new OficioseIntimacoes { Id = 9, Secoes = new List<OficioseIntimacoesSecao> { secao } };

            _repoSecoesMock.Setup(r => r.FindByIdAsync(30)).ReturnsAsync(secao);
            _repoMock.Setup(r => r.GetWithChildrenByIdAsync(9)).ReturnsAsync(aggregate);
            _repoSecaoItensMock.Setup(r => r.FindByIdAsync(301)).ReturnsAsync(itemOld);
            _repoSecaoItensMock.Setup(r => r.UpdateSecaoItensAsync(30, It.IsAny<IEnumerable<OficioseIntimacoesSecaoItem>>())).Returns(Task.CompletedTask);
            _repoSecaoItensMock.Setup(r => r.CommitAsync()).ReturnsAsync(true);

            var updates = new List<OficioseIntimacoesSecaoItemUpdate> { new() { Id = 301, Ordem = 2, Descricao = "n" } };

            var svc = CreateService();
            var result = await svc.UpdateSecaoItensAsync(30, updates);

            Assert.True(result);
            _repoSecaoItensMock.Verify(r => r.UpdateSecaoItensAsync(30, It.Is<IEnumerable<OficioseIntimacoesSecaoItem>>(it => it.First().Ordem == 2 || it.First().Descricao == "n")), Times.Once);
            _repoSecaoItensMock.Verify(r => r.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsFalse_WhenNotFound()
        {
            _repoMock.Setup(r => r.FindByIdAsync(404)).ReturnsAsync((OficioseIntimacoes?)null);

            var svc = CreateService();
            var result = await svc.DeleteAsync(404);

            Assert.False(result);
            _repoMock.Verify(r => r.FindByIdAsync(404), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_DeletesAndCommits_WhenFound()
        {
            var entity = new OficioseIntimacoes { Id = 2 };
            _repoMock.Setup(r => r.FindByIdAsync(2)).ReturnsAsync(entity);
            _repoMock.Setup(r => r.DeleteAsync(entity)).Returns(Task.CompletedTask);
            _repoMock.Setup(r => r.CommitAsync()).ReturnsAsync(true);

            var svc = CreateService();
            var result = await svc.DeleteAsync(2);

            Assert.True(result);
            _repoMock.Verify(r => r.DeleteAsync(entity), Times.Once);
            _repoMock.Verify(r => r.CommitAsync(), Times.Once);
        }
    }
}