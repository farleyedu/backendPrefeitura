using PortalTCMSP.Domain.Entities.BaseEntity;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.Entities.SessaoPlenariaEntity
{
    [ExcludeFromCodeCoverage]
    public class SessaoPlenariaAtaAnexo : Entity
    {
        public long IdSessaoPlenariaAta { get; set; }
        public string Link { get; set; } = string.Empty;
        public string? TipoArquivo { get; set; }    
        public string? NomeExibicao { get; set; }
        public int Ordem { get; set; } = 0;
        public SessaoPlenariaAta? Ata { get; set; }
    }
}
