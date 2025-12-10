using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Responses.Servicos.MultasProcedimentos
{ 
    [ExcludeFromCodeCoverage]
    public sealed class MultasProcedimentosPortariaRelacionadaResponse
    {
        public long Id { get; set; }
        public int Ordem { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
    }
}
