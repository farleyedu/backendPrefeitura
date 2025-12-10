using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Responses.Fiscalizacao.RelatorioFiscalizacao
{
    [ExcludeFromCodeCoverage]
    public sealed class FiscalizacaoRelatorioFiscalizacaoConteudoCarrosselResponse
    {
        public long Id { get; set; }
        public int Ordem { get; set; }
        public bool Ativo { get; set; }
        public string? Descricao { get; set; }
        public string? Link { get; set; }
        public FiscalizacaoRelatorioFiscalizacaoConteudoLinkResponse? ConteudoLink { get; set; }
    }
}
