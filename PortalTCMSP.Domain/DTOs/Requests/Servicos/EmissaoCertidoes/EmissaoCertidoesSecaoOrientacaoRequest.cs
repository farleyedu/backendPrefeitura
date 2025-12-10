using PortalTCMSP.Domain.Enum;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Requests.Servicos.EmissaoCertidoes
{
    [ExcludeFromCodeCoverage]
    public sealed class EmissaoCertidoesSecaoOrientacaoRequest
    {
        public TipoSecao TipoSecao { get; set; }
        public string TituloPagina { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public List<EmissaoCertidoesOrientacaoRequest>? Orientacoes { get; set; }
    }
}
