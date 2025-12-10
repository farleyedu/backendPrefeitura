using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Responses.Noticia
{
    [ExcludeFromCodeCoverage]
    public class ListaNoticiaResponse
    {
        public int Total { get; set; }
        public int Pagina { get; set; }
        public int Tamanho { get; set; }
        public List<NoticiaResumoResponse> Itens { get; set; } = [];
    }
}
