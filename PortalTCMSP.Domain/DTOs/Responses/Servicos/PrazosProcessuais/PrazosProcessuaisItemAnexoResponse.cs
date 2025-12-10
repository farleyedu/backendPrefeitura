using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Responses.Servicos.PrazosProcessuais
{
    [ExcludeFromCodeCoverage]
    public sealed class PrazosProcessuaisItemAnexoResponse
    {
        public long Id { get; set; }
        public bool Ativo { get; set; }
        public int Ordem { get; set; }
        public string NomeArquivo { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
    }
}
