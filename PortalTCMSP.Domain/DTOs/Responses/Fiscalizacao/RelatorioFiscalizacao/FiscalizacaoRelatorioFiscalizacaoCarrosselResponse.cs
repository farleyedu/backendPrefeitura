using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Responses.Fiscalizacao.RelatorioFiscalizacao
{
    [ExcludeFromCodeCoverage]
    public sealed class FiscalizacaoRelatorioFiscalizacaoCarrosselResponse
    {
        public long Id { get; set; }
        public bool Ativo { get; set; }
        public int Ordem { get; set; }
        public string? Titulo { get; set; }
        public List<FiscalizacaoRelatorioFiscalizacaoConteudoCarrosselResponse> ConteudoCarrocel { get; set; } = new();
    }
}
