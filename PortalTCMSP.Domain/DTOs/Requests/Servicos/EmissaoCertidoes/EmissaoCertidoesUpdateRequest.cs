using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Requests.Servicos.EmissaoCertidoes
{
    [ExcludeFromCodeCoverage]
    public sealed class EmissaoCertidoesUpdateRequest
    {
        public string TituloPagina { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public bool Ativo { get; set; } = true;
        public List<EmissaoCertidoesAcaoRequest>? Acoes { get; set; }
        public List<EmissaoCertidoesSecaoOrientacaoRequest>? SecaoOrientacoes { get; set; }
    }
}
