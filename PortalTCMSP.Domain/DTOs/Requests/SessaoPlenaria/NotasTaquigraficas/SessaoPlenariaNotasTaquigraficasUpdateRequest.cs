using PortalTCMSP.Domain.Enum;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Requests.SessaoPlenaria.NotasTaquigraficas
{
    [ExcludeFromCodeCoverage]
    public sealed class SessaoPlenariaNotasTaquigraficasUpdateRequest
    {
        public long? IdSessaoPlenaria { get; set; }
        public string? Numero { get; set; } = string.Empty;
        public NotasTipo? Tipo { get; set; }
        public DateTime? DataPublicacao { get; set; }
        public List<SessaoPlenariaNotasTaquigraficasAnexoItemRequest>? Anexos { get; set; }
    }
}
