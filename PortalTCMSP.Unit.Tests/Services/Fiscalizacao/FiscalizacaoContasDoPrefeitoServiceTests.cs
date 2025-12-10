using Moq;
using PortalTCMSP.Domain.DTOs.Requests.Fiscalizacao.ContasDoPrefeito;
using PortalTCMSP.Domain.Entities.FiscalizacaoEntity.ContasDoPrefeito;
using PortalTCMSP.Domain.Repositories.Fiscalizacao;
using PortalTCMSP.Infra.Services.Fiscalizacao;
using System.Linq.Expressions;

namespace PortalTCMSP.Unit.Tests.Services.Fiscalizacao
{
    public class FiscalizacaoContasDoPrefeitoServiceTests
    {
        private readonly Mock<IFiscalizacaoContasDoPrefeitoRepository> _repoMock;
        private readonly FiscalizacaoContasDoPrefeitoService _service;

        public FiscalizacaoContasDoPrefeitoServiceTests()
        {
            _repoMock = new Mock<IFiscalizacaoContasDoPrefeitoRepository>();
            _service = new FiscalizacaoContasDoPrefeitoService(_repoMock.Object);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsMappedWhenFound()
        {
            var entity = new FiscalizacaoContasDoPrefeito
            {
                Id = 10,
                Ano = "2025",
                Pauta = "Pauta X",
                DataCriacao = DateTime.UtcNow,
                Anexos = new List<FiscalizacaoContasDoPrefeitoAnexo>
                {
                    new FiscalizacaoContasDoPrefeitoAnexo { Id = 1, Link = "http://a", Ordem = 1 }
                }
            };

            _repoMock.Setup(r => r.GetWithAnexosByIdAsync(10)).ReturnsAsync(entity);

            var resp = await _service.GetByIdAsync(10);

            Assert.NotNull(resp);
            Assert.Equal(10, resp!.Id);
            Assert.Equal(entity.Ano, resp.Ano);
            _repoMock.Verify(r => r.GetWithAnexosByIdAsync(10), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNullWhenNotFound()
        {
            _repoMock.Setup(r => r.GetWithAnexosByIdAsync(It.IsAny<long>())).ReturnsAsync((FiscalizacaoContasDoPrefeito?)null);

            var resp = await _service.GetByIdAsync(999);

            Assert.Null(resp);
            _repoMock.Verify(r => r.GetWithAnexosByIdAsync(999), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_ThrowsWhenDuplicateExists()
        {
            _repoMock.Setup(r => r.AnyAsync(It.IsAny<Expression<Func<FiscalizacaoContasDoPrefeito, bool>>>()))
                     .ReturnsAsync(true);

            var req = new FiscalizacaoContasDoPrefeitoCreateRequest
            {
                Ano = "2025",
                Pauta = "P"
            };

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => _service.CreateAsync(req));
            Assert.Contains("Já existe um registro", ex.Message);
            _repoMock.Verify(r => r.AnyAsync(It.IsAny<Expression<Func<FiscalizacaoContasDoPrefeito, bool>>>()), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_InsertsEntityAndReturnsId()
        {
            _repoMock.Setup(r => r.AnyAsync(It.IsAny<Expression<Func<FiscalizacaoContasDoPrefeito, bool>>>()))
                     .ReturnsAsync(false);

            _repoMock.Setup(r => r.InsertAsync(It.IsAny<FiscalizacaoContasDoPrefeito>()))
                     .Callback<FiscalizacaoContasDoPrefeito>(e => e.Id = 42)
                     .Returns(Task.CompletedTask);

            _repoMock.Setup(r => r.CommitAsync()).ReturnsAsync(true);

            var req = new FiscalizacaoContasDoPrefeitoCreateRequest
            {
                Ano = " 2025 ",
                Pauta = " Pauta ",
                Anexos = new List<FiscalizacaoContasDoPrefeitoAnexoItemRequest>
                {
                    new FiscalizacaoContasDoPrefeitoAnexoItemRequest { Link = " http://x ", Ordem = 1 }
                }
            };

            var id = await _service.CreateAsync(req);

            Assert.Equal(42, id);
            _repoMock.Verify(r => r.InsertAsync(It.IsAny<FiscalizacaoContasDoPrefeito>()), Times.Once);
            _repoMock.Verify(r => r.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsFalseWhenEntityNotFound()
        {
            _repoMock.Setup(r => r.GetWithAnexosByIdAsync(It.IsAny<long>())).ReturnsAsync((FiscalizacaoContasDoPrefeito?)null);

            var req = new FiscalizacaoContasDoPrefeitoUpdateRequest
            {
                Ano = "2025",
                Pauta = "P"
            };

            var result = await _service.UpdateAsync(1, req);

            Assert.False(result);
            _repoMock.Verify(r => r.GetWithAnexosByIdAsync(1), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ThrowsWhenDuplicateExists()
        {
            var existing = new FiscalizacaoContasDoPrefeito { Id = 5, Ano = "2024", Pauta = "X", Anexos = new List<FiscalizacaoContasDoPrefeitoAnexo>() };
            _repoMock.Setup(r => r.GetWithAnexosByIdAsync(5)).ReturnsAsync(existing);

            _repoMock.Setup(r => r.AnyAsync(It.IsAny<Expression<Func<FiscalizacaoContasDoPrefeito, bool>>>()))
                     .ReturnsAsync(true);

            var req = new FiscalizacaoContasDoPrefeitoUpdateRequest
            {
                Ano = "2025",
                Pauta = "P"
            };

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => _service.UpdateAsync(5, req));
            Assert.Contains("Já existe um registro", ex.Message);
            _repoMock.Verify(r => r.GetWithAnexosByIdAsync(5), Times.Once);
            _repoMock.Verify(r => r.AnyAsync(It.IsAny<Expression<Func<FiscalizacaoContasDoPrefeito, bool>>>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ReplacesAnexosUpdatesAndCommits()
        {
            var existing = new FiscalizacaoContasDoPrefeito
            {
                Id = 7,
                Ano = "2024",
                Pauta = "Old",
                Anexos = new List<FiscalizacaoContasDoPrefeitoAnexo>()
            };

            _repoMock.Setup(r => r.GetWithAnexosByIdAsync(7)).ReturnsAsync(existing);
            _repoMock.Setup(r => r.AnyAsync(It.IsAny<Expression<Func<FiscalizacaoContasDoPrefeito, bool>>>()))
                     .ReturnsAsync(false);

            IEnumerable<FiscalizacaoContasDoPrefeitoAnexo>? captured = null;
            _repoMock.Setup(r => r.ReplaceAnexosAsync(existing.Id, It.IsAny<IEnumerable<FiscalizacaoContasDoPrefeitoAnexo>>()))
                     .Callback<long, IEnumerable<FiscalizacaoContasDoPrefeitoAnexo>>((id, novos) => captured = novos)
                     .Returns(Task.CompletedTask);

            _repoMock.Setup(r => r.UpdateAsync(It.IsAny<FiscalizacaoContasDoPrefeito>())).Returns(Task.CompletedTask);
            _repoMock.Setup(r => r.CommitAsync()).ReturnsAsync(true);

            var req = new FiscalizacaoContasDoPrefeitoUpdateRequest
            {
                Ano = " 2025 ",
                Pauta = " Pauta Y ",
                Anexos = new List<FiscalizacaoContasDoPrefeitoAnexoItemRequest>
                {
                    new FiscalizacaoContasDoPrefeitoAnexoItemRequest { Link = " http://link ", TipoArquivo = " pdf ", NomeExibicao = " name ", Ordem = 2 }
                }
            };

            var result = await _service.UpdateAsync(7, req);

            Assert.True(result);
            _repoMock.Verify(r => r.GetWithAnexosByIdAsync(7), Times.Once);
            _repoMock.Verify(r => r.ReplaceAnexosAsync(existing.Id, It.IsAny<IEnumerable<FiscalizacaoContasDoPrefeitoAnexo>>()), Times.Once);
            _repoMock.Verify(r => r.UpdateAsync(It.IsAny<FiscalizacaoContasDoPrefeito>()), Times.Once);
            _repoMock.Verify(r => r.CommitAsync(), Times.Once);

            Assert.NotNull(captured);
            var list = captured!.ToList();
            Assert.Single(list);
            Assert.Equal("http://link", list[0].Link); // trimmed
            Assert.Equal("pdf", list[0].TipoArquivo?.Trim());
            Assert.Equal("name", list[0].NomeExibicao?.Trim());
            Assert.Equal(2, list[0].Ordem);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsFalseWhenNotFound()
        {
            _repoMock.Setup(r => r.FindByIdAsync(It.IsAny<long>())).ReturnsAsync((FiscalizacaoContasDoPrefeito?)null);

            var result = await _service.DeleteAsync(99);

            Assert.False(result);
            _repoMock.Verify(r => r.FindByIdAsync(99), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_DeletesEntityAndCommits()
        {
            var entity = new FiscalizacaoContasDoPrefeito { Id = 12 };
            _repoMock.Setup(r => r.FindByIdAsync(12)).ReturnsAsync(entity);
            _repoMock.Setup(r => r.DeleteAsync(entity)).Returns(Task.CompletedTask);
            _repoMock.Setup(r => r.CommitAsync()).ReturnsAsync(true);

            var result = await _service.DeleteAsync(12);

            Assert.True(result);
            _repoMock.Verify(r => r.FindByIdAsync(12), Times.Once);
            _repoMock.Verify(r => r.DeleteAsync(entity), Times.Once);
            _repoMock.Verify(r => r.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task GetListAsync_ReturnsNullWhenNoItems()
        {
            var req = new FiscalizacaoContasDoPrefeitoSearchRequest { Page = 1, Count = 10 };
            _repoMock.Setup(r => r.Search(req)).Returns(new List<FiscalizacaoContasDoPrefeito>().AsQueryable());

            var result = await _service.GetListAsync(req);

            Assert.Null(result);
            _repoMock.Verify(r => r.Search(req), Times.Once);
        }

        [Fact]
        public async Task GetListAsync_ReturnsPagedResultWhenItemsExist()
        {
            var items = new List<FiscalizacaoContasDoPrefeito>
            {
                new FiscalizacaoContasDoPrefeito { Id = 1, Ano = "2023", Pauta = "A", DataCriacao = DateTime.UtcNow, Anexos = new List<FiscalizacaoContasDoPrefeitoAnexo>() },
                new FiscalizacaoContasDoPrefeito { Id = 2, Ano = "2024", Pauta = "B", DataCriacao = DateTime.UtcNow, Anexos = new List<FiscalizacaoContasDoPrefeitoAnexo>() },
                new FiscalizacaoContasDoPrefeito { Id = 3, Ano = "2025", Pauta = "C", DataCriacao = DateTime.UtcNow, Anexos = new List<FiscalizacaoContasDoPrefeitoAnexo>() }
            }.AsQueryable();

            var req = new FiscalizacaoContasDoPrefeitoSearchRequest { Page = 1, Count = 10 };
            _repoMock.Setup(r => r.Search(req)).Returns(items);

            var result = await _service.GetListAsync(req);

            Assert.NotNull(result);
            Assert.Equal(3, result!.Total);
            Assert.Equal(3, result.List.Count());
            _repoMock.Verify(r => r.Search(req), Times.Once);
        }
    }
}