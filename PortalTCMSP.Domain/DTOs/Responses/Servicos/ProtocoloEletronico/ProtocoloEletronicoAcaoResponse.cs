using PortalTCMSP.Domain.Enum;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Responses.Servicos.ProtocoloEletronico
{
    [ExcludeFromCodeCoverage]
    public sealed class ProtocoloEletronicoAcaoResponse
    {
        public long Id { get; set; }
        public bool Ativo { get; set; }
        public int Ordem { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public DateTime DataPublicacao { get; set; }
        public TipoAcao TipoAcao { get; set; }
        public string UrlAcao { get; set; } = string.Empty;
    }
}
