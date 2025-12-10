using PortalTCMSP.Domain.Entities.BaseEntity;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.Entities.ServicosEntity.OficioseIntimacoesEntity
{
    [ExcludeFromCodeCoverage]
    public class OficioseIntimacoesSecao : Entity
    {
        public long IdOficioseIntimacoes { get; set; }
        public int Ordem { get; set; }
        public string Nome { get; set; } = string.Empty;

        public OficioseIntimacoes OficioseIntimacoes { get; set; } = default!;
        public ICollection<OficioseIntimacoesSecaoItem> SecaoItem { get; set; } = [];
    }
}
