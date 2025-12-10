using Moq;
using PortalTCMSP.Domain.DTOs.Requests.SessaoPlenaria.Jurispudencia;
using PortalTCMSP.Domain.Entities.SessaoPlenariaEntity;
using PortalTCMSP.Domain.Repositories.SessaoPlenaria;
using PortalTCMSP.Infra.Services.SessaoPlenaria;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Unit.Tests.Services.SessaoPlenariaServiceTest;

[ExcludeFromCodeCoverage]
public class SessaoPlenariaJurispudenciaServiceTest
{
    private readonly SessaoPlenariaJurispudenciaService _service;
    private readonly Mock<ISessaoPlenariaJurispudenciaRepository> _repoMock;

    public SessaoPlenariaJurispudenciaServiceTest()
    {
        _repoMock = new Mock<ISessaoPlenariaJurispudenciaRepository>();
        _service = new SessaoPlenariaJurispudenciaService(_repoMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllJurisprudencias()
    {
        var items = new List<SessaoPlenariaJurispudencia>
        {
            new SessaoPlenariaJurispudencia { Id = 1, Slug = "slug-1", Titulo = "Título 1", Descricao = "Desc 1", DataCriacao = DateTime.UtcNow },
            new SessaoPlenariaJurispudencia { Id = 2, Slug = "slug-2", Titulo = "Título 2", Descricao = "Desc 2", DataCriacao = DateTime.UtcNow }
        };

        _repoMock.Setup(r => r.AllAsync()).ReturnsAsync(items);

        var result = await _service.GetAllAsync();

        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnJurisprudencia_WhenExists()
    {
        var item = new SessaoPlenariaJurispudencia { Id = 1, Slug = "slug-1", Titulo = "Título 1", Descricao = "Desc 1", DataCriacao = DateTime.UtcNow };

        _repoMock.Setup(r => r.FindByIdAsync(item.Id)).ReturnsAsync(item);

        var result = await _service.GetByIdAsync(item.Id);

        Assert.NotNull(result);
        Assert.Equal(item.Id, result.Id);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenNotExists()
    {
        _repoMock.Setup(r => r.FindByIdAsync(It.IsAny<long>())).ReturnsAsync((SessaoPlenariaJurispudencia?)null!);

        var result = await _service.GetByIdAsync(999);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetBySlugAsync_ShouldReturnJurisprudencia_WhenExists()
    {
        var item = new SessaoPlenariaJurispudencia { Id = 1, Slug = "slug-1", Titulo = "Título 1", Descricao = "Desc 1", DataCriacao = DateTime.UtcNow };

        _repoMock.Setup(r => r.FindBySlugAsync(item.Slug)).ReturnsAsync(item);

        var result = await _service.GetBySlugAsync(item.Slug);

        Assert.NotNull(result);
        Assert.Equal(item.Slug, result.Slug);
    }

    [Fact]
    public async Task GetBySlugAsync_ShouldReturnNull_WhenNotExists()
    {
        _repoMock.Setup(r => r.FindBySlugAsync(It.IsAny<string>())).ReturnsAsync((SessaoPlenariaJurispudencia?)null);

        var result = await _service.GetBySlugAsync("slug-inexistente");

        Assert.Null(result);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnId()
    {
        var request = new SessaoPlenariaJurisprudenciaCreateRequest
        {
            Slug = "slug-1",
            Titulo = "Título 1",
            Descricao = "Desc 1"
        };

        _repoMock.Setup(r => r.AnyAsync(It.IsAny<System.Linq.Expressions.Expression<Func<SessaoPlenariaJurispudencia, bool>>>())).ReturnsAsync(false);
        _repoMock.Setup(r => r.InsertAsync(It.IsAny<SessaoPlenariaJurispudencia>())).Returns(Task.CompletedTask);
        _repoMock.Setup(r => r.CommitAsync()).ReturnsAsync(true);

        var result = await _service.CreateAsync(request);

        Assert.True(result >= 0);
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowException_WhenSlugExists()
    {
        var request = new SessaoPlenariaJurisprudenciaCreateRequest
        {
            Slug = "slug-1",
            Titulo = "Título 1",
            Descricao = "Desc 1"
        };

        _repoMock.Setup(r => r.AnyAsync(It.IsAny<System.Linq.Expressions.Expression<Func<SessaoPlenariaJurispudencia, bool>>>())).ReturnsAsync(true);

        await Assert.ThrowsAsync<InvalidOperationException>(() => _service.CreateAsync(request));
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnTrue_WhenJurisprudenciaExists()
    {
        var item = new SessaoPlenariaJurispudencia { Id = 1, Slug = "slug-1", Titulo = "Título 1", Descricao = "Desc 1", DataCriacao = DateTime.UtcNow };
        var request = new SessaoPlenariaJurisprudenciaUpdateRequest
        {
            Slug = "slug-1",
            Titulo = "Título 1",
            Descricao = "Desc 1"
        };

        _repoMock.Setup(r => r.FindByIdAsync(item.Id)).ReturnsAsync(item);
        _repoMock.Setup(r => r.AnyAsync(It.IsAny<System.Linq.Expressions.Expression<Func<SessaoPlenariaJurispudencia, bool>>>())).ReturnsAsync(false);
        _repoMock.Setup(r => r.UpdateAsync(It.IsAny<SessaoPlenariaJurispudencia>())).Returns(Task.CompletedTask);
        _repoMock.Setup(r => r.CommitAsync()).ReturnsAsync(true);

        var result = await _service.UpdateAsync(item.Id, request);

        Assert.True(result);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnFalse_WhenJurisprudenciaNotExists()
    {
        var request = new SessaoPlenariaJurisprudenciaUpdateRequest
        {
            Slug = "slug-1",
            Titulo = "Título 1",
            Descricao = "Desc 1"
        };

        _repoMock.Setup(r => r.FindByIdAsync(It.IsAny<long>())).ReturnsAsync((SessaoPlenariaJurispudencia?)null!);

        var result = await _service.UpdateAsync(999, request);

        Assert.False(result);
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrowException_WhenSlugExistsForOther()
    {
        var item = new SessaoPlenariaJurispudencia { Id = 1, Slug = "slug-2", Titulo = "Título 2", Descricao = "Desc 2", DataCriacao = DateTime.UtcNow };
        var request = new SessaoPlenariaJurisprudenciaUpdateRequest
        {
            Slug = "slug-1",
            Titulo = "Título 1",
            Descricao = "Desc 1"
        };

        _repoMock.Setup(r => r.FindByIdAsync(item.Id)).ReturnsAsync(item);
        _repoMock.Setup(r => r.AnyAsync(It.IsAny<System.Linq.Expressions.Expression<Func<SessaoPlenariaJurispudencia, bool>>>())).ReturnsAsync(true);

        await Assert.ThrowsAsync<InvalidOperationException>(() => _service.UpdateAsync(item.Id, request));
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnTrue_WhenJurisprudenciaExists()
    {
        var item = new SessaoPlenariaJurispudencia { Id = 1, Slug = "slug-1", Titulo = "Título 1", Descricao = "Desc 1", DataCriacao = DateTime.UtcNow };

        _repoMock.Setup(r => r.FindByIdAsync(item.Id)).ReturnsAsync(item);
        _repoMock.Setup(r => r.DeleteAsync(item)).Returns(Task.CompletedTask);
        _repoMock.Setup(r => r.CommitAsync()).ReturnsAsync(true);

        var result = await _service.DeleteAsync(item.Id);

        Assert.True(result);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnFalse_WhenJurisprudenciaNotExists()
    {
        _repoMock.Setup(r => r.FindByIdAsync(It.IsAny<long>())).ReturnsAsync((SessaoPlenariaJurispudencia?)null!);

        var result = await _service.DeleteAsync(999);

        Assert.False(result);
    }
}