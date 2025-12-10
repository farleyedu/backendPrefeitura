using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Responses.Servicos.EmissaoCertidoes
{
    [ExcludeFromCodeCoverage]
    public sealed class EmissaoCertidoesOrientacaoResponse
    {
        public long Id { get; set; }
        public int Ordem { get; set; }
        public List<string> Descritivos { get; set; } = new();
    }
}
