namespace PortalTCMSP.Domain.Services.Home
{
    public interface IHtmlService
    {
        Task<string> SalvarHtmlAsync(string slug, string conteudoHtml);
        Task<string> LerHtmlAsync(string relativePath);
        List<string> ListarVersoes(string slug);
        Task<string> RestaurarVersaoAsync(string slug, string nomeArquivo);
    }
}
