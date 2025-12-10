using PortalTCMSP.Domain.Entities.BaseEntity;
using PortalTCMSP.Domain.Enum;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.Entities.SessaoPlenariaEntity
{
    [ExcludeFromCodeCoverage]
    public class SessaoPlenariaPauta : Entity
    {
        public long? IdSessaoPlenaria { get; set; }
        public string Numero { get; set; } = string.Empty;
        public PautaTipo? Tipo { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataDaSesao { get; set; }
        public DateTime? DataPublicacao { get; set; }
        public DateTime? DataAtualizacao { get; set; }
        public ICollection<SessaoPlenariaPautaAnexo> Anexos { get; set; } = new List<SessaoPlenariaPautaAnexo>();
        public SessaoPlenaria? SessaoPlenaria { get; set; }
    }
}
