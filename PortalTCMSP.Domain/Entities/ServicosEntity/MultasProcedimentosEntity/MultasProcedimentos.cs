using PortalTCMSP.Domain.Entities.BaseEntity;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.Entities.ServicosEntity.MultasProcedimentosEntity
{
    [ExcludeFromCodeCoverage]
    public class MultasProcedimentos : Entity
    {
        public string TituloPagina { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public bool Ativo { get; set; } = true;
        public DateTime DataCriacao { get; set; }
        public DateTime? DataAtualizacao { get; set; }

        public ICollection<MultasProcedimentosProcedimento> Procedimentos { get; set; } = [];
        public ICollection<MultasProcedimentosPortariaRelacionada> PortariasRelacionadas { get; set; } = [];
    }
}
