using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Requests.Servicos.OficioseIntimacoes
{
    [ExcludeFromCodeCoverage]
    public sealed class OficioseIntimacoesSecaoItemUpdate
    {
        public long Id { get; set; }
        public int Ordem { get; set; }
        public string Descricao { get; set; } = string.Empty;
    }
}
