using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Responses.Fiscalizacao.RelatorioFiscalizacao
{
    [ExcludeFromCodeCoverage]
    public sealed class FiscalizacaoRelatorioFiscalizacaoConteudoLinkResponse
    {
        public long Id { get; set; }
        public string? Titulo { get; set; }
        public DateTime? DataPublicacao { get; set; }
        public string? ConselheiroRelator { get; set; }
        public string? ImagemUrl { get; set; }
        public bool PosicionamentoAEsquerda { get; set; }
        public string? Descritivo { get; set; }
        public string? PeriodoRealizacao { get; set; }
        public string? PeriodoAbrangencia { get; set; }
        public string? TituloDestaque { get; set; }
        public List<FiscalizacaoRelatorioFiscalizacaoConteudoDestaqueResponse> ConteudoDestaque { get; set; } = [];
        public FiscalizacaoRelatorioFiscalizacaoTcRelacionadosResponse? TcRelacionados { get; set; }
        public FiscalizacaoRelatorioFiscalizacaoAnexosResponse? Anexos { get; set; }
    }
}
