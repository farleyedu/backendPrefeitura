using PortalTCMSP.Domain.Entities.BaseEntity;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.Entities.NoticiaEntity
{
    [ExcludeFromCodeCoverage]
    public class NoticiaBloco : Entity
    {
        public long NoticiaId { get; set; }
        public Noticia Noticia { get; set; } = default!;
        public int Ordem { get; set; }
        public string Tipo { get; set; } = default!;
        public string? ConfigJson { get; set; }
        public string ValorJson { get; set; } = default!;
        public DateTime CriadoEm { get; set; } = DateTime.UtcNow;
    }
}
