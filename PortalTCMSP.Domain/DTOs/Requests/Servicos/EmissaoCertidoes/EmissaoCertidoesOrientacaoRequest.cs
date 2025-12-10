using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Requests.Servicos.EmissaoCertidoes
{
    [ExcludeFromCodeCoverage]
    public sealed class EmissaoCertidoesOrientacaoRequest
    {
        public int Ordem { get; set; }
        public List<string>? Descritivos { get; set; }
    }
}
