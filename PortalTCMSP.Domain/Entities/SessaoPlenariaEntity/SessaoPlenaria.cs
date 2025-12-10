using PortalTCMSP.Domain.Entities.BaseEntity;
using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.Entities.SessaoPlenariaEntity
{
    [ExcludeFromCodeCoverage]
    public class SessaoPlenaria : Entity
    {
        public string Slug { get; set; } = string.Empty;
        public string Titulo { get; set; } = string.Empty;
        public string? Descricao { get; set; } = string.Empty;
        public string? SeoTitulo { get; set; } = string.Empty;
        public string? SeoDescricao { get; set; } = string.Empty;
        public string? UrlVideo { get; set; } = string.Empty;
        public DateTime DataCriacao { get; set; }
        public DateTime? DataAtualizacao { get; set; }
        public DateTime? DataPublicacao { get; set; }
        public string Ativo { get; set; } = "S";
        public ICollection<SessaoPlenariaPauta> Pautas { get; set; } = new List<SessaoPlenariaPauta>();
        public ICollection<SessaoPlenariaAta> Atas { get; set; } = new List<SessaoPlenariaAta>();
        public ICollection<SessaoPlenariaNotasTaquigraficas> NotasTaquigraficas { get; set; } = new List<SessaoPlenariaNotasTaquigraficas>();
    }
}
