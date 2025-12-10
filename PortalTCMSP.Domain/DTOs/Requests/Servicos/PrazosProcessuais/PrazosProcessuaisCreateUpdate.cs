using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Requests.Servicos.PrazosProcessuais
{
    [ExcludeFromCodeCoverage]
    public sealed class PrazosProcessuaisCreateUpdate
    {
        public long Id { get; set; }
        public string TituloPagina { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public bool Ativo { get; set; } = true;
        public List<PrazosProcessuaisItemRequest>? PrazosProcessuaisItens { get; set; }
    }
}
