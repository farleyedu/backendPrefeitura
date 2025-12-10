using Moq;
using PortalTCMSP.Domain.DTOs.Requests.SessaoPlenaria.Pauta;
using PortalTCMSP.Domain.DTOs.Responses.Base;
using PortalTCMSP.Domain.DTOs.Responses.SessaoPlenaria.Pauta;
using PortalTCMSP.Domain.Entities.SessaoPlenariaEntity;
using PortalTCMSP.Domain.Enum;
using PortalTCMSP.Domain.Repositories.SessaoPlenaria;
using PortalTCMSP.Infra.Services.SessaoPlenaria;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Unit.Tests.Services.SessaoPlenariaServiceTest;

[ExcludeFromCodeCoverage]
public class SessaoPlenariaPautaServiceTest
{
    private readonly SessaoPlenariaPautaService _service;
    private readonly Mock<ISessaoPlenariaPautaRepository> _repoMock;

    public SessaoPlenariaPautaServiceTest()
    {
        _repoMock = new Mock<ISessaoPlenariaPautaRepository>();
        _service = new SessaoPlenariaPautaService(_repoMock.Object);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnPauta_WhenExists()
    {
        var pauta = new SessaoPlenariaPauta { Id = 1, IdSessaoPlenaria = 1, Tipo = PautaTipo.Ordinaria, DataCriacao = DateTime.UtcNow };

        _repoMock.Setup(r => r.GetWithAnexosByIdAsync(pauta.Id)).ReturnsAsync(pauta);

        var result = await _service.GetByIdAsync(pauta.Id);

        Assert.NotNull(result);
        Assert.Equal(pauta.Id, result.Id);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenNotExists()
    {
        _repoMock.Setup(r => r.GetWithAnexosByIdAsync(It.IsAny<long>())).ReturnsAsync((SessaoPlenariaPauta?)null);

        var result = await _service.GetByIdAsync(999);

        Assert.Null(result);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnId()
    {
        var request = new SessaoPlenariaPautaCreateRequest
        {
            IdSessaoPlenaria = 1,
            Tipo = PautaTipo.Ordinaria,
            DataDaSesao = DateTime.UtcNow,
            DataPublicacao = DateTime.UtcNow,
            Anexos = new List<SessaoPlenariaPautaAnexoItemRequest>
            {
                new SessaoPlenariaPautaAnexoItemRequest { Link = "http://example.com/anexo1.pdf", NomeExibicao = "Anexo 1", TipoArquivo = "pdf" }
            }
        };

        _repoMock.Setup(r => r.InsertAsync(It.IsAny<SessaoPlenariaPauta>())).Returns(Task.CompletedTask);
        _repoMock.Setup(r => r.CommitAsync()).ReturnsAsync(true);

        var result = await _service.CreateAsync(request);

        Assert.True(result >= 0);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnTrue_WhenPautaExists()
    {
        var pauta = new SessaoPlenariaPauta { Id = 1, IdSessaoPlenaria = 1, Tipo = PautaTipo.Ordinaria, DataCriacao = DateTime.UtcNow };
        var request = new SessaoPlenariaPautaUpdateRequest
        {
            IdSessaoPlenaria = 1,
            Tipo = PautaTipo.Extraordinaria,
            DataDaSesao = DateTime.UtcNow,
            DataPublicacao = DateTime.UtcNow,
            Anexos = new List<SessaoPlenariaPautaAnexoItemRequest>
            {
                new SessaoPlenariaPautaAnexoItemRequest { Link = "http://example.com/anexo1.pdf", NomeExibicao = "Anexo 1", TipoArquivo = "pdf" }
            }
        };

        _repoMock.Setup(r => r.GetWithAnexosByIdAsync(pauta.Id)).ReturnsAsync(pauta);
        _repoMock.Setup(r => r.ReplaceAnexosAsync(pauta.Id, It.IsAny<IEnumerable<SessaoPlenariaPautaAnexo>>())).Returns(Task.CompletedTask);
        _repoMock.Setup(r => r.UpdateAsync(It.IsAny<SessaoPlenariaPauta>())).Returns(Task.CompletedTask);
        _repoMock.Setup(r => r.CommitAsync()).ReturnsAsync(true);

        var result = await _service.UpdateAsync(pauta.Id, request);

        Assert.True(result);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnFalse_WhenPautaNotExists()
    {
        var request = new SessaoPlenariaPautaUpdateRequest
        {
            IdSessaoPlenaria = 1,
            Tipo = PautaTipo.Ordinaria,
            DataDaSesao = DateTime.UtcNow,
            DataPublicacao = DateTime.UtcNow,
            Anexos = new List<SessaoPlenariaPautaAnexoItemRequest>()
        };

        _repoMock.Setup(r => r.GetWithAnexosByIdAsync(It.IsAny<long>())).ReturnsAsync((SessaoPlenariaPauta?)null);

        var result = await _service.UpdateAsync(999, request);

        Assert.False(result);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnTrue_WhenPautaExists()
    {
        var pauta = new SessaoPlenariaPauta { Id = 1, IdSessaoPlenaria = 1, Tipo = PautaTipo.Ordinaria, DataCriacao = DateTime.UtcNow };

        _repoMock.Setup(r => r.FindByIdAsync(pauta.Id)).ReturnsAsync(pauta);
        _repoMock.Setup(r => r.DeleteAsync(pauta)).Returns(Task.CompletedTask);
        _repoMock.Setup(r => r.CommitAsync()).ReturnsAsync(true);

        var result = await _service.DeleteAsync(pauta.Id);

        Assert.True(result);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnFalse_WhenPautaNotExists()
    {
        _repoMock.Setup(r => r.FindByIdAsync(It.IsAny<long>())).ReturnsAsync((SessaoPlenariaPauta?)null!);

        var result = await _service.DeleteAsync(999);

        Assert.False(result);
    }

    [Fact]
    public async Task GetAllPautas_ShouldReturnPaginatedResult()
    {
        var request = new SessaoPlenariaPautaSearchRequest { IdSessaoPlenaria = 1, Tipo = PautaTipo.Ordinaria, Page = 1, Count = 10 };
        var pautas = new List<SessaoPlenariaPauta>
        {
            new SessaoPlenariaPauta { Id = 1, IdSessaoPlenaria = 1, Tipo = PautaTipo.Ordinaria, DataCriacao = DateTime.UtcNow },
            new SessaoPlenariaPauta { Id = 2, IdSessaoPlenaria = 1, Tipo = PautaTipo.Ordinaria, DataCriacao = DateTime.UtcNow }
        };
        var paginated = new ResultadoPaginadoResponse<SessaoPlenariaPautaResponse>(1, 10, pautas.Count, pautas.Select(p => new SessaoPlenariaPautaResponse { IdSessaoPlenaria = p.IdSessaoPlenaria }));

        _repoMock.Setup(r => r.Search(request)).ReturnsAsync(pautas);

        var result = await _service.GetAllPautas(request);

        Assert.NotNull(result);
        Assert.Equal(paginated.Count, result.Count);
    }
}