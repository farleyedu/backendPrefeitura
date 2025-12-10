using Moq;
using PortalTCMSP.Domain.DTOs.Requests.Servicos.PrazosProcessuais;
using PortalTCMSP.Domain.Entities.ServicosEntity.PrazosProcessuaisEntity;
using PortalTCMSP.Domain.Repositories.Servicos.PrazosProcessuais;
using PortalTCMSP.Infra.Services.Servicos.PrazosProcessuais;

namespace PortalTCMSP.Tests.Services
{
    public class PrazosProcessuaisServiceTests
    {
        private readonly Mock<IPrazosProcessuaisRepository> _repoMock = new();
        private readonly Mock<IPrazosProcessuaisItemRepository> _itemRepoMock = new();
        private readonly Mock<IPrazosProcessuaisItemAnexoRepository> _anexoRepoMock = new();

        private PrazosProcessuaisService CreateService() =>
            new PrazosProcessuaisService(_repoMock.Object, _itemRepoMock.Object, _anexoRepoMock.Object);

        [Fact]
        public async Task GetAllAsync_CallsRepoAndReturnsMappedCollection()
        {
            // Arrange
            var entities = new List<PrazosProcessuais>
            {
                new PrazosProcessuais { Id = 1, PrazosProcessuaisItens = new List<PrazosProcessuaisItem>() },
                new PrazosProcessuais { Id = 2, PrazosProcessuaisItens = new List<PrazosProcessuaisItem>() }
            };
            _repoMock.Setup(r => r.AllWithChildrenAsync()).ReturnsAsync(entities);

            var svc = CreateService();

            // Act
            var result = await svc.GetAllAsync();

            // Assert
            _repoMock.Verify(r => r.AllWithChildrenAsync(), Times.Once);
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetByIdAsync_WhenEntityExists_ReturnsResponse()
        {
            // Arrange
            var entity = new PrazosProcessuais { Id = 5, PrazosProcessuaisItens = new List<PrazosProcessuaisItem>() };
            _repoMock.Setup(r => r.GetWithChildrenByIdAsync(5)).ReturnsAsync(entity);

            var svc = CreateService();

            // Act
            var response = await svc.GetByIdAsync(5);

            // Assert
            _repoMock.Verify(r => r.GetWithChildrenByIdAsync(5), Times.Once);
            Assert.NotNull(response);
            Assert.Equal(5, response!.Id);
        }

        [Fact]
        public async Task GetByIdAsync_WhenEntityNotFound_ReturnsNull()
        {
            // Arrange
            _repoMock.Setup(r => r.GetWithChildrenByIdAsync(It.IsAny<long>())).ReturnsAsync((PrazosProcessuais?)null);
            var svc = CreateService();

            // Act
            var response = await svc.GetByIdAsync(123);

            // Assert
            Assert.Null(response);
        }

        [Fact]
        public async Task CreateAsync_WhenSlugExists_DisablesExistingThenInsertsAndCommits()
        {
            // Arrange
            var existing = new PrazosProcessuais { Id = 77, PrazosProcessuaisItens = new List<PrazosProcessuaisItem>() };
            _repoMock.Setup(r => r.GetBySlugAtivoAsync("my-slug")).ReturnsAsync(existing);
            _repoMock.Setup(r => r.DisableAsync(existing.Id)).ReturnsAsync(true);
            _repoMock.Setup(r => r.InsertAsync(It.IsAny<PrazosProcessuais>())).Returns(Task.CompletedTask);
            _repoMock.Setup(r => r.CommitAsync()).ReturnsAsync(true);

            var svc = CreateService();
            var request = new PrazosProcessuaisCreateRequest
            {
                Slug = "my-slug",
                TituloPagina = "Title",
                Ativo = true
            };

            // Act
            var response = await svc.CreateAsync(request);

            // Assert
            _repoMock.Verify(r => r.GetBySlugAtivoAsync("my-slug"), Times.Once);
            _repoMock.Verify(r => r.DisableAsync(77), Times.Once);
            _repoMock.Verify(r => r.InsertAsync(It.IsAny<PrazosProcessuais>()), Times.Once);
            _repoMock.Verify(r => r.CommitAsync(), Times.Once);
            Assert.NotNull(response);
            Assert.Equal(request.Slug, response.Slug);
        }

        [Fact]
        public async Task CreateAsync_WhenSlugDoesNotExist_InsertsAndCommits()
        {
            // Arrange
            _repoMock.Setup(r => r.GetBySlugAtivoAsync(It.IsAny<string>())).ReturnsAsync((PrazosProcessuais?)null);
            _repoMock.Setup(r => r.InsertAsync(It.IsAny<PrazosProcessuais>())).Returns(Task.CompletedTask);
            _repoMock.Setup(r => r.CommitAsync()).ReturnsAsync(true);

            var svc = CreateService();
            var request = new PrazosProcessuaisCreateRequest
            {
                Slug = "unique-slug",
                TituloPagina = "Title",
                Ativo = true
            };

            // Act
            var response = await svc.CreateAsync(request);

            // Assert
            _repoMock.Verify(r => r.GetBySlugAtivoAsync("unique-slug"), Times.Once);
            _repoMock.Verify(r => r.DisableAsync(It.IsAny<long>()), Times.Never);
            _repoMock.Verify(r => r.InsertAsync(It.IsAny<PrazosProcessuais>()), Times.Once);
            _repoMock.Verify(r => r.CommitAsync(), Times.Once);
            Assert.NotNull(response);
            Assert.Equal(request.Slug, response.Slug);
        }

        [Fact]
        public async Task CreateAnexoAsync_CallsRepoCreateAndCommits_ReturnsCommitResult()
        {
            // Arrange
            var captured = (IEnumerable<PrazosProcessuaisItemAnexo>?)null;
            _anexoRepoMock
                .Setup(a => a.CreateAnexoAsync(10, It.IsAny<IEnumerable<PrazosProcessuaisItemAnexo>>()))
                .Callback<long, IEnumerable<PrazosProcessuaisItemAnexo>>((id, items) => captured = items)
                .Returns(Task.CompletedTask);

            _repoMock.Setup(r => r.CommitAsync()).ReturnsAsync(true);

            var svc = CreateService();
            var novos = new[]
            {
                new PrazosProcessuaisItemAnexoRequest { Ativo = true, NomeArquivo = " a.pdf ", Ordem = 1, Url = " u ", Tipo = " t " }
            };

            // Act
            var result = await svc.CreateAnexoAsync(10, novos);

            // Assert
            _anexoRepoMock.Verify(a => a.CreateAnexoAsync(10, It.IsAny<IEnumerable<PrazosProcessuaisItemAnexo>>()), Times.Once);
            _repoMock.Verify(r => r.CommitAsync(), Times.Once);
            Assert.True(result);
            Assert.NotNull(captured);
            Assert.Single(captured!);
            // The service maps request.ToEntity(id) — ensure id mapping happened
            Assert.All(captured!, item => Assert.Equal(10, item.IdPrazosProcessuaisItem));
        }

        [Fact]
        public async Task CreatePrazosProcessuaisItemAsync_CallsRepoCreateAndCommits_ReturnsCommitResult()
        {
            // Arrange
            var captured = (IEnumerable<PrazosProcessuaisItem>?)null;
            _itemRepoMock
                .Setup(i => i.CreatePrazosProcessuaisItemAsync(20, It.IsAny<IEnumerable<PrazosProcessuaisItem>>()))
                .Callback<long, IEnumerable<PrazosProcessuaisItem>>((id, items) => captured = items)
                .Returns(Task.CompletedTask);

            _repoMock.Setup(r => r.CommitAsync()).ReturnsAsync(true);

            var svc = CreateService();
            var novos = new[]
            {
                new PrazosProcessuaisItemRequest { Ativo = true, Nome = " Item ", Ordem = 2, DataPublicacao = DateTime.UtcNow, TempoDecorrido = "1d" }
            };

            // Act
            var result = await svc.CreatePrazosProcessuaisItemAsync(20, novos);

            // Assert
            _itemRepoMock.Verify(i => i.CreatePrazosProcessuaisItemAsync(20, It.IsAny<IEnumerable<PrazosProcessuaisItem>>()), Times.Once);
            _repoMock.Verify(r => r.CommitAsync(), Times.Once);
            Assert.True(result);
            Assert.NotNull(captured);
            Assert.Single(captured!);
            Assert.All(captured!, item => Assert.Equal(20, item.IdPrazosProcessuais));
        }

        [Fact]
        public async Task UpdateAsync_WhenEntityNotFound_ReturnsFalse()
        {
            // Arrange
            _repoMock.Setup(r => r.GetWithChildrenByIdAsync(It.IsAny<long>())).ReturnsAsync((PrazosProcessuais?)null);
            var svc = CreateService();
            var request = new PrazosProcessuaisCreateRequest { Slug = "s", TituloPagina = "t", Ativo = true };

            // Act
            var result = await svc.UpdateAsync(100, request);

            // Assert
            Assert.False(result);
            _repoMock.Verify(r => r.UpdateAsync(It.IsAny<PrazosProcessuais>()), Times.Never);
            _repoMock.Verify(r => r.CommitAsync(), Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_WhenEntityExists_UpdatesAndCommits_ReturnsCommitResult()
        {
            // Arrange
            var entity = new PrazosProcessuais { Id = 4, PrazosProcessuaisItens = new List<PrazosProcessuaisItem>() };
            _repoMock.Setup(r => r.GetWithChildrenByIdAsync(4)).ReturnsAsync(entity);
            _repoMock.Setup(r => r.UpdateAsync(It.IsAny<PrazosProcessuais>())).Returns(Task.CompletedTask);
            _repoMock.Setup(r => r.CommitAsync()).ReturnsAsync(true);

            var svc = CreateService();
            var request = new PrazosProcessuaisCreateRequest { Slug = "s", TituloPagina = "t2", Ativo = false };

            // Act
            var result = await svc.UpdateAsync(4, request);

            // Assert
            Assert.True(result);
            _repoMock.Verify(r => r.UpdateAsync(It.Is<PrazosProcessuais>(p => p.Id == 4)), Times.Once);
            _repoMock.Verify(r => r.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateAnexoAsync_TrimsStrings_UpdatesAndCommits()
        {
            // Arrange
            IEnumerable<PrazosProcessuaisItemAnexo>? captured = null;
            _anexoRepoMock
                .Setup(a => a.UpdateAnexoAsync(5, It.IsAny<IEnumerable<PrazosProcessuaisItemAnexo>>()))
                .Callback<long, IEnumerable<PrazosProcessuaisItemAnexo>>((id, items) => captured = items)
                .Returns(Task.CompletedTask);

            _repoMock.Setup(r => r.CommitAsync()).ReturnsAsync(true);

            var svc = CreateService();
            var updates = new[]
            {
                new PrazosProcessuaisItemAnexoUpdate { Id = 1, NomeArquivo = " file ", Url = " url ", Tipo = " type ", Ordem = 3, Ativo = true }
            };

            // Act
            var result = await svc.UpdateAnexoAsync(5, updates);

            // Assert
            Assert.True(result);
            _anexoRepoMock.Verify(a => a.UpdateAnexoAsync(5, It.IsAny<IEnumerable<PrazosProcessuaisItemAnexo>>()), Times.Once);
            _repoMock.Verify(r => r.CommitAsync(), Times.Once);

            Assert.NotNull(captured);
            var item = captured!.Single();
            Assert.Equal(1, item.Id);
            Assert.Equal(5, item.IdPrazosProcessuaisItem);
            Assert.Equal("file", item.NomeArquivo);
            Assert.Equal("url", item.Url);
            Assert.Equal("type", item.Tipo);
        }

        [Fact]
        public async Task UpdatePrazosProcessuaisItemAsync_WhenParentNotFound_ThrowsInvalidOperationException()
        {
            // Arrange
            _repoMock.Setup(r => r.GetWithChildrenByIdAsync(It.IsAny<long>())).ReturnsAsync((PrazosProcessuais?)null);
            var svc = CreateService();

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => svc.UpdatePrazosProcessuaisItemAsync(2, Array.Empty<PrazosProcessuaisItemUpdate>()));
        }

        [Fact]
        public async Task UpdatePrazosProcessuaisItemAsync_WhenItemsExist_UpdatesAndCommits_ReturnsCommitResult()
        {
            // Arrange
            var existingItem = new PrazosProcessuaisItem { Id = 11, IdPrazosProcessuais = 9, Nome = " old ", Ordem = 1, Ativo = true, DataPublicacao = DateTime.UtcNow, TempoDecorrido = "0" };
            var parent = new PrazosProcessuais { Id = 9, PrazosProcessuaisItens = new List<PrazosProcessuaisItem> { existingItem } };

            _repoMock.Setup(r => r.GetWithChildrenByIdAsync(9)).ReturnsAsync(parent);
            IEnumerable<PrazosProcessuaisItem>? captured = null;
            _itemRepoMock
                .Setup(i => i.UpdatePrazosProcessuaisItemAsync(9, It.IsAny<IEnumerable<PrazosProcessuaisItem>>()))
                .Callback<long, IEnumerable<PrazosProcessuaisItem>>((id, items) => captured = items)
                .Returns(Task.CompletedTask);

            _repoMock.Setup(r => r.CommitAsync()).ReturnsAsync(true);

            var svc = CreateService();
            var updates = new[]
            {
                new PrazosProcessuaisItemUpdate { Id = 11, Nome = " new name ", Ordem = 2, Ativo = false, DataPublicacao = existingItem.DataPublicacao, TempoDecorrido = "1d" }
            };

            // Act
            var result = await svc.UpdatePrazosProcessuaisItemAsync(9, updates);

            // Assert
            Assert.True(result);
            _itemRepoMock.Verify(i => i.UpdatePrazosProcessuaisItemAsync(9, It.IsAny<IEnumerable<PrazosProcessuaisItem>>()), Times.Once);
            _repoMock.Verify(r => r.CommitAsync(), Times.Once);

            Assert.NotNull(captured);
            var updated = captured!.Single();
            Assert.Equal(11, updated.Id);
            Assert.Equal("new name", updated.Nome);
            Assert.Equal(2, updated.Ordem);
            Assert.False(updated.Ativo);
        }

        [Fact]
        public async Task DeleteAsync_WhenNotFound_ReturnsFalse()
        {
            // Arrange
            _repoMock.Setup(r => r.FindByIdAsync(It.IsAny<long>())).ReturnsAsync((PrazosProcessuais?)null);
            var svc = CreateService();

            // Act
            var result = await svc.DeleteAsync(99);

            // Assert
            Assert.False(result);
            _repoMock.Verify(r => r.DeleteAsync(It.IsAny<PrazosProcessuais>()), Times.Never);
            _repoMock.Verify(r => r.CommitAsync(), Times.Never);
        }

        [Fact]
        public async Task DeleteAsync_WhenFound_DeletesAndCommits_ReturnsCommitResult()
        {
            // Arrange
            var entity = new PrazosProcessuais { Id = 33, PrazosProcessuaisItens = new List<PrazosProcessuaisItem>() };
            _repoMock.Setup(r => r.FindByIdAsync(33)).ReturnsAsync(entity);
            _repoMock.Setup(r => r.DeleteAsync(entity)).Returns(Task.CompletedTask);
            _repoMock.Setup(r => r.CommitAsync()).ReturnsAsync(true);

            var svc = CreateService();

            // Act
            var result = await svc.DeleteAsync(33);

            // Assert
            Assert.True(result);
            _repoMock.Verify(r => r.DeleteAsync(entity), Times.Once);
            _repoMock.Verify(r => r.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task GetWithChildrenBySlugAtivoAsync_ReturnsMappedResponse()
        {
            // Arrange
            var entity = new PrazosProcessuais { Id = 44, PrazosProcessuaisItens = new List<PrazosProcessuaisItem>() };
            _repoMock.Setup(r => r.GetWithChildrenBySlugAtivoAsync("slug-x")).ReturnsAsync(entity);

            var svc = CreateService();

            // Act
            var response = await svc.GetWithChildrenBySlugAtivoAsync("slug-x");

            // Assert
            _repoMock.Verify(r => r.GetWithChildrenBySlugAtivoAsync("slug-x"), Times.Once);
            Assert.NotNull(response);
            Assert.Equal(44, response!.Id);
        }

        [Fact]
        public async Task DisableAsync_ForwardsCallToRepository()
        {
            // Arrange
            _repoMock.Setup(r => r.DisableAsync(55)).ReturnsAsync(true);
            var svc = CreateService();

            // Act
            var result = await svc.DisableAsync(55);

            // Assert
            _repoMock.Verify(r => r.DisableAsync(55), Times.Once);
            Assert.True(result);
        }
    }
}