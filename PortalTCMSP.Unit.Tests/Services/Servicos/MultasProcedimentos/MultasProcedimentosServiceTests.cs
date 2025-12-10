using Moq;
using PortalTCMSP.Domain.DTOs.Requests.Servicos.MultasProcedimentos;
using PortalTCMSP.Domain.Entities.ServicosEntity.MultasProcedimentosEntity;
using PortalTCMSP.Domain.Repositories.Servicos.MultasProcedimentos;
using PortalTCMSP.Infra.Services.Servicos.MultasProcedimentos;

namespace PortalTCMSP.Infra.Tests.Services.Servicos.MultasProcedimentos
{
    public class MultasProcedimentosServiceTests
    {
        private readonly Mock<IMultasProcedimentosRepository> _repoMock = new();
        private readonly Mock<IMultasProcedimentosProcedimentoRepository> _repoProcedimentosMock = new();
        private readonly Mock<IMultasProcedimentosPortariaRelacionadaRepository> _repoPortariaMock = new();

        private MultasProcedimentosService CreateService()
            => new(_repoMock.Object, _repoProcedimentosMock.Object, _repoPortariaMock.Object);

        [Fact]
        public async Task GetAllAsync_ReturnsMappedResponses()
        {
            var entities = new List<Domain.Entities.ServicosEntity.MultasProcedimentosEntity.MultasProcedimentos>
            {
                new Domain.Entities.ServicosEntity.MultasProcedimentosEntity.MultasProcedimentos { Id = 1 },
                new Domain.Entities.ServicosEntity.MultasProcedimentosEntity.MultasProcedimentos { Id = 2 }
            };

            _repoMock.Setup(r => r.AllWithChildrenAsync()).ReturnsAsync(entities);

            var svc = CreateService();
            var result = await svc.GetAllAsync();

            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsResponse_WhenFound()
        {
            var entity = new Domain.Entities.ServicosEntity.MultasProcedimentosEntity.MultasProcedimentos { Id = 10 };
            _repoMock.Setup(r => r.GetWithChildrenByIdAsync(10)).ReturnsAsync(entity);

            var svc = CreateService();
            var result = await svc.GetByIdAsync(10);

            Assert.NotNull(result);
            Assert.Equal(10, result.Id);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsNull_WhenNotFound()
        {
            _repoMock.Setup(r => r.GetWithChildrenByIdAsync(5)).ReturnsAsync((PortalTCMSP.Domain.Entities.ServicosEntity.MultasProcedimentosEntity.MultasProcedimentos?)null);

            var svc = CreateService();
            var result = await svc.GetByIdAsync(5);

            Assert.Null(result);
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
        public async Task CreateAsync_DisablesExistingSlug_ThenInsertsAndReturnsId()
        {
            var request = new MultasProcedimentosCreateRequest
            {
                Slug = "my-slug",
                TituloPagina = "title",
                Ativo = true
            };

            var existing = new PortalTCMSP.Domain.Entities.ServicosEntity.MultasProcedimentosEntity.MultasProcedimentos { Id = 99, Slug = "my-slug" };
            _repoMock.Setup(r => r.GetBySlugAtivoAsync("my-slug")).ReturnsAsync(existing);
            _repoMock.Setup(r => r.DisableAsync(99)).ReturnsAsync(true);

            // capture the inserted entity to simulate DB assigning Id
            _repoMock.Setup(r => r.InsertAsync(It.IsAny<PortalTCMSP.Domain.Entities.ServicosEntity.MultasProcedimentosEntity.MultasProcedimentos>()))
                .Callback<PortalTCMSP.Domain.Entities.ServicosEntity.MultasProcedimentosEntity.MultasProcedimentos>(e => e.Id = 123)
                .Returns(Task.CompletedTask);

            _repoMock.Setup(r => r.CommitAsync()).ReturnsAsync(true);

            var svc = CreateService();
            var id = await svc.CreateAsync(request);

            Assert.Equal(123, id);
            _repoMock.Verify(r => r.DisableAsync(99), Times.Once);
            _repoMock.Verify(r => r.InsertAsync(It.Is<PortalTCMSP.Domain.Entities.ServicosEntity.MultasProcedimentosEntity.MultasProcedimentos>(e => e.Slug == "my-slug")), Times.Once);
            _repoMock.Verify(r => r.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task CreateProcedimentosAsync_Throws_WhenParentNotFound()
        {
            _repoMock.Setup(r => r.FindByIdAsync(5)).ReturnsAsync((PortalTCMSP.Domain.Entities.ServicosEntity.MultasProcedimentosEntity.MultasProcedimentos?)null);

            var svc = CreateService();
            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await svc.CreateProcedimentosAsync(5, new List<MultasProcedimentosProcedimentoRequest>()));
        }

        [Fact]
        public async Task CreateProcedimentosAsync_CreatesAndCommits()
        {
            var parent = new PortalTCMSP.Domain.Entities.ServicosEntity.MultasProcedimentosEntity.MultasProcedimentos { Id = 7 };
            _repoMock.Setup(r => r.FindByIdAsync(7)).ReturnsAsync(parent);

            IEnumerable<MultasProcedimentosProcedimento>? captured = null;
            _repoProcedimentosMock.Setup(r => r.CreateProcedimentosAsync(7, It.IsAny<IEnumerable<MultasProcedimentosProcedimento>>()))
                .Callback<long, IEnumerable<MultasProcedimentosProcedimento>>((id, ents) => captured = ents)
                .Returns(Task.CompletedTask);

            _repoProcedimentosMock.Setup(r => r.CommitAsync()).ReturnsAsync(true);

            var novos = new List<MultasProcedimentosProcedimentoRequest>
            {
                new() { Ordem = 1, Texto = " text ", UrlImagem = " img " },
                new() { Ordem = 2, Texto = "t2", UrlImagem = null }
            };

            var svc = CreateService();
            var result = await svc.CreateProcedimentosAsync(7, novos);

            Assert.True(result);
            Assert.NotNull(captured);
            var list = captured!.ToList();
            Assert.Equal(2, list.Count);
            Assert.Equal("text", list[0].Texto);
            Assert.Equal("img", list[0].UrlImagem);
            Assert.Equal("t2", list[1].Texto);
            Assert.Null(list[1].UrlImagem);
        }

        [Fact]
        public async Task CreatePortariaRelacionadasAsync_Throws_WhenParentNotFound()
        {
            _repoMock.Setup(r => r.FindByIdAsync(8)).ReturnsAsync((PortalTCMSP.Domain.Entities.ServicosEntity.MultasProcedimentosEntity.MultasProcedimentos?)null);

            var svc = CreateService();
            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await svc.CreatePortariaRelacionadasAsync(8, new List<MultasProcedimentosPortariaRelacionadaRequest>()));
        }

        [Fact]
        public async Task CreatePortariaRelacionadasAsync_CreatesAndCommits()
        {
            var parent = new PortalTCMSP.Domain.Entities.ServicosEntity.MultasProcedimentosEntity.MultasProcedimentos { Id = 11 };
            _repoMock.Setup(r => r.FindByIdAsync(11)).ReturnsAsync(parent);

            IEnumerable<MultasProcedimentosPortariaRelacionada>? captured = null;
            _repoPortariaMock.Setup(r => r.CreatePortariaRelacionadaAsync(11, It.IsAny<IEnumerable<MultasProcedimentosPortariaRelacionada>>()))
                .Callback<long, IEnumerable<MultasProcedimentosPortariaRelacionada>>((id, ents) => captured = ents)
                .Returns(Task.CompletedTask);

            _repoPortariaMock.Setup(r => r.CommitAsync()).ReturnsAsync(true);

            var novos = new List<MultasProcedimentosPortariaRelacionadaRequest>
            {
                new() { Ordem = 1, Titulo = " t1 ", Url = " u1 " },
                new() { Ordem = 2, Titulo = "t2", Url = "u2" }
            };

            var svc = CreateService();
            var result = await svc.CreatePortariaRelacionadasAsync(11, novos);

            Assert.True(result);
            Assert.NotNull(captured);
            var list = captured!.ToList();
            Assert.Equal(2, list.Count);
            Assert.Equal("t1", list[0].Titulo);
            Assert.Equal("u1", list[0].Url);
        }

        [Fact]
        public async Task GetBySlugAtivoAsync_ReturnsResponse_WhenFound()
        {
            var e = new PortalTCMSP.Domain.Entities.ServicosEntity.MultasProcedimentosEntity.MultasProcedimentos { Id = 55, Slug = "s" };
            _repoMock.Setup(r => r.GetBySlugAtivoAsync("s")).ReturnsAsync(e);

            var svc = CreateService();
            var resp = await svc.GetBySlugAtivoAsync("s");

            Assert.NotNull(resp);
            Assert.Equal(55, resp!.Id);
        }

        [Fact]
        public async Task GetWithChildrenBySlugAtivoAsync_ReturnsResponse_WhenFound()
        {
            var e = new PortalTCMSP.Domain.Entities.ServicosEntity.MultasProcedimentosEntity.MultasProcedimentos { Id = 66, Slug = "s2" };
            _repoMock.Setup(r => r.GetWithChildrenBySlugAtivoAsync("s2")).ReturnsAsync(e);

            var svc = CreateService();
            var resp = await svc.GetWithChildrenBySlugAtivoAsync("s2");

            Assert.NotNull(resp);
            Assert.Equal(66, resp!.Id);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsFalse_WhenNotFound()
        {
            _repoMock.Setup(r => r.GetWithChildrenByIdAsync(100)).ReturnsAsync((PortalTCMSP.Domain.Entities.ServicosEntity.MultasProcedimentosEntity.MultasProcedimentos?)null);

            var svc = CreateService();
            var result = await svc.UpdateAsync(100, new MultasProcedimentosUpdateRequest());

            Assert.False(result);
        }

        [Fact]
        public async Task UpdateAsync_ReplacesCollections_UpdatesEntityAndCommits()
        {
            var existing = new PortalTCMSP.Domain.Entities.ServicosEntity.MultasProcedimentosEntity.MultasProcedimentos
            {
                Id = 200,
                Procedimentos = new List<MultasProcedimentosProcedimento>(),
                PortariasRelacionadas = new List<MultasProcedimentosPortariaRelacionada>()
            };

            _repoMock.Setup(r => r.GetWithChildrenByIdAsync(200)).ReturnsAsync(existing);

            _repoMock.Setup(r => r.ReplaceProcedimentosAsync(200, It.IsAny<IEnumerable<MultasProcedimentosProcedimento>>()))
                .Returns(Task.CompletedTask);
            _repoMock.Setup(r => r.ReplacePortariasAsync(200, It.IsAny<IEnumerable<MultasProcedimentosPortariaRelacionada>>()))
                .Returns(Task.CompletedTask);

            _repoMock.Setup(r => r.UpdateAsync(existing)).Returns(Task.CompletedTask);
            _repoMock.Setup(r => r.CommitAsync()).ReturnsAsync(true);

            var request = new MultasProcedimentosUpdateRequest
            {
                Procedimentos = new List<MultasProcedimentosProcedimentoRequest>
                {
                    new() { Ordem = 1, Texto = " p ", UrlImagem = null }
                },
                PortariasRelacionadas = new List<MultasProcedimentosPortariaRelacionadaRequest>
                {
                    new() { Ordem = 1, Titulo = " t ", Url = " u " }
                }
            };

            var svc = CreateService();
            var result = await svc.UpdateAsync(200, request);

            Assert.True(result);
            _repoMock.Verify(r => r.ReplaceProcedimentosAsync(200, It.IsAny<IEnumerable<MultasProcedimentosProcedimento>>()), Times.Once);
            _repoMock.Verify(r => r.ReplacePortariasAsync(200, It.IsAny<IEnumerable<MultasProcedimentosPortariaRelacionada>>()), Times.Once);
            _repoMock.Verify(r => r.UpdateAsync(existing), Times.Once);
            _repoMock.Verify(r => r.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateProcedimentosAsync_Throws_WhenParentNotFound()
        {
            _repoMock.Setup(r => r.FindByIdAsync(300)).ReturnsAsync((PortalTCMSP.Domain.Entities.ServicosEntity.MultasProcedimentosEntity.MultasProcedimentos?)null);

            var svc = CreateService();
            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await svc.UpdateProcedimentosAsync(300, new List<MultasProcedimentosProcedimentoUpdate>()));
        }

        [Fact]
        public async Task UpdateProcedimentosAsync_Throws_WhenNoProcedimentos()
        {
            _repoMock.Setup(r => r.FindByIdAsync(301)).ReturnsAsync(new PortalTCMSP.Domain.Entities.ServicosEntity.MultasProcedimentosEntity.MultasProcedimentos { Id = 301 });
            _repoMock.Setup(r => r.GetWithChildrenByIdAsync(301)).ReturnsAsync((PortalTCMSP.Domain.Entities.ServicosEntity.MultasProcedimentosEntity.MultasProcedimentos?)null);

            var svc = CreateService();
            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await svc.UpdateProcedimentosAsync(301, new List<MultasProcedimentosProcedimentoUpdate>()));
        }

        [Fact]
        public async Task UpdateProcedimentosAsync_UpdatesExistingProcedimentos_AndCommits()
        {
            // existing parent with procedimentos
            var procedimentos = new List<MultasProcedimentosProcedimento>
            {
                new MultasProcedimentosProcedimento { Id = 401, Ordem = 1, Texto = "old", UrlImagem = "old.png" },
                new MultasProcedimentosProcedimento { Id = 402, Ordem = 2, Texto = "old2", UrlImagem = null }
            };
            var parentWithChildren = new PortalTCMSP.Domain.Entities.ServicosEntity.MultasProcedimentosEntity.MultasProcedimentos { Id = 40, Procedimentos = procedimentos };

            _repoMock.Setup(r => r.FindByIdAsync(40)).ReturnsAsync(new PortalTCMSP.Domain.Entities.ServicosEntity.MultasProcedimentosEntity.MultasProcedimentos { Id = 40 });
            _repoMock.Setup(r => r.GetWithChildrenByIdAsync(40)).ReturnsAsync(parentWithChildren);

            // each FindByIdAsync on procedimentos repository should return a value
            _repoProcedimentosMock.Setup(r => r.FindByIdAsync(401)).ReturnsAsync(procedimentos[0]);
            _repoProcedimentosMock.Setup(r => r.FindByIdAsync(402)).ReturnsAsync(procedimentos[1]);

            IEnumerable<MultasProcedimentosProcedimento>? updatedPassed = null;
            _repoProcedimentosMock.Setup(r => r.UpdateProcedimentosAsync(40, It.IsAny<IEnumerable<MultasProcedimentosProcedimento>>()))
                .Callback<long, IEnumerable<MultasProcedimentosProcedimento>>((id, ents) => updatedPassed = ents)
                .Returns(Task.CompletedTask);

            _repoProcedimentosMock.Setup(r => r.CommitAsync()).ReturnsAsync(true);

            var updates = new List<MultasProcedimentosProcedimentoUpdate>
            {
                new() { Id = 401, Ordem = 10, Texto = " new ", UrlImagem = " new.png " },
                new() { Id = 402, Ordem = 20, Texto = "new2", UrlImagem = null }
            };

            var svc = CreateService();
            var result = await svc.UpdateProcedimentosAsync(40, updates);

            Assert.True(result);
            Assert.NotNull(updatedPassed);
            var list = updatedPassed!.ToList();
            var first = list.First(p => p.Id == 401);
            Assert.Equal(10, first.Ordem);
            Assert.Equal("new", first.Texto);
            Assert.Equal("new.png", first.UrlImagem);
            _repoProcedimentosMock.Verify(r => r.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdatePortariaRelacionadasAsync_Throws_WhenParentNotFound()
        {
            _repoMock.Setup(r => r.FindByIdAsync(500)).ReturnsAsync((PortalTCMSP.Domain.Entities.ServicosEntity.MultasProcedimentosEntity.MultasProcedimentos?)null);

            var svc = CreateService();
            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await svc.UpdatePortariaRelacionadasAsync(500, new List<MultasProcedimentosPortariaRelacionadaUpdate>()));
        }

        [Fact]
        public async Task UpdatePortariaRelacionadasAsync_Throws_WhenNoPortarias()
        {
            _repoMock.Setup(r => r.FindByIdAsync(501)).ReturnsAsync(new PortalTCMSP.Domain.Entities.ServicosEntity.MultasProcedimentosEntity.MultasProcedimentos { Id = 501 });
            _repoMock.Setup(r => r.GetWithChildrenByIdAsync(501)).ReturnsAsync((PortalTCMSP.Domain.Entities.ServicosEntity.MultasProcedimentosEntity.MultasProcedimentos?)null);

            var svc = CreateService();
            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await svc.UpdatePortariaRelacionadasAsync(501, new List<MultasProcedimentosPortariaRelacionadaUpdate>()));
        }

        [Fact]
        public async Task UpdatePortariaRelacionadasAsync_UpdatesAndCommits()
        {
            var portarias = new List<MultasProcedimentosPortariaRelacionada>
            {
                new MultasProcedimentosPortariaRelacionada { Id = 601, Ordem = 1, Titulo = "o", Url = "u" },
                new MultasProcedimentosPortariaRelacionada { Id = 602, Ordem = 2, Titulo = "o2", Url = "u2" }
            };
            var parentWithChildren = new PortalTCMSP.Domain.Entities.ServicosEntity.MultasProcedimentosEntity.MultasProcedimentos { Id = 60, PortariasRelacionadas = portarias };

            _repoMock.Setup(r => r.FindByIdAsync(60)).ReturnsAsync(new PortalTCMSP.Domain.Entities.ServicosEntity.MultasProcedimentosEntity.MultasProcedimentos { Id = 60 });
            _repoMock.Setup(r => r.GetWithChildrenByIdAsync(60)).ReturnsAsync(parentWithChildren);

            _repoPortariaMock.Setup(r => r.FindByIdAsync(601)).ReturnsAsync(portarias[0]);
            _repoPortariaMock.Setup(r => r.FindByIdAsync(602)).ReturnsAsync(portarias[1]);

            IEnumerable<MultasProcedimentosPortariaRelacionada>? updatedPassed = null;
            _repoPortariaMock.Setup(r => r.UpdatePortariaRelacionadaAsync(60, It.IsAny<IEnumerable<MultasProcedimentosPortariaRelacionada>>()))
                .Callback<long, IEnumerable<MultasProcedimentosPortariaRelacionada>>((id, ents) => updatedPassed = ents)
                .Returns(Task.CompletedTask);

            _repoPortariaMock.Setup(r => r.CommitAsync()).ReturnsAsync(true);

            var updates = new List<MultasProcedimentosPortariaRelacionadaUpdate>
            {
                new() { Id = 601, Ordem = 11, Titulo = " new ", Url = " newu " },
                new() { Id = 602, Ordem = 22, Titulo = "new2", Url = "newu2" }
            };

            var svc = CreateService();
            var result = await svc.UpdatePortariaRelacionadasAsync(60, updates);

            Assert.True(result);
            Assert.NotNull(updatedPassed);
            var list = updatedPassed!.ToList();
            var first = list.First(p => p.Id == 601);
            Assert.Equal(11, first.Ordem);
            Assert.Equal("new", first.Titulo);
            Assert.Equal("newu", first.Url);
            _repoPortariaMock.Verify(r => r.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsFalse_WhenNotFound()
        {
            _repoMock.Setup(r => r.FindByIdAsync(700)).ReturnsAsync((PortalTCMSP.Domain.Entities.ServicosEntity.MultasProcedimentosEntity.MultasProcedimentos?)null);

            var svc = CreateService();
            var result = await svc.DeleteAsync(700);

            Assert.False(result);
        }

        [Fact]
        public async Task DeleteAsync_DeletesAndCommits_WhenFound()
        {
            var e = new PortalTCMSP.Domain.Entities.ServicosEntity.MultasProcedimentosEntity.MultasProcedimentos { Id = 701 };
            _repoMock.Setup(r => r.FindByIdAsync(701)).ReturnsAsync(e);
            _repoMock.Setup(r => r.DeleteAsync(e)).Returns(Task.CompletedTask);
            _repoMock.Setup(r => r.CommitAsync()).ReturnsAsync(true);

            var svc = CreateService();
            var result = await svc.DeleteAsync(701);

            Assert.True(result);
            _repoMock.Verify(r => r.DeleteAsync(e), Times.Once);
            _repoMock.Verify(r => r.CommitAsync(), Times.Once);
        }
    }
}