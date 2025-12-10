using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Requests.NoticiaRequest
{
    [ExcludeFromCodeCoverage]
    public class NoticiaBlocoRequest
    {
        public int Ordem { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public string Configuracao { get; set; } = string.Empty;
    }
}
