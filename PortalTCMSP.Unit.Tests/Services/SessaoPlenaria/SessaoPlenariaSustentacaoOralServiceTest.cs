using Moq;
using PortalTCMSP.Domain.DTOs.Requests.SessaoPlenaria.SustentacaoOral;
using PortalTCMSP.Domain.Entities.SessaoPlenariaEntity;
using PortalTCMSP.Domain.Repositories.SessaoPlenaria;
using PortalTCMSP.Infra.Services.SessaoPlenaria;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Unit.Tests.Services.SessaoPlenariaServiceTest;

[ExcludeFromCodeCoverage]
public class SessaoPlenariaSustentacaoOralServiceTest
{
    private readonly SessaoPlenariaSustentacaoOralService _service;
    private readonly Mock<ISessaoPlenariaSustentacaoOralRepository> _repoMock;

    public SessaoPlenariaSustentacaoOralServiceTest()
    {
        _repoMock = new Mock<ISessaoPlenariaSustentacaoOralRepository>();
        _service = new SessaoPlenariaSustentacaoOralService(_repoMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllSustentacoes()
    {
        var items = new List<SessaoPlenariaSustentacaoOral>
        {
            new SessaoPlenariaSustentacaoOral { Id = 1, Slug = "slug-1", Titulo = "Sustentação 1", Descricao = "Desc 1", DataCriacao = DateTime.UtcNow, Ativo = true },
            new SessaoPlenariaSustentacaoOral { Id = 2, Slug = "slug-2", Titulo = "Sustentação 2", Descricao = "Desc 2", DataCriacao = DateTime.UtcNow, Ativo = false }
        };

        _repoMock.Setup(r => r.AllWithAnexosAsync()).ReturnsAsync(items);

        var result = await _service.GetAllAsync();

        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnSustentacao_WhenExists()
    {
        var item = new SessaoPlenariaSustentacaoOral { Id = 1, Slug = "slug-1", Titulo = "Sustentação 1", Descricao = "Desc 1", DataCriacao = DateTime.UtcNow, Ativo = true };

        _repoMock.Setup(r => r.GetWithAnexosByIdAsync(item.Id)).ReturnsAsync(item);

        var result = await _service.GetByIdAsync(item.Id);

        Assert.NotNull(result);
        Assert.Equal(item.Id, result.Id);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenNotExists()
    {
        _repoMock.Setup(r => r.GetWithAnexosByIdAsync(It.IsAny<long>())).ReturnsAsync((SessaoPlenariaSustentacaoOral?)null);

        var result = await _service.GetByIdAsync(999);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetBySlugAsync_ShouldReturnSustentacao_WhenExists()
    {
        var item = new SessaoPlenariaSustentacaoOral { Id = 1, Slug = "slug-1", Titulo = "Sustentação 1", Descricao = "Desc 1", DataCriacao = DateTime.UtcNow, Ativo = true };

        _repoMock.Setup(r => r.GetWithAnexosBySlugAsync(item.Slug)).ReturnsAsync(item);

        var result = await _service.GetBySlugAsync(item.Slug);

        Assert.NotNull(result);
        Assert.Equal(item.Slug, result.Slug);
    }

    [Fact]
    public async Task GetBySlugAsync_ShouldReturnNull_WhenNotExists()
    {
        _repoMock.Setup(r => r.GetWithAnexosBySlugAsync(It.IsAny<string>())).ReturnsAsync((SessaoPlenariaSustentacaoOral?)null);

        var result = await _service.GetBySlugAsync("slug-inexistente");

        Assert.Null(result);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnId()
    {
        var request = new SessaoPlenariaSustentacaoOralCreateRequest
        {
            Slug = "slug-1",
            Titulo = "Sustentação 1",
            Descricao = "Desc 1",
            Ativo = true,
            Anexos = new List<SessaoPlenariaSustentacaoOralAnexoItemRequest>
            {
                new SessaoPlenariaSustentacaoOralAnexoItemRequest { Ordem = "1", Titulo = "Anexo 1", Descricao = "Desc Anexo 1" }
            }
        };

        _repoMock.Setup(r => r.AnyAsync(It.IsAny<System.Linq.Expressions.Expression<Func<SessaoPlenariaSustentacaoOral, bool>>>())).ReturnsAsync(false);
        _repoMock.Setup(r => r.InsertAsync(It.IsAny<SessaoPlenariaSustentacaoOral>())).Returns(Task.CompletedTask);
        _repoMock.Setup(r => r.CommitAsync()).ReturnsAsync(true);

        var result = await _service.CreateAsync(request);

        Assert.True(result >= 0);
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowException_WhenSlugExists()
    {
        var request = new SessaoPlenariaSustentacaoOralCreateRequest
        {
            Slug = "slug-1",
            Titulo = "Sustentação 1",
            Descricao = "Desc 1",
            Ativo = true,
            Anexos = new List<SessaoPlenariaSustentacaoOralAnexoItemRequest>()
        };

        _repoMock.Setup(r => r.AnyAsync(It.IsAny<System.Linq.Expressions.Expression<Func<SessaoPlenariaSustentacaoOral, bool>>>())).ReturnsAsync(true);

        await Assert.ThrowsAsync<InvalidOperationException>(() => _service.CreateAsync(request));
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnTrue_WhenSustentacaoExists()
    {
        var item = new SessaoPlenariaSustentacaoOral { Id = 1, Slug = "slug-1", Titulo = "Sustentação 1", Descricao = "Desc 1", DataCriacao = DateTime.UtcNow, Ativo = true };
        var request = new SessaoPlenariaSustentacaoOralUpdateRequest
        {
            Slug = "slug-1",
            Titulo = "Sustentação 1",
            Descricao = "Desc 1",
            Ativo = true,
            Anexos = new List<SessaoPlenariaSustentacaoOralAnexoItemRequest>
            {
                new SessaoPlenariaSustentacaoOralAnexoItemRequest { Ordem = "1", Titulo = "Anexo 1", Descricao = "Desc Anexo 1" }
            }
        };

        _repoMock.Setup(r => r.GetWithAnexosByIdAsync(item.Id)).ReturnsAsync(item);
        _repoMock.Setup(r => r.AnyAsync(It.IsAny<System.Linq.Expressions.Expression<Func<SessaoPlenariaSustentacaoOral, bool>>>())).ReturnsAsync(false);
        _repoMock.Setup(r => r.ReplaceAnexosAsync(item.Id, It.IsAny<IEnumerable<SessaoPlenariaSustentacaoOralAnexos>>())).Returns(Task.CompletedTask);
        _repoMock.Setup(r => r.UpdateAsync(It.IsAny<SessaoPlenariaSustentacaoOral>())).Returns(Task.CompletedTask);
        _repoMock.Setup(r => r.CommitAsync()).ReturnsAsync(true);

        var result = await _service.UpdateAsync(item.Id, request);

        Assert.True(result);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnFalse_WhenSustentacaoNotExists()
    {
        var request = new SessaoPlenariaSustentacaoOralUpdateRequest
        {
            Slug = "slug-1",
            Titulo = "Sustentação 1",
            Descricao = "Desc 1",
            Ativo = true,
            Anexos = new List<SessaoPlenariaSustentacaoOralAnexoItemRequest>()
        };

        _repoMock.Setup(r => r.GetWithAnexosByIdAsync(It.IsAny<long>())).ReturnsAsync((SessaoPlenariaSustentacaoOral?)null);

        var result = await _service.UpdateAsync(999, request);

        Assert.False(result);
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrowException_WhenSlugExistsForOther()
    {
        var item = new SessaoPlenariaSustentacaoOral { Id = 1, Slug = "slug-2", Titulo = "Sustentação 2", Descricao = "Desc 2", DataCriacao = DateTime.UtcNow, Ativo = false };
        var request = new SessaoPlenariaSustentacaoOralUpdateRequest
        {
            Slug = "slug-1",
            Titulo = "Sustentação 1",
            Descricao = "Desc 1",
            Ativo = true,
            Anexos = new List<SessaoPlenariaSustentacaoOralAnexoItemRequest>()
        };

        _repoMock.Setup(r => r.GetWithAnexosByIdAsync(item.Id)).ReturnsAsync(item);
        _repoMock.Setup(r => r.AnyAsync(It.IsAny<System.Linq.Expressions.Expression<Func<SessaoPlenariaSustentacaoOral, bool>>>())).ReturnsAsync(true);

        await Assert.ThrowsAsync<InvalidOperationException>(() => _service.UpdateAsync(item.Id, request));
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnTrue_WhenSustentacaoExists()
    {
        var item = new SessaoPlenariaSustentacaoOral { Id = 1, Slug = "slug-1", Titulo = "Sustentação 1", Descricao = "Desc 1", DataCriacao = DateTime.UtcNow, Ativo = true };

        _repoMock.Setup(r => r.FindByIdAsync(item.Id)).ReturnsAsync(item);
        _repoMock.Setup(r => r.DeleteAsync(item)).Returns(Task.CompletedTask);
        _repoMock.Setup(r => r.CommitAsync()).ReturnsAsync(true);

        var result = await _service.DeleteAsync(item.Id);

        Assert.True(result);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnFalse_WhenSustentacaoNotExists()
    {
        _repoMock.Setup(r => r.FindByIdAsync(It.IsAny<long>())).ReturnsAsync((SessaoPlenariaSustentacaoOral?)null!);

        var result = await _service.DeleteAsync(999);

        Assert.False(result);
    }
}