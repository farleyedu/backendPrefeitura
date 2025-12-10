using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Responses.Servicos.EmissaoCertidoes
{
    [ExcludeFromCodeCoverage]
    public sealed class EmissaoCertidoesResponse
    {
        public long Id { get; set; }
        public string TituloPagina { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public bool Ativo { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime? DataAtualizacao { get; set; }

        public List<EmissaoCertidoesAcaoResponse> Acoes { get; set; } = new();
        public List<EmissaoCertidoesSecaoOrientacaoResponse> SecaoOrientacoes { get; set; } = new();
    }
}
