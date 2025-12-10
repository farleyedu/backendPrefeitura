using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Requests.SessaoPlenaria.SustentacaoOral
{
    [ExcludeFromCodeCoverage]
    public sealed class SessaoPlenariaSustentacaoOralUpdateRequest
    {
        public string Slug { get; set; } = string.Empty;
        public string? Titulo { get; set; }
        public string? Descricao { get; set; }
        public bool? Ativo { get; set; }
        public List<SessaoPlenariaSustentacaoOralAnexoItemRequest>? Anexos { get; set; }
    }
}
