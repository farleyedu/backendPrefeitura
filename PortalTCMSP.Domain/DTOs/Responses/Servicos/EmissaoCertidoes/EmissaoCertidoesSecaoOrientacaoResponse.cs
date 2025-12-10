using PortalTCMSP.Domain.Enum;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Responses.Servicos.EmissaoCertidoes
{
    [ExcludeFromCodeCoverage]
    public sealed class EmissaoCertidoesSecaoOrientacaoResponse
    {
        public long Id { get; set; }
        public TipoSecao TipoSecao { get; set; }
        public string TituloPagina { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public List<EmissaoCertidoesOrientacaoResponse> Orientacoes { get; set; } = new();
    }
}
