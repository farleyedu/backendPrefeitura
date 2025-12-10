using PortalTCMSP.Domain.Enum;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Requests.Servicos.EmissaoCertidoes
{
    [ExcludeFromCodeCoverage]
    public sealed class EmissaoCertidoesSecaoOrientacaoUpdate
    {
        public long Id { get; set; }
        public TipoSecao TipoSecao { get; set; }
        public string TituloPagina { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public List<EmissaoCertidoesOrientacaoUpdate>? Orientacoes { get; set; }
    }
}
