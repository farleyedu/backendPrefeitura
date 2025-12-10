using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Responses.SessaoPlenaria.SustentacaoOral
{
    [ExcludeFromCodeCoverage]
    public sealed class SessaoPlenariaSustentacaoOralAnexoItemResponse
    {
        public long Id { get; set; }
        public string Ordem { get; set; } = string.Empty;
        public string Titulo { get; set; } = string.Empty;
        public string? Descricao { get; set; }
    }
}
