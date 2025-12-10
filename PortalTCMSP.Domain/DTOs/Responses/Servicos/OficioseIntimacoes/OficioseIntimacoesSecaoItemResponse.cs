using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Responses.Servicos.OficioseIntimacoes
{

    [ExcludeFromCodeCoverage]
    public sealed class OficioseIntimacoesSecaoItemResponse
    {
        public long Id { get; set; }
        public int Ordem { get; set; }
        public string Descricao { get; set; } = string.Empty;
    }
}
