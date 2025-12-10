using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Responses.Fiscalizacao.SecretariaControleExterno
{
    [ExcludeFromCodeCoverage]
    public sealed class FiscalizacaoSecretariaResponse
    {
        public long Id { get; set; }
        public string Slug { get; set; } = string.Empty;
        public string Titulo { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public string? Creditos { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataAtualizacao { get; set; }
        public List<FiscalizacaoSecretariaTituloItemResponse> Titulos { get; set; } = [];
        public List<FiscalizacaoSecretariaCarrosselItemResponse> Carrossel { get; set; } = [];
    }
}
