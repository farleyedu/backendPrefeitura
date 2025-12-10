using PortalTCMSP.Domain.Entities.BaseEntity;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.Entities.SessaoPlenariaEntity
{
    [ExcludeFromCodeCoverage]
    public class SessaoPlenariaSustentacaoOral : Entity
    {
        public string Slug { get; set; } = string.Empty;
        public string? Titulo { get; set; } = string.Empty;
        public string? Descricao { get; set; } = string.Empty;
        public bool Ativo { get; set; }
        public DateTime? DataCriacao { get; set; }
        public DateTime? DataAtualizacao { get; set; }
        public List<SessaoPlenariaSustentacaoOralAnexos> Anexos { get; set; } = new List<SessaoPlenariaSustentacaoOralAnexos>();
    }
}
