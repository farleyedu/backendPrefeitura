using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Requests.Servicos.MultasProcedimentos
{
    [ExcludeFromCodeCoverage]
    public sealed class MultasProcedimentosUpdateRequest
    {
        public string TituloPagina { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public bool Ativo { get; set; } = true;
        public List<MultasProcedimentosProcedimentoRequest>? Procedimentos { get; set; }
        public List<MultasProcedimentosPortariaRelacionadaRequest>? PortariasRelacionadas { get; set; }
    }
}
