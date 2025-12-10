using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Requests.Fiscalizacao.RelatorioFiscalizacao
{
    [ExcludeFromCodeCoverage]
    public sealed class FiscalizacaoRelatorioFiscalizacaoConteudoLinkRequest
    {
        public string? Titulo { get; set; }
        public DateTime? DataPublicacao { get; set; }
        public string? ConselheiroRelator { get; set; }
        public string? ImagemUrl { get; set; }
        public bool PosicionamentoAEsquerda { get; set; }
        public string? Descritivo { get; set; }
        public string? PeriodoRealizacao { get; set; }
        public string? PeriodoAbrangencia { get; set; }
        public string? TituloDestaque { get; set; }
        public List<FiscalizacaoRelatorioFiscalizacaoConteudoDestaqueRequest>? ConteudoDestaque { get; set; }
        public FiscalizacaoRelatorioFiscalizacaoTcRelacionadosRequest? TcRelacionados { get; set; }
        public FiscalizacaoRelatorioFiscalizacaoAnexosRequest? Anexos { get; set; }
    }
}
