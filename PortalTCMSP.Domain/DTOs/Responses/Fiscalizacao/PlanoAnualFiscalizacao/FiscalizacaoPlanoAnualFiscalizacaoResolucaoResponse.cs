using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Responses.Fiscalizacao.PlanoAnualFiscalizacao
{
    [ExcludeFromCodeCoverage]
    public sealed class FiscalizacaoPlanoAnualFiscalizacaoResolucaoResponse
    {
        public long Id { get; set; }
        public string Slug { get; set; } = string.Empty;
        public int Numero { get; set; }
        public int Ano { get; set; }
        public DateTime DataPublicacao { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string? SubTitulo { get; set; }
        public bool Ativo { get; set; }
        public FiscalizacaoResolucaoEmentaResponse? Ementa { get; set; }
        public List<FiscalizacaoResolucaoDispositivoResponse> Dispositivos { get; set; } = [];
        public List<FiscalizacaoResolucaoAnexoResponse> Anexos { get; set; } = [];
        public List<FiscalizacaoResolucaoAtaResponse> Atas { get; set; } = [];
    }
}
