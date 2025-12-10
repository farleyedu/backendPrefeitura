using PortalTCMSP.Domain.Entities.BaseEntity;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.Entities.ServicosEntity.OficioseIntimacoesEntity
{
    [ExcludeFromCodeCoverage]
    public class OficioseIntimacoes : Entity
    {
        public string Titulo { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public bool Ativo { get; set; } = true;
        public DateTime DataCriacao { get; set; }
        public DateTime? DataAtualizacao { get; set; }

        public ICollection<OficioseIntimacoesSecao> Secoes { get; set; } = [];
    }
}
