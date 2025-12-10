using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Responses.Servicos.MultasProcedimentos
{
    [ExcludeFromCodeCoverage]
    public sealed class MultasProcedimentosResponse
    {
        public long Id { get; set; }
        public string TituloPagina { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public bool Ativo { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataAtualizacao { get; set; }

        public List<MultasProcedimentosProcedimentoResponse> Procedimentos { get; set; } = [];
        public List<MultasProcedimentosPortariaRelacionadaResponse> PortariasRelacionadas { get; set; } = [];
    }
}
