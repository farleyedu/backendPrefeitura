using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Responses.Servicos.PrazosProcessuais
{
    [ExcludeFromCodeCoverage]
    public sealed class PrazosProcessuaisItemResponse
    {
        public long Id { get; set; }
        public bool Ativo { get; set; }
        public int Ordem { get; set; }
        public string Nome { get; set; } = string.Empty;
        public DateTime DataPublicacao { get; set; }
        public string TempoDecorrido { get; set; } = string.Empty;
        public List<PrazosProcessuaisItemAnexoResponse> Anexos { get; set; } = [];
    }
}
