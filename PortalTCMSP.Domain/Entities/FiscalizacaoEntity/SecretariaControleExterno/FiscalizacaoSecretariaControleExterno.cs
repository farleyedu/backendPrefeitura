using PortalTCMSP.Domain.Entities.BaseEntity;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.Entities.FiscalizacaoEntity.SecretariaControleExterno
{
    [ExcludeFromCodeCoverage]
    public class FiscalizacaoSecretariaControleExterno : Entity
    {
        public string Slug { get; set; } = string.Empty;
        public string Titulo { get; set; } = string.Empty;
        public string? Descricao { get; set; }
        public string? Creditos { get; set; }
        public bool Ativo { get; set; } = true;
        public DateTime DataCriacao { get; set; }
        public DateTime? DataAtualizacao { get; set; }
        public ICollection<FiscalizacaoSecretariaSecaoConteudoTitulo> Titulos { get; set; } = [];
        public ICollection<FiscalizacaoSecretariaSecaoConteudoCarrosselItem> Carrossel { get; set; } = [];
    }
}
