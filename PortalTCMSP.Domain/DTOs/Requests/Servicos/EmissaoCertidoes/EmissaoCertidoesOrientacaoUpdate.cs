using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Requests.Servicos.EmissaoCertidoes
{
    [ExcludeFromCodeCoverage]
    public sealed class EmissaoCertidoesOrientacaoUpdate
    {
        public long Id { get; set; }
        public int Ordem { get; set; }
        public List<string>? Descritivos { get; set; }
    }
}
