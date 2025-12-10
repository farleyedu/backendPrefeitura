using PortalTCMSP.Domain.Entities.BaseEntity;
using PortalTCMSP.Domain.Enum;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.Entities.SessaoPlenariaEntity
{
    [ExcludeFromCodeCoverage]
    public class SessaoPlenariaAta : Entity
    {
        public long? IdSessaoPlenaria { get; set; }
        public string Numero { get; set; } = string.Empty;
        public AtaTipo Tipo { get; set; }
        public DateTime? DataPublicacao { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataAtualizacao { get; set; }
        public ICollection<SessaoPlenariaAtaAnexo> Anexos { get; set; } = [];
        public SessaoPlenaria? SessaoPlenaria { get; set; }
    }
}
