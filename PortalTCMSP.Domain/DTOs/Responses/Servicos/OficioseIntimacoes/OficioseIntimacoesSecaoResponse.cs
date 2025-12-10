using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Responses.Servicos.OficioseIntimacoes
{
    [ExcludeFromCodeCoverage]
    public sealed class OficioseIntimacoesSecaoResponse
    {
        public long Id { get; set; }
        public int Ordem { get; set; }
        public string Nome { get; set; } = string.Empty;
        public List<OficioseIntimacoesSecaoItemResponse> SecaoItem { get; set; } = [];
    }
}
