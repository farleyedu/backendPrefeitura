using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Requests.Servicos.MultasProcedimentos
{
    [ExcludeFromCodeCoverage]
    public sealed class MultasProcedimentosProcedimentoRequest
    {

        public int Ordem { get; set; }
        public string Texto { get; set; } = string.Empty;
        public string? UrlImagem { get; set; }
    }
}
