using PortalTCMSP.Domain.Entities.BaseEntity;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.Entities.ServicosEntity.MultasProcedimentosEntity
{
    [ExcludeFromCodeCoverage]
    public class MultasProcedimentosPortariaRelacionada : Entity
    {
        public long IdMultasProcedimentos { get; set; }
        public int Ordem { get; set; } = 0;
        public string Titulo { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;

        public MultasProcedimentos MultasProcedimentos { get; set; } = default!;
    }
}
