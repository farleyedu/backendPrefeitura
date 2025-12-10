using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Requests.Fiscalizacao.ContasDoPrefeito
{
    [ExcludeFromCodeCoverage]
    public sealed class FiscalizacaoContasDoPrefeitoUpdateRequest
    {
        [Required] public string Ano { get; set; } = string.Empty;
        [Required] public string Pauta { get; set; } = string.Empty;
        public DateTime? DataSessao { get; set; }
        public DateTime? DataPublicacao { get; set; }
        public List<FiscalizacaoContasDoPrefeitoAnexoItemRequest>? Anexos { get; set; }
    }
}
