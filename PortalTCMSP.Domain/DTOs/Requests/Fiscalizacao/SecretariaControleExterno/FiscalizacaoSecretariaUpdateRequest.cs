using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Requests.Fiscalizacao.SecretariaControleExterno
{
    [ExcludeFromCodeCoverage]
    public sealed class FiscalizacaoSecretariaUpdateRequest
    {
        public string Slug { get; set; } = string.Empty;
        public string Titulo { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public string? Creditos { get; set; }
        public bool Ativo { get; set; }
        public List<FiscalizacaoSecretariaTituloItemRequest>? Titulos { get; set; }
        public List<FiscalizacaoSecretariaCarrosselItemRequest>? Carrossel { get; set; }
    }
}
