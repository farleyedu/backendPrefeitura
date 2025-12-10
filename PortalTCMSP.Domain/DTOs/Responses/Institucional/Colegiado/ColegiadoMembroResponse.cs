using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Responses.Institucional.Colegiado
{
    [ExcludeFromCodeCoverage]
    public class ColegiadoMembroResponse
    {
        public long Id { get; set; }  
        public int Ordem { get; set; }
        public string Cargo { get; set; } = string.Empty;
        public string Nome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public string? ImagemUrl { get; set; }
    }
}
