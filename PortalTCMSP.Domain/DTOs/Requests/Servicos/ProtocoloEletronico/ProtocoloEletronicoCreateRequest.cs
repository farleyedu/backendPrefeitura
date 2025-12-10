using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Requests.Servicos.ProtocoloEletronico
{
    [ExcludeFromCodeCoverage]
    public sealed class ProtocoloEletronicoCreateRequest
    {
        public string TituloPagina { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public bool Ativo { get; set; } = true;
        public List<ProtocoloEletronicoAcaoRequest>? Acoes { get; set; }
    }
}
