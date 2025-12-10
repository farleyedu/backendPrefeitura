using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Requests.Fiscalizacao.SecretariaControleExterno
{
    [ExcludeFromCodeCoverage]
    public sealed class FiscalizacaoSecretariaCarrosselItemRequest
    {
        public int Ordem { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public string ImagemUrl { get; set; } = string.Empty;
    }
}
