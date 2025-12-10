using Microsoft.AspNetCore.Hosting;
using Moq;
using PortalTCMSP.Infra.Services.Home;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace PortalTCMSP.Unit.Tests.Services.Home
{
    [ExcludeFromCodeCoverage]
    public class HtmlServiceTest : IDisposable
    {
        private readonly string _rootPath;
        private readonly HtmlService _service;

        public HtmlServiceTest()
        {
            _rootPath = Path.Combine(Path.GetTempPath(), "HtmlServiceTest_" + Path.GetRandomFileName());
            Directory.CreateDirectory(_rootPath);

            var envMock = new Mock<IWebHostEnvironment>();
            envMock.Setup(e => e.WebRootPath).Returns(_rootPath);

            _service = new HtmlService(envMock.Object);
        }

        public void Dispose()
        {
            if (Directory.Exists(_rootPath))
                Directory.Delete(_rootPath, true);
        }

        [Fact]
        public async Task SalvarHtmlAsync_DeveCriarArquivoEPathCorreto()
        {
            var slug = "testpagina";
            var conteudo = "<html>meu html</html>";

            var pathRetornado = await _service.SalvarHtmlAsync(slug, conteudo);

            // Verifica retorno
            Assert.Equal($"/paginas/{slug}.html", pathRetornado);

            // Verifica arquivo realmente criado
            var arquivoFisico = Path.Combine(_rootPath, "paginas", $"{slug}.html");
            Assert.True(File.Exists(arquivoFisico));
            var lido = await File.ReadAllTextAsync(arquivoFisico, Encoding.UTF8);
            Assert.Equal(conteudo, lido);
        }

        [Fact]
        public async Task SalvarHtmlAsync_DeveIncrementarVersaoSeArquivoJaExiste()
        {
            var slug = "pagina-versao";
            var conteudo1 = "primeira versão";
            var conteudo2 = "segunda versão";

            await _service.SalvarHtmlAsync(slug, conteudo1);
            var pathRetornado = await _service.SalvarHtmlAsync(slug, conteudo2);

            Assert.Equal($"/paginas/{slug}_v2.html", pathRetornado);

            var arquivo1 = Path.Combine(_rootPath, "paginas", $"{slug}.html");
            var arquivo2 = Path.Combine(_rootPath, "paginas", $"{slug}_v2.html");
            Assert.True(File.Exists(arquivo1));
            Assert.True(File.Exists(arquivo2));
        }

        [Fact]
        public async Task LerHtmlAsync_DeveRetornarConteudoDoArquivo()
        {
            var slug = "paginaler";
            var conteudo = "<html>ler</html>";
            var path = await _service.SalvarHtmlAsync(slug, conteudo);

            var textoLido = await _service.LerHtmlAsync(path);

            Assert.Equal(conteudo, textoLido);
        }

        [Fact]
        public async Task LerHtmlAsync_DeveRetornarVazioSeArquivoNaoExiste()
        {
            var textoLido = await _service.LerHtmlAsync("/paginas/inexistente.html");
            Assert.Equal(string.Empty, textoLido);
        }

        [Fact]
        public async Task ListarVersoesAsync_DeveRetornarListaDeArquivosPorSlug()
        {
            var slug = "lista";
            await _service.SalvarHtmlAsync(slug, "1");
            await _service.SalvarHtmlAsync(slug, "2");
            await _service.SalvarHtmlAsync(slug, "3");

            var lista = _service.ListarVersoes(slug);

            Assert.Equal(3, lista.Count);
            Assert.Contains($"/paginas/{slug}.html", lista);
            Assert.Contains($"/paginas/{slug}_v2.html", lista);
            Assert.Contains($"/paginas/{slug}_v3.html", lista);
        }

        [Fact]
        public void ListarVersoes_DeveRetornarListaVaziaSeNaoExiste()
        {
            var lista = _service.ListarVersoes("nada");
            Assert.Empty(lista);
        }

        [Fact]
        public async Task RestaurarVersaoAsync_DeveCriarNovaVersao()
        {
            var slug = "restaurar";
            await _service.SalvarHtmlAsync(slug, "original");
            var segunda = await _service.SalvarHtmlAsync(slug, "modificado");
            var arquivos = _service.ListarVersoes(slug);

            // Força restaurar a primeira versão
            var nomePrimeiroArquivo = Path.GetFileName(arquivos.First());
            var pathRestaurado = await _service.RestaurarVersaoAsync(slug, nomePrimeiroArquivo);

            Assert.EndsWith(".html", pathRestaurado);
            Assert.Contains("_v3.html", pathRestaurado); // terceira versão criada
        }

        [Fact]
        public async Task RestaurarVersaoAsync_DeveLancarSeArquivoNaoExiste()
        {
            await Assert.ThrowsAsync<FileNotFoundException>(() => _service.RestaurarVersaoAsync("slug", "inexistente.html"));
        }
    }
}
