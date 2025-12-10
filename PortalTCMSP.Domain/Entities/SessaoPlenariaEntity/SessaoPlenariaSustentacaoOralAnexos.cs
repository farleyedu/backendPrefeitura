using PortalTCMSP.Domain.Entities.BaseEntity;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.Entities.SessaoPlenariaEntity
{
    [ExcludeFromCodeCoverage]
    public class SessaoPlenariaSustentacaoOralAnexos : Entity
    {
        public long IdSessaoPlenariaSustentacaoOral { get; set; }
        public string Ordem { get; set; } = string.Empty;
        public string Titulo { get; set; } = string.Empty;
        public string? Descricao { get; set; } = string.Empty;
        public SessaoPlenariaSustentacaoOral SustentacaoOral { get; set; } = default!;
    }
}
