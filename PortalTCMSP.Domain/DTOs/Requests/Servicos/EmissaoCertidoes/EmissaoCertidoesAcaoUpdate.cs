using PortalTCMSP.Domain.Enum;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Requests.Servicos.EmissaoCertidoes
{
    [ExcludeFromCodeCoverage]
    public sealed class EmissaoCertidoesAcaoUpdate
    {
        public long Id { get; set; }
        public bool Ativo { get; set; } = true;
        public int Ordem { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public DateTime DataPublicacao { get; set; }
        public TipoAcao TipoAcao { get; set; }
        public string UrlAcao { get; set; } = string.Empty;
    }
}
