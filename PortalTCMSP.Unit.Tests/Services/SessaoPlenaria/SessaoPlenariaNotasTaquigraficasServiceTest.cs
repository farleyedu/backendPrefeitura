using Moq;
using PortalTCMSP.Domain.DTOs.Requests.SessaoPlenaria.Ata;
using PortalTCMSP.Domain.DTOs.Requests.SessaoPlenaria.NotasTaquigraficas;
using PortalTCMSP.Domain.DTOs.Responses.Base;
using PortalTCMSP.Domain.DTOs.Responses.SessaoPlenaria.NotasTaquigraficas;
using PortalTCMSP.Domain.Entities.SessaoPlenariaEntity;
using PortalTCMSP.Domain.Enum;
using PortalTCMSP.Domain.Repositories.SessaoPlenaria;
using PortalTCMSP.Infra.Services.SessaoPlenaria;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Unit.Tests.Services.SessaoPlenariaServiceTest;

[ExcludeFromCodeCoverage]
public class SessaoPlenariaNotasTaquigraficasServiceTest
{
    private readonly SessaoPlenariaNotasTaquigraficasService _service;
    private readonly Mock<ISessaoPlenariaNotasTaquigraficasRepository> _repoMock;

    public SessaoPlenariaNotasTaquigraficasServiceTest()
    {
        _repoMock = new Mock<ISessaoPlenariaNotasTaquigraficasRepository>();
        _service = new SessaoPlenariaNotasTaquigraficasService(_repoMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllNotas()
    {
        var notas = new List<SessaoPlenariaNotasTaquigraficas>
        {
            new SessaoPlenariaNotasTaquigraficas { Id = 1, IdSessaoPlenaria = 1, Tipo = NotasTipo.Ordinaria, DataCriacao = DateTime.UtcNow },
            new SessaoPlenariaNotasTaquigraficas { Id = 2, IdSessaoPlenaria = 1, Tipo = NotasTipo.Extraordinaria, DataCriacao = DateTime.UtcNow }
        };

        _repoMock.Setup(r => r.AllWithAnexosAsync()).ReturnsAsync(notas);

        var result = await _service.GetAllAsync();

        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNota_WhenExists()
    {
        var nota = new SessaoPlenariaNotasTaquigraficas { Id = 1, IdSessaoPlenaria = 1, Tipo = NotasTipo.Ordinaria, DataCriacao = DateTime.UtcNow };

        _repoMock.Setup(r => r.GetWithAnexosByIdAsync(nota.Id)).ReturnsAsync(nota);

        var result = await _service.GetByIdAsync(nota.Id);

        Assert.NotNull(result);
        Assert.Equal(nota.Id, result.Id);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenNotExists()
    {
        _repoMock.Setup(r => r.GetWithAnexosByIdAsync(It.IsAny<long>())).ReturnsAsync((SessaoPlenariaNotasTaquigraficas?)null);

        var result = await _service.GetByIdAsync(999);

        Assert.Null(result);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnId()
    {
        var request = new SessaoPlenariaNotasTaquigraficasCreateRequest
        {
            IdSessaoPlenaria = 1,
            Tipo = NotasTipo.Ordinaria,
            DataPublicacao = DateTime.UtcNow,
            Anexos = new List<SessaoPlenariaNotasTaquigraficasAnexoItemRequest>
            {
                new SessaoPlenariaNotasTaquigraficasAnexoItemRequest { Link = "http://example.com/anexo1.pdf", NomeExibicao = "Anexo 1", TipoArquivo = "pdf" }
            }
        };

        _repoMock.Setup(r => r.InsertAsync(It.IsAny<SessaoPlenariaNotasTaquigraficas>())).Returns(Task.CompletedTask);
        _repoMock.Setup(r => r.CommitAsync()).ReturnsAsync(true);

        var result = await _service.CreateAsync(request);

        Assert.True(result >= 0);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnTrue_WhenNotaExists()
    {
        var nota = new SessaoPlenariaNotasTaquigraficas { Id = 1, IdSessaoPlenaria = 1, Tipo = NotasTipo.Ordinaria, DataCriacao = DateTime.UtcNow };
        var request = new SessaoPlenariaNotasTaquigraficasUpdateRequest
        {
            IdSessaoPlenaria = 1,
            Tipo = NotasTipo.Extraordinaria,
            DataPublicacao = DateTime.UtcNow,
            Anexos = new List<SessaoPlenariaNotasTaquigraficasAnexoItemRequest>
            {
                new SessaoPlenariaNotasTaquigraficasAnexoItemRequest { Link = "http://example.com/anexo1.pdf", NomeExibicao = "Anexo 1", TipoArquivo = "pdf" }
            }
        };

        _repoMock.Setup(r => r.GetWithAnexosByIdAsync(nota.Id)).ReturnsAsync(nota);
        _repoMock.Setup(r => r.ReplaceAnexosAsync(nota.Id, It.IsAny<IEnumerable<SessaoPlenariaNotasTaquigraficasAnexos>>())).Returns(Task.CompletedTask);
        _repoMock.Setup(r => r.UpdateAsync(It.IsAny<SessaoPlenariaNotasTaquigraficas>())).Returns(Task.CompletedTask);
        _repoMock.Setup(r => r.CommitAsync()).ReturnsAsync(true);

        var result = await _service.UpdateAsync(nota.Id, request);

        Assert.True(result);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnTrue_WhenNotaExists()
    {
        var nota = new SessaoPlenariaNotasTaquigraficas { Id = 1, IdSessaoPlenaria = 1, Tipo = NotasTipo.Ordinaria, DataCriacao = DateTime.UtcNow };

        _repoMock.Setup(r => r.FindByIdAsync(nota.Id)).ReturnsAsync(nota);
        _repoMock.Setup(r => r.DeleteAsync(nota)).Returns(Task.CompletedTask);
        _repoMock.Setup(r => r.CommitAsync()).ReturnsAsync(true);

        var result = await _service.DeleteAsync(nota.Id);

        Assert.True(result);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnFalse_WhenNotaNotExists()
    {
        _repoMock.Setup(r => r.FindByIdAsync(It.IsAny<long>())).ReturnsAsync((SessaoPlenariaNotasTaquigraficas?)null!);

        var result = await _service.DeleteAsync(999);

        Assert.False(result);
    }

    [Fact]
    public async Task GetAllNotas_ShouldReturnPaginatedResult()
    {
        var request = new SessaoPlenariaNotasTaquigraficasSearchRequest { IdSessaoPlenaria = 1, Tipo = NotasTipo.Ordinaria, Page = 1, Count = 10 };
        var notas = new List<SessaoPlenariaNotasTaquigraficas>
        {
            new SessaoPlenariaNotasTaquigraficas { Id = 1, IdSessaoPlenaria = 1, Tipo = NotasTipo.Ordinaria, DataCriacao = DateTime.UtcNow },
            new SessaoPlenariaNotasTaquigraficas { Id = 2, IdSessaoPlenaria = 1, Tipo = NotasTipo.Ordinaria, DataCriacao = DateTime.UtcNow }
        };
        var paginated = new ResultadoPaginadoResponse<SessaoPlenariaNotasTaquigraficasResponse>(1, 10, notas.Count, notas.Select(n => new SessaoPlenariaNotasTaquigraficasResponse { Id = n.Id }));

        _repoMock.Setup(r => r.Search(request)).ReturnsAsync(notas);

        var result = await _service.GetAllNotas(request);

        Assert.NotNull(result);
        Assert.Equal(paginated.Count, result.Count);
    }
}