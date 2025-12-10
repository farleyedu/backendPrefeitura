using PortalTCMSP.Domain.Entities.BaseEntity;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.Entities.ServicosEntity.OficioseIntimacoesEntity
{
    [ExcludeFromCodeCoverage]
    public class OficioseIntimacoesSecaoItem : Entity
    {
        public long IdOficioseIntimacoesSecao { get; set; }
        public int Ordem { get; set; }
        public string Descricao { get; set; } = string.Empty;

        public OficioseIntimacoesSecao OficioseIntimacoesSecao { get; set; } = default!;
    }
}
