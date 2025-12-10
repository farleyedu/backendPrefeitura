using PortalTCMSP.Domain.Enum;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Requests.SessaoPlenaria.Ata
{
    [ExcludeFromCodeCoverage]
    public sealed class SessaoPlenariaAtaUpdateRequest
    {
        public long? IdSessaoPlenaria { get; set; }
        public string? Numero { get; set; } = string.Empty;
        public AtaTipo? Tipo { get; set; }
        public DateTime? DataPublicacao { get; set; }
        public List<SessaoPlenariaAtaAnexoItemRequest>? Anexos { get; set; } 
    }
}
