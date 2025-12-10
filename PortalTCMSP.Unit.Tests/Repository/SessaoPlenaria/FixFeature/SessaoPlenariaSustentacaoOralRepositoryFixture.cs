using PortalTCMSP.Domain.Entities.SessaoPlenariaEntity;
using PortalTCMSP.Domain.Enum;
using System.Diagnostics.CodeAnalysis;


namespace PortalTCMSP.Unit.Tests.Repository.SessaoPlenaria.FixFeature;

[ExcludeFromCodeCoverage]
public class SessaoPlenariaSustentacaoOralRepositoryFixture
{
    public static SessaoPlenariaSustentacaoOral GetSessaoPlenariaSustentacaoOral(int id = 1, int idSessaoPlenaria = 1, int idSessao = 1) => new SessaoPlenariaSustentacaoOral
    {
        Id = id,
        DataAtualizacao = DateTime.Now,
        DataCriacao = DateTime.Now.AddDays(-10),
        Ativo = true,
        Descricao = "Descrição de Teste",
        Slug = "sessao-plenaria-sustentacao-oral-teste",
        Titulo = "Título de Teste",
        Anexos = GetSessoesPlenariasSustentacoesOraisAnexos(idSessaoPlenaria)
    };

    public static List<SessaoPlenariaSustentacaoOral> GetSessoesPlenariasSustentacoesOrais(int idSessaoPlenaria) =>
        [
            GetSessaoPlenariaSustentacaoOral(1, idSessaoPlenaria),
                GetSessaoPlenariaSustentacaoOral(2, idSessaoPlenaria)
        ];

    public static SessaoPlenariaSustentacaoOralAnexos GetSessoesPlenariasSustentacoesOraisAnexo(
        int id = 1,
        int idSessaoPlenariaSustentacaoOral = 1,
        int ordem = 1,
        string tipoArquivo = "pdf"
        ) => new SessaoPlenariaSustentacaoOralAnexos
        {
            Id = id,
            IdSessaoPlenariaSustentacaoOral = idSessaoPlenariaSustentacaoOral,
            Ordem = $"{ordem}",
            Titulo = $"Anexo {id}",
            Descricao = $"Descrição do Anexo {id}",
        };

    public static List<SessaoPlenariaSustentacaoOralAnexos> GetSessoesPlenariasSustentacoesOraisAnexos(int idSessaoPlenariaSustentacaoOral) =>
        [
            GetSessoesPlenariasSustentacoesOraisAnexo(2 * idSessaoPlenariaSustentacaoOral, idSessaoPlenariaSustentacaoOral, 1, "pdf"),
                GetSessoesPlenariasSustentacoesOraisAnexo(2 * idSessaoPlenariaSustentacaoOral -1, idSessaoPlenariaSustentacaoOral, 2, "docx")
        ];
}
