using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Responses.SessaoPlenaria.SustentacaoOral
{
    [ExcludeFromCodeCoverage]
    public sealed class SessaoPlenariaSustentacaoOralResponse
    {
        public long Id { get; set; }
        public string Slug { get; set; } = string.Empty;
        public string? Titulo { get; set; }
        public string? Descricao { get; set; }
        public bool Ativo { get; set; }
        public DateTime? DataCriacao { get; set; }
        public DateTime? DataAtualizacao { get; set; }
        public List<SessaoPlenariaSustentacaoOralAnexoItemResponse> Anexos { get; set; } = [];
    }
}
