using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Requests.Servicos.OficioseIntimacoes
{
    [ExcludeFromCodeCoverage]
    public sealed class OficioseIntimacoesSecaoItemRequest
    {
        public int Ordem { get; set; }
        public string Descricao { get; set; } = string.Empty;
    }
}
