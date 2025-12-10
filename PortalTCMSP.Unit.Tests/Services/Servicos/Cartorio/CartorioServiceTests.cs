using Moq;
using PortalTCMSP.Domain.DTOs.Requests.Servicos.Cartorio;
using PortalTCMSP.Domain.Entities.ServicosEntity.CartorioEntity;
using PortalTCMSP.Domain.Repositories.Servicos.Cartorio;
using PortalTCMSP.Infra.Services.Servicos.Cartorio;

namespace PortalTCMSP.Unit.Tests.Services.Servicos.Cartorio
{
    public class CartorioServiceTests
    {
        private readonly Mock<ICartorioRepository> _repoMock = new();
        private readonly Mock<ICartorioAtendimentoRepository> _repoAtendimentoMock = new();

        private CartorioService CreateService()
            => new CartorioService(_repoMock.Object, _repoAtendimentoMock.Object);

        [Fact]
        public async Task GetAllAsync_ReturnsMappedResponses()
        {
            var entities = new List<Domain.Entities.ServicosEntity.CartorioEntity.Cartorio>
            {
                new Domain.Entities.ServicosEntity.CartorioEntity.Cartorio { Id = 1, TituloPagina = "T1", Slug = "s1", Ativo = true, DataCriacao = DateTime.UtcNow }
            };
            _repoMock.Setup(r => r.AllWithChildrenAsync()).ReturnsAsync(entities);

            var svc = CreateService();
            var result = await svc.GetAllAsync();

            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(1, result.First().Id);
            _repoMock.Verify(r => r.AllWithChildrenAsync(), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsResponse_WhenFound()
        {
            var entity = new Domain.Entities.ServicosEntity.CartorioEntity.Cartorio { Id = 5, TituloPagina = "X", Slug = "x", Ativo = true, DataCriacao = DateTime.UtcNow };
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
            _repoMock.Setup(r => r.GetWithChildrenByIdAsync(10)).ReturnsAsync((Domain.Entities.ServicosEntity.CartorioEntity.Cartorio?)null);

            var svc = CreateService();
            var response = await svc.GetByIdAsync(10);

            Assert.Null(response);
        }

        [Fact]
        public async Task DisableAsync_DelegatesToRepository()
        {
            _repoMock.Setup(r => r.DisableAsync(3)).ReturnsAsync(true);

            var svc = CreateService();
            var result = await svc.DisableAsync(3);

            Assert.True(result);
            _repoMock.Verify(r => r.DisableAsync(3), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_InsertsNewAndReturnsId_WhenNoExistingSlug()
        {
            var request = new CartorioCreateRequest { TituloPagina = "Title", Slug = "my-slug", Ativo = true };
            _repoMock.Setup(r => r.GetBySlugAtivoAsync(It.IsAny<string>())).ReturnsAsync((Domain.Entities.ServicosEntity.CartorioEntity.Cartorio?)null);
            _repoMock.Setup(r => r.InsertAsync(It.IsAny<Domain.Entities.ServicosEntity.CartorioEntity.Cartorio>()))
                     .Callback<Domain.Entities.ServicosEntity.CartorioEntity.Cartorio>(e => e.Id = 42)
                     .Returns(Task.CompletedTask);
            _repoMock.Setup(r => r.CommitAsync()).ReturnsAsync(true);

            var svc = CreateService();
            var id = await svc.CreateAsync(request);

            Assert.Equal(42, id);
            _repoMock.Verify(r => r.GetBySlugAtivoAsync(It.IsAny<string>()), Times.Once);
            _repoMock.Verify(r => r.InsertAsync(It.IsAny<Domain.Entities.ServicosEntity.CartorioEntity.Cartorio>()), Times.Once);
            _repoMock.Verify(r => r.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_DisablesExistingBeforeInsert_WhenExistingSlugFound()
        {
            var existing = new Domain.Entities.ServicosEntity.CartorioEntity.Cartorio { Id = 99, TituloPagina = "Old", Slug = "my-slug", Ativo = true };
            var request = new CartorioCreateRequest { TituloPagina = "New", Slug = "my-slug", Ativo = true };

            _repoMock.Setup(r => r.GetBySlugAtivoAsync(It.IsAny<string>())).ReturnsAsync(existing);
            _repoMock.Setup(r => r.DisableAsync(existing.Id)).ReturnsAsync(true);
            _repoMock.Setup(r => r.InsertAsync(It.IsAny<Domain.Entities.ServicosEntity.CartorioEntity.Cartorio>()))
                     .Callback<Domain.Entities.ServicosEntity.CartorioEntity.Cartorio>(e => e.Id = 100)
                     .Returns(Task.CompletedTask);
            _repoMock.Setup(r => r.CommitAsync()).ReturnsAsync(true);

            var svc = CreateService();
            var id = await svc.CreateAsync(request);

            Assert.Equal(100, id);
            _repoMock.Verify(r => r.DisableAsync(99), Times.Once);
            _repoMock.Verify(r => r.InsertAsync(It.IsAny<Domain.Entities.ServicosEntity.CartorioEntity.Cartorio>()), Times.Once);
        }

        [Fact]
        public async Task CreateAtendimentosAsync_MapsAndCommits()
        {
            var reqs = new List<CartorioAtendimentoRequest>
            {
                new CartorioAtendimentoRequest { Ordem = 1, Titulo = " T1 ", Descricao = " D1 " }
            };

            _repoAtendimentoMock.Setup(r => r.CreateAtendimentosAsync(5, It.IsAny<IEnumerable<CartorioAtendimento>>()))
                                 .Returns(Task.CompletedTask)
                                 .Verifiable();
            _repoAtendimentoMock.Setup(r => r.CommitAsync()).ReturnsAsync(true);

            var svc = CreateService();
            var result = await svc.CreateAtendimentosAsync(5, reqs);

            Assert.True(result);
            _repoAtendimentoMock.Verify(r => r.CreateAtendimentosAsync(5, It.Is<IEnumerable<CartorioAtendimento>>(c =>
                c.First().Titulo == "T1" &&
                c.First().Descricao == "D1" &&
                c.First().IdCartorio == 5
            )), Times.Once);
            _repoAtendimentoMock.Verify(r => r.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsFalse_WhenNotFound()
        {
            _repoMock.Setup(r => r.GetWithChildrenByIdAsync(20)).ReturnsAsync((Domain.Entities.ServicosEntity.CartorioEntity.Cartorio?)null);

            var svc = CreateService();
            var result = await svc.UpdateAsync(20, new CartorioUpdateRequest());

            Assert.False(result);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesAndCommits_WhenFound()
        {
            var existing = new Domain.Entities.ServicosEntity.CartorioEntity.Cartorio { Id = 30, TituloPagina = "Old", Slug = "old", Atendimentos = new List<CartorioAtendimento>() };
            _repoMock.Setup(r => r.GetWithChildrenByIdAsync(30)).ReturnsAsync(existing);
            _repoMock.Setup(r => r.ReplaceAtendimentosAsync(30, It.IsAny<IEnumerable<CartorioAtendimento>>())).Returns(Task.CompletedTask);
            _repoMock.Setup(r => r.UpdateAsync(existing)).Returns(Task.CompletedTask);
            _repoMock.Setup(r => r.CommitAsync()).ReturnsAsync(true);

            var req = new CartorioUpdateRequest
            {
                TituloPagina = "New",
                Slug = "new",
                Ativo = true,
                Atendimentos = new List<CartorioAtendimentoRequest>
                {
                    new CartorioAtendimentoRequest { Ordem = 1, Titulo = " t ", Descricao = " d " }
                }
            };

            var svc = CreateService();
            var result = await svc.UpdateAsync(30, req);

            Assert.True(result);
            _repoMock.Verify(r => r.ReplaceAtendimentosAsync(30, It.IsAny<IEnumerable<CartorioAtendimento>>()), Times.Once);
            _repoMock.Verify(r => r.UpdateAsync(existing), Times.Once);
            _repoMock.Verify(r => r.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateAtendimentosAsync_Throws_WhenNotFound()
        {
            _repoMock.Setup(r => r.GetWithChildrenByIdAsync(40)).ReturnsAsync((Domain.Entities.ServicosEntity.CartorioEntity.Cartorio?)null);

            var svc = CreateService();
            await Assert.ThrowsAsync<InvalidOperationException>(() => svc.UpdateAtendimentosAsync(40, new List<CartorioAtendimentoUpdate>()));
        }

        [Fact]
        public async Task UpdateAtendimentosAsync_UpdatesAndCommits_WhenFound()
        {
            var old = new CartorioAtendimento { Id = 600, Ordem = 1, Titulo = "old", Descricao = "d" };
            var parent = new Domain.Entities.ServicosEntity.CartorioEntity.Cartorio { Id = 50, Atendimentos = new List<CartorioAtendimento> { old } };
            _repoMock.Setup(r => r.GetWithChildrenByIdAsync(50)).ReturnsAsync(parent);
            _repoAtendimentoMock.Setup(r => r.UpdateAtendimentosAsync(50, It.IsAny<IEnumerable<CartorioAtendimento>>())).Returns(Task.CompletedTask);
            _repoAtendimentoMock.Setup(r => r.CommitAsync()).ReturnsAsync(true);

            var novas = new List<CartorioAtendimentoUpdate>
            {
                new CartorioAtendimentoUpdate { Id = 600, Ordem = 2, Titulo = " new ", Descricao = " newd " }
            };

            var svc = CreateService();
            var result = await svc.UpdateAtendimentosAsync(50, novas);

            Assert.True(result);
            _repoAtendimentoMock.Verify(r => r.UpdateAtendimentosAsync(50, It.Is<IEnumerable<CartorioAtendimento>>(c =>
                c.First().Ordem == 2 &&
                c.First().Titulo == "new" &&
                c.First().Descricao == "newd"
            )), Times.Once);
            _repoAtendimentoMock.Verify(r => r.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsFalse_WhenNotFound()
        {
            _repoMock.Setup(r => r.FindByIdAsync(90)).ReturnsAsync((Domain.Entities.ServicosEntity.CartorioEntity.Cartorio?)null);

            var svc = CreateService();
            var result = await svc.DeleteAsync(90);

            Assert.False(result);
            _repoMock.Verify(r => r.FindByIdAsync(90), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_DeletesAndCommits_WhenFound()
        {
            var entity = new Domain.Entities.ServicosEntity.CartorioEntity.Cartorio { Id = 91 };
            _repoMock.Setup(r => r.FindByIdAsync(91)).ReturnsAsync(entity);
            _repoMock.Setup(r => r.DeleteAsync(entity)).Returns(Task.CompletedTask);
            _repoMock.Setup(r => r.CommitAsync()).ReturnsAsync(true);

            var svc = CreateService();
            var result = await svc.DeleteAsync(91);

            Assert.True(result);
            _repoMock.Verify(r => r.DeleteAsync(entity), Times.Once);
            _repoMock.Verify(r => r.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task GetWithChildrenBySlugAtivoAsync_ReturnsResponse_WhenFound()
        {
            var entity = new Domain.Entities.ServicosEntity.CartorioEntity.Cartorio { Id = 11, TituloPagina = "Z", Slug = "z" };
            _repoMock.Setup(r => r.GetWithChildrenBySlugAtivoAsync("z")).ReturnsAsync(entity);

            var svc = CreateService();
            var res = await svc.GetWithChildrenBySlugAtivoAsync("z");

            Assert.NotNull(res);
            Assert.Equal(11, res!.Id);
            _repoMock.Verify(r => r.GetWithChildrenBySlugAtivoAsync("z"), Times.Once);
        }
    }
}