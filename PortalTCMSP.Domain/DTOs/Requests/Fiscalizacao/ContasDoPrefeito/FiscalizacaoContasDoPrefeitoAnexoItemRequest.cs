using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Requests.Fiscalizacao.ContasDoPrefeito
{
    [ExcludeFromCodeCoverage]
    public sealed class FiscalizacaoContasDoPrefeitoAnexoItemRequest
    {
        [Required] public string Link { get; set; } = string.Empty;
        public string? TipoArquivo { get; set; }
        public string? NomeExibicao { get; set; }
        public int Ordem { get; set; } = 0;
    }
}
