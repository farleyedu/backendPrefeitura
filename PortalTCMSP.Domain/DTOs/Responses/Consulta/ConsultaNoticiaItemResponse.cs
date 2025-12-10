using PortalTCMSP.Domain.DTOs.Responses.Noticia;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Responses.Consulta
{
    [ExcludeFromCodeCoverage]
    public class ConsultaNoticiaItemResponse
    {
        public long Id { get; set; }
        public string Slug { get; set; } = default!;
        public string Titulo { get; set; } = default!;
        public string? Resumo { get; set; }
        public DateTime PublicadoQuando { get; set; }
        public bool Destaque { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsOld { get; set; } 
        public List<CategoriaItem> Categorias { get; set; } = new();
    }
}
