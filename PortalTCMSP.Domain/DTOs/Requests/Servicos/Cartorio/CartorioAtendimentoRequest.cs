using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Requests.Servicos.Cartorio
{
    [ExcludeFromCodeCoverage]
    public sealed class CartorioAtendimentoRequest
    {
        public int Ordem { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
    }
}
