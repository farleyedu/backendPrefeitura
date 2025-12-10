using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Requests.Fiscalizacao.RelatorioFiscalizacao
{
    [ExcludeFromCodeCoverage]
    public sealed class FiscalizacaoRelatorioFiscalizacaoConteudoCarrosselRequest
    {
        public int Ordem { get; set; }
        public bool Ativo { get; set; } = true;
        public string? Descricao { get; set; }
        public string? Link { get; set; }
        public FiscalizacaoRelatorioFiscalizacaoConteudoLinkRequest? ConteudoLink { get; set; }
    }
}
