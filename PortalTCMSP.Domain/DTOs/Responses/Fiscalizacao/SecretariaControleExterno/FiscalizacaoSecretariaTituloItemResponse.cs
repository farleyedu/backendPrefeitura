using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Responses.Fiscalizacao.SecretariaControleExterno
{
    [ExcludeFromCodeCoverage]
    public sealed class FiscalizacaoSecretariaTituloItemResponse
    {
        public long Id { get; set; }
        public int Ordem { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string ImagemUrl { get; set; } = string.Empty;
        public string? Descricao { get; set; }
    }
}
