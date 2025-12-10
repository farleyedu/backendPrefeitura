using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Unit.Tests.Repository.SessaoPlenaria.FixFeature
{
    [ExcludeFromCodeCoverage]
    public class SessaoPlenariaRepositoryFixture
    {

        // Sessão Plenária
        public static PortalTCMSP.Domain.Entities.SessaoPlenariaEntity.SessaoPlenaria GetSessao(int id = 1) => new PortalTCMSP.Domain.Entities.SessaoPlenariaEntity.SessaoPlenaria
        {
            Id = id,
            Titulo = $"Sessão {id}",
            Descricao = $"Descrição da Sessão {id}",
            Ativo = "S",
            DataAtualizacao = DateTime.Now,
            DataCriacao = DateTime.Now.AddDays(-10),
            DataPublicacao = DateTime.Now.AddDays(-5),
            NotasTaquigraficas = SessaoPlenariaNotasTaquigraficasRepositoryFixture.GetSessoesPlenariasNotasTaquigraficas(id),
            Pautas = SessaoPlenariaPautaRepositoryFixture.GetSessoesPlenariasPautas(id),
            SeoDescricao = $"SEO Descrição da Sessão {id}",
            SeoTitulo = $"SEO Título da Sessão {id}",
            Slug = $"sessao-plenaria-{id}",
            UrlVideo = $"http://example.com/sessao{id}.mp4",
            Atas = SessaoPlenariaAtaRepositoryFixture.GetSessoesPlenariasAtas(id),
        };

        public static List<PortalTCMSP.Domain.Entities.SessaoPlenariaEntity.SessaoPlenaria> GetSessoes() => [GetSessao(1), GetSessao(2)];
    }
}
