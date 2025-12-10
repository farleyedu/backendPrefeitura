using PortalTCMSP.Domain.Entities.BaseEntity;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.Entities.SessaoPlenariaEntity
{
    [ExcludeFromCodeCoverage]
    public class SessaoPlenariaJurispudencia : Entity
    {
        public string Slug { get; set; } = string.Empty;
        public string Titulo { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public string TituloBoletins { get; set; } = string.Empty;
        public string DescricaoBoletins { get; set; } = string.Empty;
        public string LinkBoletins { get; set; } = string.Empty;
        public string TituloPesquisa { get; set; } = string.Empty;
        public string DescricaoPesquisa { get; set; } = string.Empty;
        public string LinkPesquisa { get; set; } = string.Empty;
        public string TituloSumulas { get; set; } = string.Empty;
        public string DescricaoSumulas { get; set; } = string.Empty;
        public string LinkSumulas { get; set; } = string.Empty;
        public bool Ativo { get; set; } 
        public DateTime DataCriacao { get; set; }
        public DateTime? DataAtualizacao { get; set; }
    }
}
