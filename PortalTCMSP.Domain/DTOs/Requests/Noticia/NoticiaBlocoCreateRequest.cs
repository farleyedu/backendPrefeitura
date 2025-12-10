using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace PortalTCMSP.Domain.DTOs.Requests.NoticiaRequest
{
    [ExcludeFromCodeCoverage]
    public class NoticiaBlocoCreateRequest
    {
        public int Ordem { get; set; }
        public string Tipo { get; set; } = default!;
        public JsonElement? Config { get; set; }
        public JsonElement Valor { get; set; }
    }
}
