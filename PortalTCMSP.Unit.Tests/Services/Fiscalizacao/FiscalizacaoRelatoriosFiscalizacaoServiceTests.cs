using Moq;
using PortalTCMSP.Domain.DTOs.Requests.Fiscalizacao.RelatorioFiscalizacao;
using PortalTCMSP.Domain.Entities.FiscalizacaoEntity.RelatorioFiscalizacao;
using PortalTCMSP.Domain.Repositories.Fiscalizacao;
using PortalTCMSP.Infra.Services.Fiscalizacao;
using System.Linq.Expressions;


namespace PortalTCMSP.Unit.Tests.Services.Fiscalizacao
{
    public class FiscalizacaoRelatoriosFiscalizacaoServiceTests
    {
        private readonly Mock<IFiscalizacaoRelatorioFiscalizacaoRepository> _repoMock;
        private readonly FiscalizacaoRelatoriosFiscalizacaoService _service;

        public FiscalizacaoRelatoriosFiscalizacaoServiceTests()
        {
            _repoMock = new Mock<IFiscalizacaoRelatorioFiscalizacaoRepository>();
            _service = new FiscalizacaoRelatoriosFiscalizacaoService(_repoMock.Object);
        }

        private static FiscalizacaoRelatorioFiscalizacao CreateEntity(long id = 1, string slug = "slug")
            => new FiscalizacaoRelatorioFiscalizacao
            {
                Id = id,
                Slug = slug,
                Titulo = "titulo",
                Descricao = "desc",
                Ativo = true,
                DataCriacao = DateTime.UtcNow,
                Carrocel = []
            };

        [Fact]
        public async Task GetAllAsync_ReturnsMappedResponses()
        {
            var entity = CreateEntity(5, "all-slug");
            _repoMock.Setup(r => r.AllWithChildrenAsync()).ReturnsAsync(new List<FiscalizacaoRelatorioFiscalizacao> { entity });

            var result = (await _service.GetAllAsync()).ToList();

            Assert.Single(result);
            Assert.Equal(entity.Id, result[0].Id);
            Assert.Equal(entity.Slug, result[0].Slug);
            _repoMock.Verify(r => r.AllWithChildrenAsync(), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsResponse_WhenFound()
        {
            var entity = CreateEntity(7, "by-id");
            _repoMock.Setup(r => r.GetWithChildrenByIdAsync(7)).ReturnsAsync(entity);

            var result = await _service.GetByIdAsync(7);

            Assert.NotNull(result);
            Assert.Equal(entity.Id, result!.Id);
            _repoMock.Verify(r => r.GetWithChildrenByIdAsync(7), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNull_WhenNotFound()
        {
            _repoMock.Setup(r => r.GetWithChildrenByIdAsync(It.IsAny<long>())).ReturnsAsync((FiscalizacaoRelatorioFiscalizacao?)null);

            var result = await _service.GetByIdAsync(999);

            Assert.Null(result);
        }

        [Fact]
        public async Task GetBySlugAsync_ReturnsResponse_WhenFound()
        {
            var entity = CreateEntity(8, "by-slug");
            _repoMock.Setup(r => r.GetWithChildrenBySlugAsync("by-slug")).ReturnsAsync(entity);

            var result = await _service.GetBySlugAsync("by-slug");

            Assert.NotNull(result);
            Assert.Equal(entity.Slug, result!.Slug);
            _repoMock.Verify(r => r.GetWithChildrenBySlugAsync("by-slug"), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_Succeeds_ReturnsNewId()
        {
            var request = new FiscalizacaoRelatorioFiscalizacaoCreateRequest
            {
                Slug = "New Slug",
                Titulo = "Title",
                Descricao = "Desc",
                Ativo = true,
                Carrocel = new List<FiscalizacaoRelatorioFiscalizacaoCarrosselRequest>()
            };

            _repoMock
                .Setup(r => r.AnyAsync(It.IsAny<Expression<Func<FiscalizacaoRelatorioFiscalizacao, bool>>>()))
                .ReturnsAsync(false);

            _repoMock
                .Setup(r => r.InsertAsync(It.IsAny<FiscalizacaoRelatorioFiscalizacao>()))
                .Callback<FiscalizacaoRelatorioFiscalizacao>(e => e.Id = 123)
                .Returns(Task.CompletedTask);

            _repoMock.Setup(r => r.CommitAsync()).ReturnsAsync(true);

            var id = await _service.CreateAsync(request);

            Assert.Equal(123, id);
            _repoMock.Verify(r => r.InsertAsync(It.IsAny<FiscalizacaoRelatorioFiscalizacao>()), Times.Once);
            _repoMock.Verify(r => r.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_Throws_WhenSlugAlreadyExists()
        {
            var request = new FiscalizacaoRelatorioFiscalizacaoCreateRequest { Slug = "Duplicate" };

            _repoMock
                .Setup(r => r.AnyAsync(It.IsAny<Expression<Func<FiscalizacaoRelatorioFiscalizacao, bool>>>()))
                .ReturnsAsync(true);

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => _service.CreateAsync(request));
            Assert.Equal("Já existe um conteúdo com este slug.", ex.Message);
            _repoMock.Verify(r => r.InsertAsync(It.IsAny<FiscalizacaoRelatorioFiscalizacao>()), Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsFalse_WhenNotFound()
        {
            _repoMock.Setup(r => r.GetWithChildrenByIdAsync(50)).ReturnsAsync((FiscalizacaoRelatorioFiscalizacao?)null);

            var request = new FiscalizacaoRelatorioFiscalizacaoConteudoUpdateRequest { Slug = "whatever" };
            var result = await _service.UpdateAsync(50, request);

            Assert.False(result);
            _repoMock.Verify(r => r.UpdateAsync(It.IsAny<FiscalizacaoRelatorioFiscalizacao>()), Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_Throws_WhenSlugConflictsWithAnotherEntity()
        {
            var existing = CreateEntity(20, "old-slug");
            _repoMock.Setup(r => r.GetWithChildrenByIdAsync(20)).ReturnsAsync(existing);

            _repoMock
                .Setup(r => r.AnyAsync(It.IsAny<Expression<Func<FiscalizacaoRelatorioFiscalizacao, bool>>>()))
                .ReturnsAsync(true);

            var request = new FiscalizacaoRelatorioFiscalizacaoConteudoUpdateRequest { Slug = "new-slug" };

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => _service.UpdateAsync(20, request));
            Assert.Equal("Já existe um conteúdo com este slug.", ex.Message);

            _repoMock.Verify(r => r.ReplaceCarrosselAsync(It.IsAny<long>(), It.IsAny<IEnumerable<FiscalizacaoRelatorioFiscalizacaoCarrossel>>()), Times.Never);
            _repoMock.Verify(r => r.UpdateAsync(It.IsAny<FiscalizacaoRelatorioFiscalizacao>()), Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_Succeeds_ReturnsTrue()
        {
            var existing = CreateEntity(30, "same-slug");
            _repoMock.Setup(r => r.GetWithChildrenByIdAsync(30)).ReturnsAsync(existing);

            _repoMock
                .Setup(r => r.AnyAsync(It.IsAny<Expression<Func<FiscalizacaoRelatorioFiscalizacao, bool>>>()))
                .ReturnsAsync(false);

            _repoMock.Setup(r => r.ReplaceCarrosselAsync(existing.Id, It.IsAny<IEnumerable<FiscalizacaoRelatorioFiscalizacaoCarrossel>>()))
                .Returns(Task.CompletedTask);

            _repoMock.Setup(r => r.UpdateAsync(It.IsAny<FiscalizacaoRelatorioFiscalizacao>())).Returns(Task.CompletedTask);
            _repoMock.Setup(r => r.CommitAsync()).ReturnsAsync(true);

            var request = new FiscalizacaoRelatorioFiscalizacaoConteudoUpdateRequest
            {
                Slug = existing.Slug,
                Carrocel = new List<FiscalizacaoRelatorioFiscalizacaoCarrosselRequest>
                {
                    new FiscalizacaoRelatorioFiscalizacaoCarrosselRequest
                    {
                        Ativo = true,
                        Ordem = 1,
                        Titulo = "c",
                        ConteudoCarrocel = new List<FiscalizacaoRelatorioFiscalizacaoConteudoCarrosselRequest>()
                    }
                }
            };

            var result = await _service.UpdateAsync(existing.Id, request);

            Assert.True(result);
            _repoMock.Verify(r => r.ReplaceCarrosselAsync(existing.Id, It.IsAny<IEnumerable<FiscalizacaoRelatorioFiscalizacaoCarrossel>>()), Times.Once);
            _repoMock.Verify(r => r.UpdateAsync(It.IsAny<FiscalizacaoRelatorioFiscalizacao>()), Times.Once);
            _repoMock.Verify(r => r.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsFalse_WhenNotFound()
        {
            _repoMock.Setup(r => r.FindByIdAsync(77)).ReturnsAsync((FiscalizacaoRelatorioFiscalizacao?)null);

            var result = await _service.DeleteAsync(77);

            Assert.False(result);
            _repoMock.Verify(r => r.DeleteAsync(It.IsAny<FiscalizacaoRelatorioFiscalizacao>()), Times.Never);
        }

        [Fact]
        public async Task DeleteAsync_Succeeds_ReturnsTrue()
        {
            var entity = CreateEntity(88, "to-delete");
            _repoMock.Setup(r => r.FindByIdAsync(88)).ReturnsAsync(entity);
            _repoMock.Setup(r => r.DeleteAsync(entity)).Returns(Task.CompletedTask);
            _repoMock.Setup(r => r.CommitAsync()).ReturnsAsync(true);

            var result = await _service.DeleteAsync(88);

            Assert.True(result);
            _repoMock.Verify(r => r.DeleteAsync(entity), Times.Once);
            _repoMock.Verify(r => r.CommitAsync(), Times.Once);
        }
    }
}
