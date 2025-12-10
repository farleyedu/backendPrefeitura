using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Requests.Servicos.OficioseIntimacoes
{
    [ExcludeFromCodeCoverage]
    public sealed class OficioseIntimacoesUpdateRequest
    {
        public string Titulo { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public bool Ativo { get; set; } = true;
        public List<OficioseIntimacoesSecaoRequest>? Secoes { get; set; }
    }
}
