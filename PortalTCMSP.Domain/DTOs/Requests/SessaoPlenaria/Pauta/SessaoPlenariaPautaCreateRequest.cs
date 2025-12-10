using PortalTCMSP.Domain.Enum;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Requests.SessaoPlenaria.Pauta
{
    [ExcludeFromCodeCoverage]
    public sealed class SessaoPlenariaPautaCreateRequest
    {
        public long? IdSessaoPlenaria { get; set; }
        public string Numero { get; set; } = string.Empty;
        public PautaTipo? Tipo { get; set; }
        public DateTime? DataDaSesao { get; set; }
        public DateTime? DataPublicacao { get; set; }
        public List<SessaoPlenariaPautaAnexoItemRequest> Anexos { get; set; } = new();
    }
}
