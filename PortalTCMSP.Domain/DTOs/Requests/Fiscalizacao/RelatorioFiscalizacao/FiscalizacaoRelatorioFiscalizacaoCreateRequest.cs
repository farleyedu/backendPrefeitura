using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Requests.Fiscalizacao.RelatorioFiscalizacao
{
    [ExcludeFromCodeCoverage]
    public sealed class FiscalizacaoRelatorioFiscalizacaoCreateRequest
    {
        public string Slug { get; set; } = string.Empty;
        public string Titulo { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public bool Ativo { get; set; } = true;
        public List<FiscalizacaoRelatorioFiscalizacaoCarrosselRequest>? Carrocel { get; set; }
    }
}
