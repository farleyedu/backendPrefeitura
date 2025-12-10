using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Requests.Servicos.MultasProcedimentos
{
    [ExcludeFromCodeCoverage]
    public sealed class MultasProcedimentosPortariaRelacionadaRequest
    {
        public int Ordem { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
    }
}
