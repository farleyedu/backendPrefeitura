using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Requests.Servicos.OficioseIntimacoes
{
    [ExcludeFromCodeCoverage]
    public sealed class OficioseIntimacoesSecaoRequest
    {
        public int Ordem { get; set; }
        public string Nome { get; set; } = string.Empty;
        public List<OficioseIntimacoesSecaoItemRequest>? SecaoItem { get; set; }
    }
}
