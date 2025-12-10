using Moq;
using PortalTCMSP.Domain.DTOs.Requests.Servicos.EmissaoCertidoes;
using PortalTCMSP.Domain.Entities.ServicosEntity.EmissaoCertidoesEntity;
using PortalTCMSP.Domain.Enum;
using PortalTCMSP.Domain.Repositories.Servicos.EmissaoCertidoes;
using PortalTCMSP.Infra.Services.Servicos.EmissaoCertidoes;

namespace PortalTCMSP.Unit.Tests.Services.Servicos.EmissaoCertidoes
{
    public class EmissaoCertidoesServiceTests
    {
        private readonly Mock<IEmissaoCertidoesRepository> _repoMock = new();
        private readonly Mock<IEmissaoCertidoesAcaoRepository> _repoAcoesMock = new();
        private readonly Mock<IEmissaoCertidoesSecaoOrientacaoRepository> _repoSecaoMock = new();

        private EmissaoCertidoesService CreateService()
            => new EmissaoCertidoesService(_repoMock.Object, _repoAcoesMock.Object, _repoSecaoMock.Object);

        [Fact]
        public async Task GetAllAsync_ReturnsMappedResponses()
        {
            var entities = new List<Domain.Entities.ServicosEntity.EmissaoCertidoesEntity.EmissaoCertidoes>
            {
                new Domain.Entities.ServicosEntity.EmissaoCertidoesEntity.EmissaoCertidoes { Id = 1, TituloPagina = "T1", Slug = "s1", Ativo = true, DataCriacao = DateTime.UtcNow }
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
            var entity = new Domain.Entities.ServicosEntity.EmissaoCertidoesEntity.EmissaoCertidoes { Id = 5, TituloPagina = "X", Slug = "x", Ativo = true, DataCriacao = DateTime.UtcNow };
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
            _repoMock.Setup(r => r.GetWithChildrenByIdAsync(10)).ReturnsAsync((Domain.Entities.ServicosEntity.EmissaoCertidoesEntity.EmissaoCertidoes?)null);

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
            var request = new EmissaoCertidoesCreateRequest { TituloPagina = "Title", Slug = "my-slug", Ativo = true };
            _repoMock.Setup(r => r.GetBySlugAtivoAsync(It.IsAny<string>())).ReturnsAsync((Domain.Entities.ServicosEntity.EmissaoCertidoesEntity.EmissaoCertidoes?)null);
            _repoMock.Setup(r => r.InsertAsync(It.IsAny<Domain.Entities.ServicosEntity.EmissaoCertidoesEntity.EmissaoCertidoes>()))
                     .Callback<Domain.Entities.ServicosEntity.EmissaoCertidoesEntity.EmissaoCertidoes>(e => e.Id = 42)
                     .Returns(Task.CompletedTask);
            _repoMock.Setup(r => r.CommitAsync()).ReturnsAsync(true);

            var svc = CreateService();
            var id = await svc.CreateAsync(request);

            Assert.Equal(42, id);
            _repoMock.Verify(r => r.GetBySlugAtivoAsync(It.IsAny<string>()), Times.Once);
            _repoMock.Verify(r => r.InsertAsync(It.IsAny<Domain.Entities.ServicosEntity.EmissaoCertidoesEntity.EmissaoCertidoes>()), Times.Once);
            _repoMock.Verify(r => r.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_DisablesExistingBeforeInsert_WhenExistingSlugFound()
        {
            var existing = new Domain.Entities.ServicosEntity.EmissaoCertidoesEntity.EmissaoCertidoes { Id = 99, TituloPagina = "Old", Slug = "my-slug", Ativo = true };
            var request = new EmissaoCertidoesCreateRequest { TituloPagina = "New", Slug = "my-slug", Ativo = true };

            _repoMock.Setup(r => r.GetBySlugAtivoAsync(It.IsAny<string>())).ReturnsAsync(existing);
            _repoMock.Setup(r => r.DisableAsync(existing.Id)).ReturnsAsync(true);
            _repoMock.Setup(r => r.InsertAsync(It.IsAny<Domain.Entities.ServicosEntity.EmissaoCertidoesEntity.EmissaoCertidoes>()))
                     .Callback<Domain.Entities.ServicosEntity.EmissaoCertidoesEntity.EmissaoCertidoes>(e => e.Id = 100)
                     .Returns(Task.CompletedTask);
            _repoMock.Setup(r => r.CommitAsync()).ReturnsAsync(true);

            var svc = CreateService();
            var id = await svc.CreateAsync(request);

            Assert.Equal(100, id);
            _repoMock.Verify(r => r.DisableAsync(99), Times.Once);
            _repoMock.Verify(r => r.InsertAsync(It.IsAny<Domain.Entities.ServicosEntity.EmissaoCertidoesEntity.EmissaoCertidoes>()), Times.Once);
        }

        [Fact]
        public async Task CreateAcoesAsync_MapsAndCommits()
        {
            var reqs = new List<EmissaoCertidoesAcaoRequest>
            {
                new EmissaoCertidoesAcaoRequest { Ativo = true, Ordem = 1, Titulo = " T1 ", UrlAcao = " /u ", DataPublicacao = DateTime.UtcNow, TipoAcao = TipoAcao.GERAR }
            };

            _repoAcoesMock.Setup(r => r.CreateAcoesAsync(5, It.IsAny<IEnumerable<EmissaoCertidoesAcao>>()))
                           .Returns(Task.CompletedTask)
                           .Verifiable();
            _repoAcoesMock.Setup(r => r.CommitAsync()).ReturnsAsync(true);

            var svc = CreateService();
            var result = await svc.CreateAcoesAsync(5, reqs);

            Assert.True(result);
            _repoAcoesMock.Verify(r => r.CreateAcoesAsync(5, It.Is<IEnumerable<EmissaoCertidoesAcao>>(c =>
                c.First().Titulo == "T1" &&
                c.First().UrlAcao == "/u" &&
                c.First().IdEmissaoCertidoes == 5
            )), Times.Once);
            _repoAcoesMock.Verify(r => r.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task CreateSecoesAsync_Throws_WhenNotFound()
        {
            _repoMock.Setup(r => r.FindByIdAsync(7)).ReturnsAsync((Domain.Entities.ServicosEntity.EmissaoCertidoesEntity.EmissaoCertidoes?)null);

            var svc = CreateService();
            await Assert.ThrowsAsync<InvalidOperationException>(() => svc.CreateSecoesAsync(7, new List<EmissaoCertidoesSecaoOrientacaoRequest>()));
        }

        [Fact]
        public async Task CreateSecoesAsync_MapsAndCommits()
        {
            var parent = new Domain.Entities.ServicosEntity.EmissaoCertidoesEntity.EmissaoCertidoes { Id = 8 };
            _repoMock.Setup(r => r.FindByIdAsync(8)).ReturnsAsync(parent);
            _repoSecaoMock.Setup(r => r.CreateScoesAsync(8, It.IsAny<IEnumerable<EmissaoCertidoesSecaoOrientacao>>())).Returns(Task.CompletedTask);
            _repoSecaoMock.Setup(r => r.CommitAsync()).ReturnsAsync(true);

            var secs = new List<EmissaoCertidoesSecaoOrientacaoRequest>
            {
                new EmissaoCertidoesSecaoOrientacaoRequest
                {
                    TipoSecao = TipoSecao.PessoaFisica,
                    TituloPagina = " Titulo ",
                    Descricao = " Desc ",
                    Orientacoes = new List<EmissaoCertidoesOrientacaoRequest>
                    {
                        new EmissaoCertidoesOrientacaoRequest { Ordem = 1, Descritivos = new List<string> { " a ", " b " } }
                    }
                }
            };

            var svc = CreateService();
            var result = await svc.CreateSecoesAsync(8, secs);

            Assert.True(result);
            _repoSecaoMock.Verify(r => r.CreateScoesAsync(8, It.Is<IEnumerable<EmissaoCertidoesSecaoOrientacao>>(c =>
                c.First().TituloPagina == "Titulo" &&
                c.First().Descricao == "Desc" &&
                c.First().Orientacoes.First().Descritivos.Count == 2
            )), Times.Once);
            _repoSecaoMock.Verify(r => r.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task GetWithChildrenBySlugAtivoAsync_ReturnsResponse_WhenFound()
        {
            var entity = new Domain.Entities.ServicosEntity.EmissaoCertidoesEntity.EmissaoCertidoes { Id = 11, TituloPagina = "Z", Slug = "z" };
            _repoMock.Setup(r => r.GetWithChildrenBySlugAtivoAsync("z")).ReturnsAsync(entity);

            var svc = CreateService();
            var res = await svc.GetWithChildrenBySlugAtivoAsync("z");

            Assert.NotNull(res);
            Assert.Equal(11, res!.Id);
            _repoMock.Verify(r => r.GetWithChildrenBySlugAtivoAsync("z"), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ReturnsFalse_WhenNotFound()
        {
            _repoMock.Setup(r => r.GetWithChildrenByIdAsync(20)).ReturnsAsync((Domain.Entities.ServicosEntity.EmissaoCertidoesEntity.EmissaoCertidoes?)null);

            var svc = CreateService();
            var result = await svc.UpdateAsync(20, new EmissaoCertidoesUpdateRequest());

            Assert.False(result);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesAndCommits_WhenFound()
        {
            var existing = new Domain.Entities.ServicosEntity.EmissaoCertidoesEntity.EmissaoCertidoes { Id = 30, TituloPagina = "Old", Slug = "old", Acoes = new List<EmissaoCertidoesAcao>(), SecaoOrientacoes = new List<EmissaoCertidoesSecaoOrientacao>() };
            _repoMock.Setup(r => r.GetWithChildrenByIdAsync(30)).ReturnsAsync(existing);
            _repoMock.Setup(r => r.ReplaceAcoesAsync(30, It.IsAny<IEnumerable<EmissaoCertidoesAcao>>())).Returns(Task.CompletedTask);
            _repoMock.Setup(r => r.ReplaceSecoesAsync(30, It.IsAny<IEnumerable<EmissaoCertidoesSecaoOrientacao>>())).Returns(Task.CompletedTask);
            _repoMock.Setup(r => r.UpdateAsync(existing)).Returns(Task.CompletedTask);
            _repoMock.Setup(r => r.CommitAsync()).ReturnsAsync(true);

            var req = new EmissaoCertidoesUpdateRequest
            {
                TituloPagina = "New",
                Slug = "new",
                Ativo = true,
                Acoes = new List<EmissaoCertidoesAcaoRequest>
                {
                    new EmissaoCertidoesAcaoRequest { Ativo = true, Ordem = 1, Titulo = " t ", UrlAcao = " u ", DataPublicacao = DateTime.UtcNow, TipoAcao = TipoAcao.ACESSAR }
                },
                SecaoOrientacoes = new List<EmissaoCertidoesSecaoOrientacaoRequest>
                {
                    new EmissaoCertidoesSecaoOrientacaoRequest { TipoSecao = TipoSecao.PessoaFisica, TituloPagina = " s ", Descricao = " d " }
                }
            };

            var svc = CreateService();
            var result = await svc.UpdateAsync(30, req);

            Assert.True(result);
            _repoMock.Verify(r => r.ReplaceAcoesAsync(30, It.IsAny<IEnumerable<EmissaoCertidoesAcao>>()), Times.Once);
            _repoMock.Verify(r => r.ReplaceSecoesAsync(30, It.IsAny<IEnumerable<EmissaoCertidoesSecaoOrientacao>>()), Times.Once);
            _repoMock.Verify(r => r.UpdateAsync(existing), Times.Once);
            _repoMock.Verify(r => r.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateAcoesAsync_Throws_WhenNoParentEntity()
        {
            _repoMock.Setup(r => r.GetWithChildrenByIdAsync(40)).ReturnsAsync((Domain.Entities.ServicosEntity.EmissaoCertidoesEntity.EmissaoCertidoes?)null);

            var svc = CreateService();
            await Assert.ThrowsAsync<InvalidOperationException>(() => svc.UpdateAcoesAsync(40, new List<EmissaoCertidoesAcaoUpdate>()));
        }

        [Fact]
        public async Task UpdateAcoesAsync_Throws_WhenActionNotFound()
        {
            var parent = new Domain.Entities.ServicosEntity.EmissaoCertidoesEntity.EmissaoCertidoes { Id = 41, Acoes = new List<EmissaoCertidoesAcao> { new EmissaoCertidoesAcao { Id = 500 } } };
            _repoMock.Setup(r => r.GetWithChildrenByIdAsync(41)).ReturnsAsync(parent);
            _repoAcoesMock.Setup(r => r.FindByIdAsync(500)).ReturnsAsync((EmissaoCertidoesAcao?)null);

            var svc = CreateService();
            var novas = new List<EmissaoCertidoesAcaoUpdate> { new EmissaoCertidoesAcaoUpdate { Id = 500 } };

            await Assert.ThrowsAsync<InvalidOperationException>(() => svc.UpdateAcoesAsync(41, novas));
        }

        [Fact]
        public async Task UpdateAcoesAsync_UpdatesAndCommits_WhenFound()
        {
            var old = new EmissaoCertidoesAcao { Id = 600, Ordem = 1, Titulo = "old", UrlAcao = "u", Ativo = false };
            var parent = new Domain.Entities.ServicosEntity.EmissaoCertidoesEntity.EmissaoCertidoes { Id = 50, Acoes = new List<EmissaoCertidoesAcao> { old } };
            _repoMock.Setup(r => r.GetWithChildrenByIdAsync(50)).ReturnsAsync(parent);
            _repoAcoesMock.Setup(r => r.FindByIdAsync(600)).ReturnsAsync(old);
            _repoAcoesMock.Setup(r => r.UpdateAcoesAsync(50, It.IsAny<IEnumerable<EmissaoCertidoesAcao>>())).Returns(Task.CompletedTask);
            _repoAcoesMock.Setup(r => r.CommitAsync()).ReturnsAsync(true);

            var novas = new List<EmissaoCertidoesAcaoUpdate>
            {
                new EmissaoCertidoesAcaoUpdate { Id = 600, Ordem = 2, Titulo = " new ", UrlAcao = " newu ", DataPublicacao = DateTime.UtcNow, TipoAcao = TipoAcao.GERAR, Ativo = true }
            };

            var svc = CreateService();
            var result = await svc.UpdateAcoesAsync(50, novas);

            Assert.True(result);
            _repoAcoesMock.Verify(r => r.UpdateAcoesAsync(50, It.Is<IEnumerable<EmissaoCertidoesAcao>>(c =>
                c.First().Ordem == 2 &&
                c.First().Titulo == "new" &&
                c.First().UrlAcao == "newu" &&
                c.First().Ativo == true
            )), Times.Once);
            _repoAcoesMock.Verify(r => r.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateSecoesAsync_Throws_WhenNoParentEntity()
        {
            _repoMock.Setup(r => r.GetWithChildrenByIdAsync(60)).ReturnsAsync((Domain.Entities.ServicosEntity.EmissaoCertidoesEntity.EmissaoCertidoes?)null);

            var svc = CreateService();
            await Assert.ThrowsAsync<InvalidOperationException>(() => svc.UpdateSecoesAsync(60, new List<EmissaoCertidoesSecaoOrientacaoUpdate>()));
        }

        [Fact]
        public async Task UpdateSecoesAsync_Throws_WhenSecaoNotFound()
        {
            var parent = new Domain.Entities.ServicosEntity.EmissaoCertidoesEntity.EmissaoCertidoes { Id = 61, SecaoOrientacoes = new List<EmissaoCertidoesSecaoOrientacao> { new EmissaoCertidoesSecaoOrientacao { Id = 700 } } };
            _repoMock.Setup(r => r.GetWithChildrenByIdAsync(61)).ReturnsAsync(parent);
            _repoSecaoMock.Setup(r => r.FindByIdAsync(700)).ReturnsAsync((EmissaoCertidoesSecaoOrientacao?)null);

            var svc = CreateService();
            var novas = new List<EmissaoCertidoesSecaoOrientacaoUpdate> { new EmissaoCertidoesSecaoOrientacaoUpdate { Id = 700 } };

            await Assert.ThrowsAsync<InvalidOperationException>(() => svc.UpdateSecoesAsync(61, novas));
        }

        [Fact]
        public async Task UpdateSecoesAsync_UpdatesAndCommits_WhenFound()
        {
            var orientacao = new EmissaoCertidoesOrientacao { Ordem = 1, Descritivos = new List<EmissaoCertidoesDescritivo> { new EmissaoCertidoesDescritivo { Texto = "a" } } };
            var secao = new EmissaoCertidoesSecaoOrientacao { Id = 800, TipoSecao = TipoSecao.PessoaFisica, TituloPagina = "old", Descricao = "d", Orientacoes = new List<EmissaoCertidoesOrientacao> { orientacao } };
            var parent = new Domain.Entities.ServicosEntity.EmissaoCertidoesEntity.EmissaoCertidoes { Id = 70, SecaoOrientacoes = new List<EmissaoCertidoesSecaoOrientacao> { secao } };

            _repoMock.Setup(r => r.GetWithChildrenByIdAsync(70)).ReturnsAsync(parent);
            _repoSecaoMock.Setup(r => r.FindByIdAsync(800)).ReturnsAsync(secao);
            _repoSecaoMock.Setup(r => r.UpdateSecoesAsync(70, It.IsAny<IEnumerable<EmissaoCertidoesSecaoOrientacao>>())).Returns(Task.CompletedTask);
            _repoSecaoMock.Setup(r => r.CommitAsync()).ReturnsAsync(true);

            var novas = new List<EmissaoCertidoesSecaoOrientacaoUpdate>
            {
                new EmissaoCertidoesSecaoOrientacaoUpdate
                {
                    Id = 800,
                    TipoSecao = TipoSecao.PessoaFisica,
                    TituloPagina = " new ",
                    Descricao = " newdesc ",
                    Orientacoes = new List<EmissaoCertidoesOrientacaoUpdate>
                    {
                        new EmissaoCertidoesOrientacaoUpdate { Ordem = 2, Descritivos = new List<string> { " x ", " y " } }
                    }
                }
            };

            var svc = CreateService();
            var result = await svc.UpdateSecoesAsync(70, novas);

            Assert.True(result);
            _repoSecaoMock.Verify(r => r.UpdateSecoesAsync(70, It.Is<IEnumerable<EmissaoCertidoesSecaoOrientacao>>(c =>
                c.First().TituloPagina == "new" &&
                c.First().Descricao == "newdesc" &&
                c.First().Orientacoes.First().Descritivos.Count == 2
            )), Times.Once);
            _repoSecaoMock.Verify(r => r.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_ReturnsFalse_WhenNotFound()
        {
            _repoMock.Setup(r => r.FindByIdAsync(90)).ReturnsAsync((Domain.Entities.ServicosEntity.EmissaoCertidoesEntity.EmissaoCertidoes?)null);

            var svc = CreateService();
            var result = await svc.DeleteAsync(90);

            Assert.False(result);
            _repoMock.Verify(r => r.FindByIdAsync(90), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_DeletesAndCommits_WhenFound()
        {
            var entity = new Domain.Entities.ServicosEntity.EmissaoCertidoesEntity.EmissaoCertidoes { Id = 91 };
            _repoMock.Setup(r => r.FindByIdAsync(91)).ReturnsAsync(entity);
            _repoMock.Setup(r => r.DeleteAsync(entity)).Returns(Task.CompletedTask);
            _repoMock.Setup(r => r.CommitAsync()).ReturnsAsync(true);

            var svc = CreateService();
            var result = await svc.DeleteAsync(91);

            Assert.True(result);
            _repoMock.Verify(r => r.DeleteAsync(entity), Times.Once);
            _repoMock.Verify(r => r.CommitAsync(), Times.Once);
        }
    }
}