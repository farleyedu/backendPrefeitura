using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Responses.Fiscalizacao.RelatorioFiscalizacao
{
    [ExcludeFromCodeCoverage]
    public sealed class FiscalizacaoRelatorioFiscalizacaoConteudoDestaqueResponse
    {
        public string? Titulo { get; set; }
        public List<FiscalizacaoRelatorioFiscalizacaoDescricaoResponse> Descricoes { get; set; } = [];
        public string? ImagemUrl { get; set; }
    }
}
