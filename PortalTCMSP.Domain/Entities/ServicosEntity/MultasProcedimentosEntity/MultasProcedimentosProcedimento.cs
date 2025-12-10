using PortalTCMSP.Domain.Entities.BaseEntity;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.Entities.ServicosEntity.MultasProcedimentosEntity
{
    [ExcludeFromCodeCoverage]
    public class MultasProcedimentosProcedimento : Entity
    {
        public long IdMultasProcedimentos { get; set; }
        public int Ordem { get; set; } = 0;
        public string Texto { get; set; } = string.Empty;
        public string? UrlImagem { get; set; }

        public MultasProcedimentos MultasProcedimentos { get; set; } = default!;
    }
}
