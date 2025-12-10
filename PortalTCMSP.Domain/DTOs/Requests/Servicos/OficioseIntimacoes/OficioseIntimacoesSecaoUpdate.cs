using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Requests.Servicos.OficioseIntimacoes
{
    [ExcludeFromCodeCoverage]
    public sealed class OficioseIntimacoesSecaoUpdate
    {
        public long Id { get; set; }
        public int Ordem { get; set; }
        public string Nome { get; set; } = string.Empty;
    }
}
