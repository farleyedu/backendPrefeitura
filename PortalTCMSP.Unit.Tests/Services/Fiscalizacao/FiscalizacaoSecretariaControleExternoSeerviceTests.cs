using Moq;
using PortalTCMSP.Domain.DTOs.Requests.Fiscalizacao.SecretariaControleExterno;
using PortalTCMSP.Domain.Entities.FiscalizacaoEntity.SecretariaControleExterno;
using PortalTCMSP.Domain.Repositories.Fiscalizacao;
using PortalTCMSP.Infra.Services.Fiscalizacao;

namespace PortalTCMSP.Unit.Tests.Services.Fiscalizacao
{
    public class FiscalizacaoSecretariaControleExternoSeerviceTests
    {
        private readonly Mock<IFiscalizacaoSecretariaControleExternoRepository> _repoMock;
        private readonly FiscalizacaoSecretariaControleExternoSeervice _service;

        public FiscalizacaoSecretariaControleExternoSeerviceTests()
        {
            _repoMock = new Mock<IFiscalizacaoSecretariaControleExternoRepository>();
            _service = new FiscalizacaoSecretariaControleExternoSeervice(_repoMock.Object);
        }

        [Fact]
        public async Task GetAllAsync_CallsRepoAndReturnsMapped()
        {
            var entity = new FiscalizacaoSecretariaControleExterno { Id = 1, Slug = "s", Titulo = "t" };
            _repoMock.Setup(r => r.AllWithChildrenAsync()).ReturnsAsync(new List<FiscalizacaoSecretariaControleExterno> { entity });

            var result = await _service.GetAllAsync();

            Assert.NotNull(result);
            Assert.NotEmpty(result);
            _repoMock.Verify(r => r.AllWithChildrenAsync(), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsMappedWhenFound()
        {
            var entity = new FiscalizacaoSecretariaControleExterno { Id = 5, Slug = "by-id", Titulo = "t" };
            _repoMock.Setup(r => r.GetWithChildrenByIdAsync(5)).ReturnsAsync(entity);

            var resp = await _service.GetByIdAsync(5);

            Assert.NotNull(resp);
            Assert.Equal(5, resp.Id);
            _repoMock.Verify(r => r.GetWithChildrenByIdAsync(5), Times.Once);
        }

        [Fact]
        public async Task GetBySlugAsync_ReturnsMappedWhenFound()
        {
            var entity = new FiscalizacaoSecretariaControleExterno { Id = 6, Slug = "the-slug", Titulo = "t" };
            _repoMock.Setup(r => r.GetWithChildrenBySlugAsync("the-slug")).ReturnsAsync(entity);

            var resp = await _service.GetBySlugAsync("the-slug");

            Assert.NotNull(resp);
            Assert.Equal("the-slug", resp.Slug);
            _repoMock.Verify(r => r.GetWithChildrenBySlugAsync("the-slug"), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_ThrowsWhenSlugExists()
        {
            var req = new FiscalizacaoSecretariaCreateRequest { Slug = "dup", Titulo = "t", Ativo = true };
            _repoMock.Setup(r => r.AnyAsync(It.IsAny<System.Linq.Expressions.Expression<Func<FiscalizacaoSecretariaControleExterno, bool>>>())).ReturnsAsync(true);

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => _service.CreateAsync(req));
            Assert.Contains("slug", ex.Message, StringComparison.OrdinalIgnoreCase);

            _repoMock.Verify(r => r.AnyAsync(It.IsAny<System.Linq.Expressions.Expression<Func<FiscalizacaoSecretariaControleExterno, bool>>>()), Times.Once);
            _repoMock.Verify(r => r.InsertAsync(It.IsAny<FiscalizacaoSecretariaControleExterno>()), Times.Never);
        }

        [Fact]
        public async Task CreateAsync_InsertsEntityAndReturnsId()
        {
            var req = new FiscalizacaoSecretariaCreateRequest { Slug = "new-slug", Titulo = "t", Ativo = true };

            _repoMock.Setup(r => r.AnyAsync(It.IsAny<System.Linq.Expressions.Expression<Func<FiscalizacaoSecretariaControleExterno, bool>>>())).ReturnsAsync(false);

            _repoMock.Setup(r => r.InsertAsync(It.IsAny<FiscalizacaoSecretariaControleExterno>()))
                .Callback<FiscalizacaoSecretariaControleExterno>(e => e.Id = 123)
                .Returns(Task.CompletedTask);

            _repoMock.Setup(r => r.CommitAsync()).ReturnsAsync(true);

            var id = await _service.CreateAsync(req);

            Assert.Equal(123, id);
            _repoMock.Verify(r => r.AnyAsync(It.IsAny<System.Linq.Expressions.Expression<Func<FiscalizacaoSecretariaControleExterno, bool>>>()), Times.Once);
            _repoMock.Verify(r => r.InsertAsync(It.IsAny<FiscalizacaoSecretariaControleExterno>()), Times.Once);
            _repoMock.Verify(r => r.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsFalseWhenNotFound()
        {
            _repoMock.Setup(r => r.GetWithChildrenByIdAsync(10)).ReturnsAsync((FiscalizacaoSecretariaControleExterno?)null);

            var res = await _service.UpdateAsync(10, new FiscalizacaoSecretariaUpdateRequest { Slug = "x" });

            Assert.False(res);
            _repoMock.Verify(r => r.GetWithChildrenByIdAsync(10), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ThrowsWhenSlugConflicts()
        {
            var existing = new FiscalizacaoSecretariaControleExterno { Id = 20, Slug = "old" };
            _repoMock.Setup(r => r.GetWithChildrenByIdAsync(20)).ReturnsAsync(existing);

            _repoMock.Setup(r => r.AnyAsync(It.IsAny<System.Linq.Expressions.Expression<Func<FiscalizacaoSecretariaControleExterno, bool>>>())).ReturnsAsync(true);

            var request = new FiscalizacaoSecretariaUpdateRequest { Slug = "new-slug", Titulos = null, Carrossel = null };

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => _service.UpdateAsync(20, request));
            Assert.Contains("slug", ex.Message, StringComparison.OrdinalIgnoreCase);

            _repoMock.Verify(r => r.AnyAsync(It.IsAny<System.Linq.Expressions.Expression<Func<FiscalizacaoSecretariaControleExterno, bool>>>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ReplacesCollectionsAndCommits()
        {
            var existing = new FiscalizacaoSecretariaControleExterno
            {
                Id = 30,
                Slug = "same-slug",
                Titulos = new List<FiscalizacaoSecretariaSecaoConteudoTitulo>(),
                Carrossel = new List<FiscalizacaoSecretariaSecaoConteudoCarrosselItem>()
            };

            _repoMock.Setup(r => r.GetWithChildrenByIdAsync(30)).ReturnsAsync(existing);
            _repoMock.Setup(r => r.AnyAsync(It.IsAny<System.Linq.Expressions.Expression<Func<FiscalizacaoSecretariaControleExterno, bool>>>())).ReturnsAsync(true);

            IEnumerable<FiscalizacaoSecretariaSecaoConteudoTitulo>? replacedTitulos = null;
            _repoMock.Setup(r => r.ReplaceTitulosAsync(30, It.IsAny<IEnumerable<FiscalizacaoSecretariaSecaoConteudoTitulo>>()))
                .Callback<long, IEnumerable<FiscalizacaoSecretariaSecaoConteudoTitulo>>((id, t) => replacedTitulos = t)
                .Returns(Task.CompletedTask);

            IEnumerable<FiscalizacaoSecretariaSecaoConteudoCarrosselItem>? replacedCarrossel = null;
            _repoMock.Setup(r => r.ReplaceCarrosselAsync(30, It.IsAny<IEnumerable<FiscalizacaoSecretariaSecaoConteudoCarrosselItem>>()))
                .Callback<long, IEnumerable<FiscalizacaoSecretariaSecaoConteudoCarrosselItem>>((id, c) => replacedCarrossel = c)
                .Returns(Task.CompletedTask);

            _repoMock.Setup(r => r.UpdateAsync(It.IsAny<FiscalizacaoSecretariaControleExterno>())).Returns(Task.CompletedTask);
            _repoMock.Setup(r => r.CommitAsync()).ReturnsAsync(true);

            var request = new FiscalizacaoSecretariaUpdateRequest
            {
                Slug = "same-slug",
                Titulos = new List<FiscalizacaoSecretariaTituloItemRequest>
                {
                    new FiscalizacaoSecretariaTituloItemRequest { Ordem = 1, Titulo = " T1 ", ImagemUrl = " img ", Descricao = " d " }
                },
                Carrossel = new List<FiscalizacaoSecretariaCarrosselItemRequest>
                {
                    new FiscalizacaoSecretariaCarrosselItemRequest { Ordem = 2, Titulo = " C1 ", ImagemUrl = " imgc ", Descricao = " cd " }
                }
            };

            var result = await _service.UpdateAsync(30, request);

            Assert.True(result);
            _repoMock.Verify(r => r.ReplaceTitulosAsync(30, It.IsAny<IEnumerable<FiscalizacaoSecretariaSecaoConteudoTitulo>>()), Times.Once);
            _repoMock.Verify(r => r.ReplaceCarrosselAsync(30, It.IsAny<IEnumerable<FiscalizacaoSecretariaSecaoConteudoCarrosselItem>>()), Times.Once);
            _repoMock.Verify(r => r.UpdateAsync(It.IsAny<FiscalizacaoSecretariaControleExterno>()), Times.Once);
            _repoMock.Verify(r => r.CommitAsync(), Times.Once);

            Assert.NotNull(replacedTitulos);
            var titulo = replacedTitulos!.Single();
            Assert.Equal(1, titulo.Ordem);
            Assert.Equal("T1", titulo.Titulo);
            Assert.Equal("d", titulo.Descricao);

            Assert.NotNull(replacedCarrossel);
            var car = replacedCarrossel!.Single();
            Assert.Equal(2, car.Ordem);
            Assert.Equal("C1", car.Titulo);
            Assert.Equal("imgc", car.ImagemUrl);
            Assert.Equal("cd", car.Descricao);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsFalseWhenNotFound()
        {
            _repoMock.Setup(r => r.FindByIdAsync(99)).ReturnsAsync((FiscalizacaoSecretariaControleExterno?)null);

            var res = await _service.DeleteAsync(99);

            Assert.False(res);
            _repoMock.Verify(r => r.FindByIdAsync(99), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_DeletesAndCommitsWhenFound()
        {
            var entity = new FiscalizacaoSecretariaControleExterno { Id = 100, Slug = "to-delete" };
            _repoMock.Setup(r => r.FindByIdAsync(100)).ReturnsAsync(entity);
            _repoMock.Setup(r => r.DeleteAsync(entity)).Returns(Task.CompletedTask);
            _repoMock.Setup(r => r.CommitAsync()).ReturnsAsync(true);

            var res = await _service.DeleteAsync(100);

            Assert.True(res);
            _repoMock.Verify(r => r.DeleteAsync(entity), Times.Once);
            _repoMock.Verify(r => r.CommitAsync(), Times.Once);
        }
    }
}
