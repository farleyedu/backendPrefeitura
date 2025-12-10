using PortalTCMSP.Domain.Entities.BaseEntity;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.Entities.FiscalizacaoEntity.RelatorioFiscalizacao
{
    [ExcludeFromCodeCoverage]
    public class FiscalizacaoRelatorioFiscalizacaoConteudoLink : Entity
    {
        public string? Titulo { get; set; }
        public DateTime? DataPublicacao { get; set; }
        public string? ConselheiroRelator { get; set; }
        public string? ImagemUrl { get; set; }
        public bool PosicionamentoAEsquerda { get; set; }
        public string? Descritivo { get; set; }
        public string? PeriodoRealizacao { get; set; }
        public string? PeriodoAbrangencia { get; set; }
        public string? TituloDestaque { get; set; }

        public ICollection<FiscalizacaoRelatorioFiscalizacaoConteudoDestaque> ConteudoDestaque { get; set; } = [];
        public ICollection<FiscalizacaoRelatorioFiscalizacaoTcRelacionado> TcRelacionados { get; set; } = [];
        public ICollection<FiscalizacaoRelatorioFiscalizacaoDocumentoAnexo> DocumentosAnexos { get; set; } = [];
        public ICollection<FiscalizacaoRelatorioFiscalizacaoImagemAnexa> ImagensAnexas { get; set; } = [];
    }
}
