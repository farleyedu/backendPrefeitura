using PortalTCMSP.Domain.Entities.BaseEntity;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.Entities.FiscalizacaoEntity.ContasDoPrefeito
{
    [ExcludeFromCodeCoverage]
    public class FiscalizacaoContasDoPrefeito : Entity
    {
        public string Ano { get; set; } = string.Empty;
        public string Pauta { get; set; } = string.Empty;
        public DateTime? DataSessao { get; set; }
        public DateTime? DataPublicacao { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataAtualizacao { get; set; }
        public ICollection<FiscalizacaoContasDoPrefeitoAnexo> Anexos { get; set; } = [];
    }
}
