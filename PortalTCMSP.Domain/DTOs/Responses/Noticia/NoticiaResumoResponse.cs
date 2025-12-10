using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Responses.Noticia
{
    [ExcludeFromCodeCoverage]
    public class NoticiaResumoResponse
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public DateTime DataPublicacao { get; set; }
        public string Autor { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
        public int Visualizacao { get; set; }
    }
}
