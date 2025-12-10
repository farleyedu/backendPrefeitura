using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.Entities.NoticiaEntity
{
    [ExcludeFromCodeCoverage]
    public class Auditoria
    {
        public DateTime CriadoEm { get; set; } = DateTime.UtcNow;
        public string? CriadoPor { get; set; }
        public DateTime AtualizadoEm { get; set; } = DateTime.UtcNow;
        public string? AtualizadoPor { get; set; }
        public int Versao { get; set; } = 1;
    }
}
