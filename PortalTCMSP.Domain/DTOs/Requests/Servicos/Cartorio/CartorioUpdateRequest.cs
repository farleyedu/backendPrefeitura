using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Requests.Servicos.Cartorio
{
    [ExcludeFromCodeCoverage]
    public sealed class CartorioUpdateRequest
    {
        public string TituloPagina { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public bool Ativo { get; set; } = true;
        public List<CartorioAtendimentoRequest>? Atendimentos { get; set; }
    }
}
