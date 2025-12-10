using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Requests.Servicos.MultasProcedimentos
{
    [ExcludeFromCodeCoverage]
    public sealed class MultasProcedimentosProcedimentoUpdate
    {
        public long Id { get; set; }
        public int Ordem { get; set; }
        public string Texto { get; set; } = string.Empty;
        public string? UrlImagem { get; set; }
    }
}
