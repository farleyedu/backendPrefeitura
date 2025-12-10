using PortalTCMSP.Domain.Entities.SessaoPlenariaEntity;
using System.Diagnostics.CodeAnalysis;


namespace PortalTCMSP.Unit.Tests.Repository.SessaoPlenaria.FixFeature;

[ExcludeFromCodeCoverage]
public class SessaoPlenariaJurisprudenciaRepositoryFixture
{
    public static SessaoPlenariaJurispudencia GetSessaoPlenariaJurisprudencia(int id = 1, int idSessao = 1) => new SessaoPlenariaJurispudencia
    {
        Id = id,
        DataAtualizacao = DateTime.Now,
        DataCriacao = DateTime.Now.AddDays(-10),
        Ativo = true,
        Descricao = "Descrição de Teste",
        DescricaoBoletins = "Descrição de Boletins de Teste",
        DescricaoPesquisa = "Descrição de Pesquisa de Teste",
        DescricaoSumulas = "Descrição de Súmulas de Teste",
        LinkBoletins = "http://example.com/boletins",
        LinkPesquisa = "http://example.com/pesquisa",
        LinkSumulas = "http://example.com/sumulas",
        Slug = "sessao-plenaria-jurisprudencia-teste",
        Titulo = "Título de Teste",
        TituloBoletins  = "Título de Boletins de Teste",
        TituloPesquisa = "Título de Pesquisa de Teste",
        TituloSumulas = "Título de Súmulas de Teste",
    };
}
