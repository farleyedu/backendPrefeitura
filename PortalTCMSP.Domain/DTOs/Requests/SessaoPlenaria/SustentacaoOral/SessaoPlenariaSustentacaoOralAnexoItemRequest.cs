using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Requests.SessaoPlenaria.SustentacaoOral
{
    [ExcludeFromCodeCoverage]
    public sealed class SessaoPlenariaSustentacaoOralAnexoItemRequest
    {
        public string Ordem { get; set; } = string.Empty;  
        public string Titulo { get; set; } = string.Empty;
        public string? Descricao { get; set; }
    }
}
