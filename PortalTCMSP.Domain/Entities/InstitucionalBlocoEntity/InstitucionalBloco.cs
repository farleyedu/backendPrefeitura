using PortalTCMSP.Domain.Entities.BaseEntity;
using PortalTCMSP.Domain.Entities.InstitucionalEntity;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.Entities.BlocoEntity
{
    [ExcludeFromCodeCoverage]
    public class InstitucionalBloco : Entity
    {
        public long IdInstitucional { get; set; }  
        public Institucional Institucional { get; set; } = default!; 
        public int Ordem { get; set; }
        public string? Html { get; set; }
        public string? Titulo { get; set; }
        public string? Subtitulo { get; set; }
        public string Ativo { get; set; } = "S";
        public ICollection<InstitucionalBlocoDescricao> Descricoes { get; set; } = [];
        public ICollection<InstitucionalBlocoAnexo> Anexos { get; set; } = [];
    }
}
