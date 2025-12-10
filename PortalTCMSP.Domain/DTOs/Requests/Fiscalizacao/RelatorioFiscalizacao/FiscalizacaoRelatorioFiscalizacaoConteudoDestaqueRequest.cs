using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Requests.Fiscalizacao.RelatorioFiscalizacao
{
    [ExcludeFromCodeCoverage]
    public sealed class FiscalizacaoRelatorioFiscalizacaoConteudoDestaqueRequest
    {
        public string? Titulo { get; set; }
        public List<FiscalizacaoRelatorioFiscalizacaoDescricaoRequest>? Descricoes { get; set; }
        public string? ImagemUrl { get; set; }
    }
}
