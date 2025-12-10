using Moq;
using PortalTCMSP.Domain.DTOs.Requests.Fiscalizacao.PlanoAnualFiscalizacao;
using PortalTCMSP.Domain.Entities.FiscalizacaoEntity.PlanoAnualFiscalizacao;
using PortalTCMSP.Domain.Repositories.Fiscalizacao;
using PortalTCMSP.Infra.Services.Fiscalizacao;
using System.Linq.Expressions;

namespace PortalTCMSP.Unit.Tests.Services.Fiscalizacao
{
    public class FiscalizacaoPlanoAnualFiscalizacaoServiceTests
    {
        private FiscalizacaoPlanoAnualFiscalizacaoService CreateService(Mock<IFiscalizacaoPlanoAnualFiscalizacaoRepository> repoMock)
            => new FiscalizacaoPlanoAnualFiscalizacaoService(repoMock.Object);

        [Fact]
        public async Task GetAllAsync_ReturnsEmpty_WhenRepositoryEmpty()
        {
            var repoMock = new Mock<IFiscalizacaoPlanoAnualFiscalizacaoRepository>();
            repoMock.Setup(r => r.AllWithChildrenAsync()).ReturnsAsync(new List<FiscalizacaoPlanoAnualFiscalizacaoResolucao>());

            var svc = CreateService(repoMock);
            var result = await svc.GetAllAsync();

            repoMock.Verify(r => r.AllWithChildrenAsync(), Times.Once);
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsResponse_WhenFound()
        {
            var entity = new FiscalizacaoPlanoAnualFiscalizacaoResolucao { Id = 1, Slug = "s" };
            var repoMock = new Mock<IFiscalizacaoPlanoAnualFiscalizacaoRepository>();
            repoMock.Setup(r => r.GetWithChildrenByIdAsync(1)).ReturnsAsync(entity);

            var svc = CreateService(repoMock);
            var resp = await svc.GetByIdAsync(1);

            repoMock.Verify(r => r.GetWithChildrenByIdAsync(1), Times.Once);
            Assert.NotNull(resp);
        }

        [Fact]
        public async Task GetBySlugAsync_ReturnsResponse_WhenFound()
        {
            var entity = new FiscalizacaoPlanoAnualFiscalizacaoResolucao { Id = 2, Slug = "my-slug" };
            var repoMock = new Mock<IFiscalizacaoPlanoAnualFiscalizacaoRepository>();
            repoMock.Setup(r => r.GetWithChildrenBySlugAsync("my-slug")).ReturnsAsync(entity);

            var svc = CreateService(repoMock);
            var resp = await svc.GetBySlugAsync("my-slug");

            repoMock.Verify(r => r.GetWithChildrenBySlugAsync("my-slug"), Times.Once);
            Assert.NotNull(resp);
        }

        [Fact]
        public async Task CreateAsync_Throws_WhenSlugAlreadyExists()
        {
            var repoMock = new Mock<IFiscalizacaoPlanoAnualFiscalizacaoRepository>();
            repoMock.Setup(r => r.AnyAsync(It.IsAny<Expression<Func<FiscalizacaoPlanoAnualFiscalizacaoResolucao, bool>>>()))
                    .ReturnsAsync(true);

            var svc = CreateService(repoMock);
            var request = new FiscalizacaoPlanoAnualFiscalizacaoResolucaoCreateRequest { Slug = "exists" };

            await Assert.ThrowsAsync<InvalidOperationException>(() => svc.CreateAsync(request));
            repoMock.Verify(r => r.AnyAsync(It.IsAny<Expression<Func<FiscalizacaoPlanoAnualFiscalizacaoResolucao, bool>>>()), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_InsertsEntityAndReturnsId_WhenValid()
        {
            var repoMock = new Mock<IFiscalizacaoPlanoAnualFiscalizacaoRepository>();
            repoMock.Setup(r => r.AnyAsync(It.IsAny<Expression<Func<FiscalizacaoPlanoAnualFiscalizacaoResolucao, bool>>>()))
                    .ReturnsAsync(false);

            repoMock.Setup(r => r.InsertAsync(It.IsAny<FiscalizacaoPlanoAnualFiscalizacaoResolucao>()))
                .Callback<FiscalizacaoPlanoAnualFiscalizacaoResolucao>(e => e.Id = 42)
                .Returns(Task.CompletedTask);

            repoMock.Setup(r => r.CommitAsync()).ReturnsAsync(true);

            var svc = CreateService(repoMock);
            var request = new FiscalizacaoPlanoAnualFiscalizacaoResolucaoCreateRequest
            {
                Slug = "unique",
                Titulo = "t",
                Numero = 1,
                Ano = 2025,
                DataPublicacao = DateTime.UtcNow,
                Ativo = true
            };

            var id = await svc.CreateAsync(request);

            Assert.Equal(42, id);
            repoMock.Verify(r => r.InsertAsync(It.IsAny<FiscalizacaoPlanoAnualFiscalizacaoResolucao>()), Times.Once);
            repoMock.Verify(r => r.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsFalse_WhenEntityNotFound()
        {
            var repoMock = new Mock<IFiscalizacaoPlanoAnualFiscalizacaoRepository>();
            repoMock.Setup(r => r.GetWithChildrenByIdAsync(10)).ReturnsAsync((FiscalizacaoPlanoAnualFiscalizacaoResolucao?)null);

            var svc = CreateService(repoMock);
            var request = new FiscalizacaoPlanoAnualFiscalizacaoResolucaoUpdateRequest { Slug = "s" };

            var result = await svc.UpdateAsync(10, request);

            Assert.False(result);
            repoMock.Verify(r => r.GetWithChildrenByIdAsync(10), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_Throws_WhenSlugConflict()
        {
            var existing = new FiscalizacaoPlanoAnualFiscalizacaoResolucao { Id = 5, Slug = "old" };
            var repoMock = new Mock<IFiscalizacaoPlanoAnualFiscalizacaoRepository>();
            repoMock.Setup(r => r.GetWithChildrenByIdAsync(5)).ReturnsAsync(existing);
            repoMock.Setup(r => r.AnyAsync(It.IsAny<Expression<Func<FiscalizacaoPlanoAnualFiscalizacaoResolucao, bool>>>()))
                    .ReturnsAsync(true);

            var svc = CreateService(repoMock);
            var request = new FiscalizacaoPlanoAnualFiscalizacaoResolucaoUpdateRequest { Slug = "new-slug", Titulo = "t", Numero = 1, Ano = 2025, DataPublicacao = DateTime.UtcNow};

            await Assert.ThrowsAsync<InvalidOperationException>(() => svc.UpdateAsync(5, request));
            repoMock.Verify(r => r.AnyAsync(It.IsAny<Expression<Func<FiscalizacaoPlanoAnualFiscalizacaoResolucao, bool>>>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ReplacesChildrenAndCommits_WhenValid()
        {
            var existing = new FiscalizacaoPlanoAnualFiscalizacaoResolucao { Id = 7, Slug = "same-slug" };
            var repoMock = new Mock<IFiscalizacaoPlanoAnualFiscalizacaoRepository>();
            repoMock.Setup(r => r.GetWithChildrenByIdAsync(7)).ReturnsAsync(existing);

            repoMock.Setup(r => r.AnyAsync(It.IsAny<Expression<Func<FiscalizacaoPlanoAnualFiscalizacaoResolucao, bool>>>()))
                    .ReturnsAsync(false);

            repoMock.Setup(r => r.ReplaceDispositivosAsync(It.IsAny<long>(), It.IsAny<IEnumerable<FiscalizacaoResolucaoDispositivo>>()))
                    .Returns(Task.CompletedTask)
                    .Verifiable();

            repoMock.Setup(r => r.ReplaceAnexosAsync(It.IsAny<long>(), It.IsAny<IEnumerable<FiscalizacaoResolucaoAnexo>>()))
                    .Returns(Task.CompletedTask)
                    .Verifiable();

            repoMock.Setup(r => r.ReplaceAtasAsync(It.IsAny<long>(), It.IsAny<IEnumerable<FiscalizacaoResolucaoAta>>()))
                    .Returns(Task.CompletedTask)
                    .Verifiable();

            repoMock.Setup(r => r.ReplaceEmentaAsync(It.IsAny<long>(), It.IsAny<FiscalizacaoResolucaoEmenta?>()))
                    .Returns(Task.CompletedTask)
                    .Verifiable();

            repoMock.Setup(r => r.UpdateAsync(It.IsAny<FiscalizacaoPlanoAnualFiscalizacaoResolucao>()))
                    .Returns(Task.CompletedTask)
                    .Verifiable();

            repoMock.Setup(r => r.CommitAsync()).ReturnsAsync(true);

            var svc = CreateService(repoMock);
            var request = new FiscalizacaoPlanoAnualFiscalizacaoResolucaoUpdateRequest
            {
                Slug = "same-slug", 
                Titulo = "t",
                Numero = 1,
                Ano = 2025,
                DataPublicacao = DateTime.UtcNow,
                Dispositivos = new List<FiscalizacaoResolucaoDispositivoRequest>
                {
                    new FiscalizacaoResolucaoDispositivoRequest { Ordem = 1, Artigo = " art " }
                },
                Anexos = new List<FiscalizacaoResolucaoAnexoRequest>
                {
                    new FiscalizacaoResolucaoAnexoRequest { Numero = 1, Titulo = " anexo ", Descritivo = " d ", Atividades = new List<FiscalizacaoResolucaoAtividadeRequest>() }
                },
                Atas = new List<FiscalizacaoResolucaoAtaRequest>
                {
                    new FiscalizacaoResolucaoAtaRequest { Ordem = 1, TituloAta = "ata", ConteudoAta = "conteudo" }
                },
                Ementa = new FiscalizacaoResolucaoEmentaRequest
                {
                    Descritivo = "desc",
                    LinksArtigos = new List<string> { " l1 " }
                }
            };

            var result = await svc.UpdateAsync(7, request);

            Assert.True(result);
            repoMock.Verify(r => r.ReplaceDispositivosAsync(7, It.Is<IEnumerable<FiscalizacaoResolucaoDispositivo>>(c => c.Count() == 1)), Times.Once);
            repoMock.Verify(r => r.ReplaceAnexosAsync(7, It.Is<IEnumerable<FiscalizacaoResolucaoAnexo>>(c => c.Count() == 1)), Times.Once);
            repoMock.Verify(r => r.ReplaceAtasAsync(7, It.Is<IEnumerable<FiscalizacaoResolucaoAta>>(c => c.Count() == 1)), Times.Once);
            repoMock.Verify(r => r.ReplaceEmentaAsync(7, It.IsAny<FiscalizacaoResolucaoEmenta?>()), Times.Once);
            repoMock.Verify(r => r.UpdateAsync(existing), Times.Once);
            repoMock.Verify(r => r.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsFalse_WhenNotFound()
        {
            var repoMock = new Mock<IFiscalizacaoPlanoAnualFiscalizacaoRepository>();
            repoMock.Setup(r => r.FindByIdAsync(99)).ReturnsAsync((FiscalizacaoPlanoAnualFiscalizacaoResolucao?)null);

            var svc = CreateService(repoMock);
            var res = await svc.DeleteAsync(99);

            Assert.False(res);
            repoMock.Verify(r => r.FindByIdAsync(99), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_DeletesAndCommits_WhenFound()
        {
            var entity = new FiscalizacaoPlanoAnualFiscalizacaoResolucao { Id = 55 };
            var repoMock = new Mock<IFiscalizacaoPlanoAnualFiscalizacaoRepository>();
            repoMock.Setup(r => r.FindByIdAsync(55)).ReturnsAsync(entity);
            repoMock.Setup(r => r.DeleteAsync(entity)).Returns(Task.CompletedTask);
            repoMock.Setup(r => r.CommitAsync()).ReturnsAsync(true);

            var svc = CreateService(repoMock);
            var res = await svc.DeleteAsync(55);

            Assert.True(res);
            repoMock.Verify(r => r.DeleteAsync(entity), Times.Once);
            repoMock.Verify(r => r.CommitAsync(), Times.Once);
        }
    }
}
