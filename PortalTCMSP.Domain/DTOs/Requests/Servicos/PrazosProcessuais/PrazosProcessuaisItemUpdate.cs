using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Requests.Servicos.PrazosProcessuais
{
    [ExcludeFromCodeCoverage]
    public sealed class PrazosProcessuaisItemUpdate
    {
        public long Id { get; set; }
        public bool Ativo { get; set; } = true;
        public int Ordem { get; set; }
        public string Nome { get; set; } = string.Empty;
        public DateTime DataPublicacao { get; set; }
        public string TempoDecorrido { get; set; } = string.Empty;
        //public List<PrazosProcessuaisItemAnexoRequest>? Anexos { get; set; }
    }
}
