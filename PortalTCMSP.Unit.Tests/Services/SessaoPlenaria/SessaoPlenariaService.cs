using Moq;
using PortalTCMSP.Domain.DTOs.Requests.SessaoPlenaria.Base;
using PortalTCMSP.Domain.Entities.SessaoPlenariaEntity;
using PortalTCMSP.Domain.Repositories.SessaoPlenaria;
using PortalTCMSP.Infra.Services.SessaoPlenaria;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Unit.Tests.Services.SessaoPlenariaServiceTest;

[ExcludeFromCodeCoverage]
public class SessaoPlenariaServiceTest
{
    private readonly SessaoPlenariaService _service;
    private readonly Mock<ISessaoPlenariaRepository> _repoMock;

    public SessaoPlenariaServiceTest()
    {
        _repoMock = new Mock<ISessaoPlenariaRepository>();
        _service = new SessaoPlenariaService(_repoMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllSessoes()
    {
        var sessoes = new List<SessaoPlenaria>
        {
            new SessaoPlenaria { Id = 1, Slug = "sessao-1", Titulo = "Sessão 1", DataCriacao = DateTime.UtcNow, Ativo = "S" },
            new SessaoPlenaria { Id = 2, Slug = "sessao-2", Titulo = "Sessão 2", DataCriacao = DateTime.UtcNow, Ativo = "N" }
        };

        _repoMock.Setup(r => r.AllWithChildrenAsync()).ReturnsAsync(sessoes);

        var result = await _service.GetAllAsync();

        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnSessao_WhenExists()
    {
        var sessao = new SessaoPlenaria { Id = 1, Slug = "sessao-1", Titulo = "Sessão 1", DataCriacao = DateTime.UtcNow, Ativo = "S" };

        _repoMock.Setup(r => r.GetWithChildrenByIdAsync(sessao.Id)).ReturnsAsync(sessao);

        var result = await _service.GetByIdAsync(sessao.Id);

        Assert.NotNull(result);
        Assert.Equal(sessao.Id, result.Id);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenNotExists()
    {
        _repoMock.Setup(r => r.GetWithChildrenByIdAsync(It.IsAny<long>())).ReturnsAsync((SessaoPlenaria?)null);

        var result = await _service.GetByIdAsync(999);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetBySlugAsync_ShouldReturnSessao_WhenExists()
    {
        var sessao = new SessaoPlenaria { Id = 1, Slug = "sessao-1", Titulo = "Sessão 1", DataCriacao = DateTime.UtcNow, Ativo = "S" };

        _repoMock.Setup(r => r.GetWithChildrenBySlugAsync(sessao.Slug)).ReturnsAsync(sessao);

        var result = await _service.GetBySlugAsync(sessao.Slug);

        Assert.NotNull(result);
        Assert.Equal(sessao.Slug, result.Slug);
    }

    [Fact]
    public async Task GetBySlugAsync_ShouldReturnNull_WhenNotExists()
    {
        _repoMock.Setup(r => r.GetWithChildrenBySlugAsync(It.IsAny<string>())).ReturnsAsync((SessaoPlenaria?)null);

        var result = await _service.GetBySlugAsync("slug-inexistente");

        Assert.Null(result);
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnId()
    {
        var request = new SessaoPlenariaCreateRequest
        {
            Slug = "sessao-1",
            Titulo = "Sessão 1",
            Descricao = "Descrição",
            DataPublicacao = DateTime.UtcNow,
            Ativo = true
        };

        _repoMock.Setup(r => r.AnyAsync(It.IsAny<System.Linq.Expressions.Expression<Func<SessaoPlenaria, bool>>>())).ReturnsAsync(false);
        _repoMock.Setup(r => r.DeactivateAllAsync()).Returns(Task.CompletedTask);
        _repoMock.Setup(r => r.InsertAsync(It.IsAny<SessaoPlenaria>())).Returns(Task.CompletedTask);
        _repoMock.Setup(r => r.CommitAsync()).ReturnsAsync(true);

        var result = await _service.CreateAsync(request);

        Assert.True(result >= 0);
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowException_WhenSlugExists()
    {
        var request = new SessaoPlenariaCreateRequest
        {
            Slug = "sessao-1",
            Titulo = "Sessão 1",
            Descricao = "Descrição",
            DataPublicacao = DateTime.UtcNow,
            Ativo = true
        };

        _repoMock.Setup(r => r.AnyAsync(It.IsAny<System.Linq.Expressions.Expression<Func<SessaoPlenaria, bool>>>())).ReturnsAsync(true);

        await Assert.ThrowsAsync<InvalidOperationException>(() => _service.CreateAsync(request));
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnTrue_WhenSessaoExists()
    {
        var sessao = new SessaoPlenaria { Id = 1, Slug = "sessao-1", Titulo = "Sessão 1", DataCriacao = DateTime.UtcNow, Ativo = "S" };
        var request = new SessaoPlenariaUpdateRequest
        {
            Slug = "sessao-1",
            Titulo = "Sessão 1",
            Descricao = "Descrição",
            DataPublicacao = DateTime.UtcNow,
            Ativo = true
        };

        _repoMock.Setup(r => r.GetWithChildrenByIdAsync(sessao.Id)).ReturnsAsync(sessao);
        _repoMock.Setup(r => r.AnyAsync(It.IsAny<System.Linq.Expressions.Expression<Func<SessaoPlenaria, bool>>>())).ReturnsAsync(false);
        _repoMock.Setup(r => r.UpdateAsync(It.IsAny<SessaoPlenaria>())).Returns(Task.CompletedTask);
        _repoMock.Setup(r => r.DeactivateAllExceptAsync(sessao.Id)).Returns(Task.CompletedTask);
        _repoMock.Setup(r => r.CommitAsync()).ReturnsAsync(true);

        var result = await _service.UpdateAsync(sessao.Id, request);

        Assert.True(result);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnFalse_WhenSessaoNotExists()
    {
        var request = new SessaoPlenariaUpdateRequest
        {
            Slug = "sessao-1",
            Titulo = "Sessão 1",
            Descricao = "Descrição",
            DataPublicacao = DateTime.UtcNow,
            Ativo = true
        };

        _repoMock.Setup(r => r.GetWithChildrenByIdAsync(It.IsAny<long>())).ReturnsAsync((SessaoPlenaria?)null);

        var result = await _service.UpdateAsync(999, request);

        Assert.False(result);
    }

    [Fact]
    public async Task UpdateAsync_ShouldThrowException_WhenSlugExistsForOther()
    {
        var sessao = new SessaoPlenaria { Id = 1, Slug = "sessao-2", Titulo = "Sessão 2", DataCriacao = DateTime.UtcNow, Ativo = "N" };
        var request = new SessaoPlenariaUpdateRequest
        {
            Slug = "sessao-1",
            Titulo = "Sessão 1",
            Descricao = "Descrição",
            DataPublicacao = DateTime.UtcNow,
            Ativo = true
        };

        _repoMock.Setup(r => r.GetWithChildrenByIdAsync(sessao.Id)).ReturnsAsync(sessao);
        _repoMock.Setup(r => r.AnyAsync(It.IsAny<System.Linq.Expressions.Expression<Func<SessaoPlenaria, bool>>>())).ReturnsAsync(true);

        await Assert.ThrowsAsync<InvalidOperationException>(() => _service.UpdateAsync(sessao.Id, request));
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnTrue_WhenSessaoExists()
    {
        var sessao = new SessaoPlenaria { Id = 1, Slug = "sessao-1", Titulo = "Sessão 1", DataCriacao = DateTime.UtcNow, Ativo = "S" };

        _repoMock.Setup(r => r.FindByIdAsync(sessao.Id)).ReturnsAsync(sessao);
        _repoMock.Setup(r => r.DeleteAsync(sessao)).Returns(Task.CompletedTask);
        _repoMock.Setup(r => r.CommitAsync()).ReturnsAsync(true);

        var result = await _service.DeleteAsync(sessao.Id);

        Assert.True(result);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnFalse_WhenSessaoNotExists()
    {
        _repoMock.Setup(r => r.FindByIdAsync(It.IsAny<long>())).ReturnsAsync((SessaoPlenaria?)null!);

        var result = await _service.DeleteAsync(999);

        Assert.False(result);
    }

    [Fact]
    public async Task GetAtivosAsync_ShouldReturnOnlyActiveSessoes()
    {
        var sessoes = new List<SessaoPlenaria>
        {
            new SessaoPlenaria { Id = 1,  Slug = "sessao-1", Titulo = "Sessão 1", DataCriacao = DateTime.UtcNow, Ativo = "S" },
            new SessaoPlenaria { Id = 2,  Slug = "sessao-2", Titulo = "Sessão 2", DataCriacao = DateTime.UtcNow, Ativo = "N" }
        };

        _repoMock.Setup(r => r.GetAtivosAsync()).ReturnsAsync(sessoes.Where(s => s.Ativo == "S").ToList());

        var result = await _service.GetAtivosAsync();

        Assert.NotNull(result);
        Assert.All(result, s => Assert.True(s.Ativo));
    }
}