using Moq;
using PortalTCMSP.Domain.DTOs.Requests.SessaoPlenaria.Ata;
using PortalTCMSP.Domain.DTOs.Responses.Base;
using PortalTCMSP.Domain.DTOs.Responses.SessaoPlenaria.Ata;
using PortalTCMSP.Domain.Entities.SessaoPlenariaEntity;
using PortalTCMSP.Domain.Repositories.SessaoPlenaria;
using PortalTCMSP.Infra.Services.SessaoPlenaria;
using PortalTCMSP.Unit.Tests.Repository.SessaoPlenaria.FixFeature;
using PortalTCMSP.Unit.Tests.Services.FixFeature.SessaoPlenariaServiceFixture;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Unit.Tests.Services.SessaoPlenariaServiceTest;

[ExcludeFromCodeCoverage]
public class SessaoPlenariaAtaServiceTest
{
    private readonly SessaoPlenariaAtaService _service;
    private readonly Mock<ISessaoPlenariaAtasRepository> _repoMock;

    public SessaoPlenariaAtaServiceTest()
    {
        _repoMock = new Mock<ISessaoPlenariaAtasRepository>();
        _service = new SessaoPlenariaAtaService(_repoMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllAtas()
    {
        var atas = SessaoPlenariaAtaRepositoryFixture.GetSessoesPlenariasAtas(1);

        _repoMock.Setup(r => r.AllWithAnexosAsync()).ReturnsAsync(atas);
        var result = await _service.GetAllAsync();
        
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnAta_WhenExists()
    {
        var ata = SessaoPlenariaAtaRepositoryFixture.GetSessoesPlenariasAtas(1).First();
        _repoMock.Setup(r => r.GetWithAnexosByIdAsync(ata.Id)).ReturnsAsync(ata);

        var result = await _service.GetByIdAsync(ata.Id);

        Assert.NotNull(result);
        Assert.Equal(ata.Id, result.Id);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenNotExists()
    {
        _repoMock.Setup(r => r.GetWithAnexosByIdAsync(It.IsAny<long>())).ReturnsAsync((SessaoPlenariaAta?)null);

        var result = await _service.GetByIdAsync(999);

        Assert.Null(result);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnId()
    {
        var request = SessaoPlenariaAtaServiceFixture.GetSessaoPlenariaAtaCreateRequest();
        var ata = SessaoPlenariaAtaRepositoryFixture.GetSessoesPlenariasAtas(1).First();

        _repoMock.Setup(r => r.InsertAsync(It.IsAny<SessaoPlenariaAta>())).Returns(Task.CompletedTask);
        _repoMock.Setup(r => r.CommitAsync()).ReturnsAsync(true);

        var result = await _service.CreateAsync(request);

        Assert.True(result == 0);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnTrue_WhenAtaExists()
    {
        var ata = SessaoPlenariaAtaRepositoryFixture.GetSessoesPlenariasAtas(1).First();
        var request = SessaoPlenariaAtaServiceFixture.GetUpdateRequest();

        _repoMock.Setup(r => r.GetWithAnexosByIdAsync(1)).ReturnsAsync(ata);
        _repoMock.Setup(r => r.UpdateAsync(It.IsAny<SessaoPlenariaAta>())).Returns(Task.CompletedTask);
        _repoMock.Setup(r => r.CommitAsync()).ReturnsAsync(true);

        var result = await _service.UpdateAsync(ata.Id, request);

        Assert.True(result);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnFalse_WhenAtaNotExists()
    {
        var request = SessaoPlenariaAtaServiceFixture.GetUpdateRequest();

        _repoMock.Setup(r => r.GetWithAnexosByIdAsync(It.IsAny<long>())).ReturnsAsync((SessaoPlenariaAta?)null);

        var result = await _service.UpdateAsync(999, request);

        Assert.False(result);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnTrue_WhenAtaExists()
    {
        var ata = SessaoPlenariaAtaRepositoryFixture.GetSessoesPlenariasAtas(1).First();

        _repoMock.Setup(r => r.FindByIdAsync(ata.Id)).ReturnsAsync(ata);
        _repoMock.Setup(r => r.DeleteAsync(ata)).Returns(Task.CompletedTask);
        _repoMock.Setup(r => r.CommitAsync()).ReturnsAsync(true);

        var result = await _service.DeleteAsync(ata.Id);

        Assert.True(result);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnFalse_WhenAtaNotExists()
    {
        _repoMock.Setup(r => r.FindByIdAsync(It.IsAny<long>())).ReturnsAsync((SessaoPlenariaAta?)null!);

        var result = await _service.DeleteAsync(999);

        Assert.False(result);
    }

    [Fact]
    public async Task GetAllAtas_ShouldReturnPaginatedResult()
    {
        var request = new SessaoPlenariaAtaSearchRequest { Query = "teste" };
        var atas = SessaoPlenariaAtaRepositoryFixture.GetSessoesPlenariasAtas(2);
        var paginated = new ResultadoPaginadoResponse<SessaoPlenariaAtaResponse>(1, 10, atas.Count, atas.Select(a => new SessaoPlenariaAtaResponse { Id = a.Id }));

        _repoMock.Setup(r => r.Search(request)).ReturnsAsync(atas);

        var result = await _service.GetAllAtas(request);

        Assert.NotNull(result);
        Assert.Equal(paginated.Count, result.Count);
    }
}
