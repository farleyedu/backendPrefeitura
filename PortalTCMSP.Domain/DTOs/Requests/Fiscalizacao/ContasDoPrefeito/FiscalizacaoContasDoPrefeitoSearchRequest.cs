using PortalTCMSP.Domain.DTOs.Requests.Base;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Requests.Fiscalizacao.ContasDoPrefeito
{
    [ExcludeFromCodeCoverage]
    public sealed class FiscalizacaoContasDoPrefeitoSearchRequest : BaseConsultaPaginada
    {
        public string? Ano { get; set; }
        public string? Pauta { get; set; }
        public DateTime? SessaoDe { get; set; }
        public DateTime? SessaoAte { get; set; }
        public DateTime? PublicadaDe { get; set; }
        public DateTime? PublicadaAte { get; set; }
        public string? Query { get; set; }
    }
}
