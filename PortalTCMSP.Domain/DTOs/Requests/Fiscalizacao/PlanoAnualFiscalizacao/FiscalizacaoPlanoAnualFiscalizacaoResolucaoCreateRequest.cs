using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Requests.Fiscalizacao.PlanoAnualFiscalizacao
{
    [ExcludeFromCodeCoverage]
    public sealed class FiscalizacaoPlanoAnualFiscalizacaoResolucaoCreateRequest
    {
        public string Slug { get; set; } = string.Empty;
        public int Numero { get; set; }
        public int Ano { get; set; }
        public DateTime DataPublicacao { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string? SubTitulo { get; set; }
        public bool Ativo { get; set; } = true;
        public FiscalizacaoResolucaoEmentaRequest? Ementa { get; set; }
        public List<FiscalizacaoResolucaoDispositivoRequest>? Dispositivos { get; set; }
        public List<FiscalizacaoResolucaoAnexoRequest>? Anexos { get; set; }
        public List<FiscalizacaoResolucaoAtaRequest>? Atas { get; set; }
    }
}
