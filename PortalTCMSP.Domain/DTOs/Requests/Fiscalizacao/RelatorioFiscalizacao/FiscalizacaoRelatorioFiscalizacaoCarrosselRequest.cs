using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Requests.Fiscalizacao.RelatorioFiscalizacao
{
    [ExcludeFromCodeCoverage]
    public sealed class FiscalizacaoRelatorioFiscalizacaoCarrosselRequest
    {
        public bool Ativo { get; set; } = true;
        public int Ordem { get; set; }
        public string? Titulo { get; set; }
        public List<FiscalizacaoRelatorioFiscalizacaoConteudoCarrosselRequest>? ConteudoCarrocel { get; set; }
    }
}
