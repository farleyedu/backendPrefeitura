using PortalTCMSP.Domain.Entities.BaseEntity;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.Entities.ServicosEntity.EmissaoCertidoesEntity
{
    [ExcludeFromCodeCoverage]
    public class EmissaoCertidoes : Entity
    {
        public string TituloPagina { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public bool Ativo { get; set; } = true;
        public DateTime DataCriacao { get; set; }
        public DateTime? DataAtualizacao { get; set; }

        public ICollection<EmissaoCertidoesAcao> Acoes { get; set; } = [];
        public ICollection<EmissaoCertidoesSecaoOrientacao> SecaoOrientacoes { get; set; } = [];
    }
}
