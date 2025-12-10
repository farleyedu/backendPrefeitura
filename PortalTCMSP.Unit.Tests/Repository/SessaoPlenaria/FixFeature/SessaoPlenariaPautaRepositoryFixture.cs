using PortalTCMSP.Domain.Entities.SessaoPlenariaEntity;
using PortalTCMSP.Domain.Enum;
using System.Diagnostics.CodeAnalysis;


namespace PortalTCMSP.Unit.Tests.Repository.SessaoPlenaria.FixFeature;

[ExcludeFromCodeCoverage]
public class SessaoPlenariaPautaRepositoryFixture
{
    public static SessaoPlenariaPauta GetSessaoPlenariaPauta(int id = 1, int idSessaoPlenaria = 1, int idSessao = 1) => new SessaoPlenariaPauta
    {
        Id = id,
        DataAtualizacao = DateTime.Now,
        DataCriacao = DateTime.Now.AddDays(-10),
        DataPublicacao = DateTime.Now.AddDays(-5),
        Numero = $"{id}",
        DataDaSesao = DateTime.Now.AddDays(-10),
        Tipo = PautaTipo.Ordinaria,
        IdSessaoPlenaria = idSessaoPlenaria,
        SessaoPlenaria = null!,
        Anexos = GetSessoesPlenariasPautasAnexos(id)
    };

    public static List<SessaoPlenariaPauta> GetSessoesPlenariasPautas(int idSessaoPlenaria) =>
        [
            GetSessaoPlenariaPauta(2 * idSessaoPlenaria - 1, idSessaoPlenaria),
            GetSessaoPlenariaPauta(2 * idSessaoPlenaria, idSessaoPlenaria)
        ];

    public static SessaoPlenariaPautaAnexo GetSessoesPlenariasPautasAnexo(
        int id = 1,
        int idSessaoPlenariaPauta = 1,
        int ordem = 1,
        string tipoArquivo = "pdf"
        ) => new SessaoPlenariaPautaAnexo
        {
            Id = id,
            IdSessaoPlenariaPauta = idSessaoPlenariaPauta,
            NomeExibicao = $"Anexo {id}",
            Link = $"http://example.com/anexo{id}.pdf",
            Ordem = ordem,
            TipoArquivo = $"{tipoArquivo}",
        };

    public static List<SessaoPlenariaPautaAnexo> GetSessoesPlenariasPautasAnexos(int idSessaoPlenariaPauta) =>
        [
            GetSessoesPlenariasPautasAnexo(2 * idSessaoPlenariaPauta, idSessaoPlenariaPauta, 1, "pdf"),
                GetSessoesPlenariasPautasAnexo(2 * idSessaoPlenariaPauta - 1, idSessaoPlenariaPauta, 2, "docx")
        ];
}
