using Microsoft.AspNetCore.Hosting;
using PortalTCMSP.Domain.Services.Home;
using System.Text;

namespace PortalTCMSP.Infra.Services.Home
{
    public class HtmlService : IHtmlService
    {
        private readonly IWebHostEnvironment _env;

        public HtmlService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task<string> SalvarHtmlAsync(string slug, string conteudoHtml)
        {
            var pasta = Path.Combine(_env.WebRootPath ?? "wwwroot", "paginas");
            if (!Directory.Exists(pasta))
                Directory.CreateDirectory(pasta);

            // busca arquivos existentes com padrão slug*.html
            var arquivosExistentes = Directory.GetFiles(pasta, $"{slug}*.html");
            var proximaVersao = arquivosExistentes.Length + 1;

            var nomeArquivo = proximaVersao == 1
                ? $"{slug}.html"
                : $"{slug}_v{proximaVersao}.html";

            var caminhoCompleto = Path.Combine(pasta, nomeArquivo);
            await File.WriteAllTextAsync(caminhoCompleto, conteudoHtml, Encoding.UTF8);

            return $"/paginas/{nomeArquivo}";
        }

        public async Task<string> LerHtmlAsync(string relativePath)
        {
            var fullPath = Path.Combine(_env.WebRootPath ?? "wwwroot", relativePath.TrimStart('/'));
            return File.Exists(fullPath) ? await File.ReadAllTextAsync(fullPath) : string.Empty;
        }

        public List<string> ListarVersoes(string slug)
        {
            var pasta = Path.Combine(_env.WebRootPath ?? "wwwroot", "paginas");
            if (!Directory.Exists(pasta)) return new();

            return Directory.EnumerateFiles(pasta, $"{slug}*.html")
                .OrderBy(f => f, StringComparer.OrdinalIgnoreCase)
                .Select(f => "/paginas/" + Path.GetFileName(f))
                .ToList();
        }

        public async Task<string> RestaurarVersaoAsync(string slug, string nomeArquivo)
        {
            var pasta = Path.Combine(_env.WebRootPath ?? "wwwroot", "paginas");
            var origem = Path.Combine(pasta, nomeArquivo);

            if (!File.Exists(origem))
                throw new FileNotFoundException("Versão solicitada não encontrada.");

            // criar nova versão a partir da existente
            var conteudo = await File.ReadAllTextAsync(origem);
            return await SalvarHtmlAsync(slug, conteudo);
        }
    }
}
