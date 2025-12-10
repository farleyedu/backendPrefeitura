using PortalTCMSP.Domain.Entities.BaseEntity;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.Entities.FiscalizacaoEntity.PlanoAnualFiscalizacao
{
    [ExcludeFromCodeCoverage]
    public class FiscalizacaoPlanoAnualFiscalizacaoResolucao : Entity
    {
        public string Slug { get; set; } = string.Empty;
        public int Numero { get; set; }
        public int Ano { get; set; }
        public DateTime DataPublicacao { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string? SubTitulo { get; set; }
        public bool Ativo { get; set; }
        public FiscalizacaoResolucaoEmenta? Ementa { get; set; }

        public ICollection<FiscalizacaoResolucaoDispositivo> Dispositivos { get; set; } = [];
        public ICollection<FiscalizacaoResolucaoAnexo> Anexos { get; set; } = [];
        public ICollection<FiscalizacaoResolucaoAta> Atas { get; set; } = [];
    }
}
