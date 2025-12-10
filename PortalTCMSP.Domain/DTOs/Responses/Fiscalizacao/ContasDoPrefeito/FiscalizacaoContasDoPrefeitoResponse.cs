using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Responses.Fiscalizacao.ContasDoPrefeito
{
    [ExcludeFromCodeCoverage]
    public sealed class FiscalizacaoContasDoPrefeitoResponse
    {
        public long Id { get; set; }
        public string Ano { get; set; } = string.Empty;
        public string Pauta { get; set; } = string.Empty;
        public DateTime? DataSessao { get; set; }
        public DateTime? DataPublicacao { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataAtualizacao { get; set; }
        public List<FiscalizacaoContasDoPrefeitoAnexoItemResponse> Anexos { get; set; } = [];
    }
}
