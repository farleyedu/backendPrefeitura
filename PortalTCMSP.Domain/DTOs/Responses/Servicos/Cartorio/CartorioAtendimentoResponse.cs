using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Responses.Servicos.Cartorio
{
    [ExcludeFromCodeCoverage]
    public sealed class CartorioAtendimentoResponse
    {
        public long Id { get; set; }
        public int Ordem { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
    }
}
