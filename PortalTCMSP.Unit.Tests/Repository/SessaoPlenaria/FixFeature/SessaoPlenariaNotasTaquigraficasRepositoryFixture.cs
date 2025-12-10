using PortalTCMSP.Domain.Entities.SessaoPlenariaEntity;
using PortalTCMSP.Domain.Enum;
using System.Diagnostics.CodeAnalysis;


namespace PortalTCMSP.Unit.Tests.Repository.SessaoPlenaria.FixFeature;

[ExcludeFromCodeCoverage]
public class SessaoPlenariaNotasTaquigraficasRepositoryFixture
{
    public static SessaoPlenariaNotasTaquigraficas GetSessaoPlenariaNotasTaquigraficas(int id = 1, int idSessaoPlenaria = 1, int idSessao = 1) => new SessaoPlenariaNotasTaquigraficas
    {
        Id = id,
        DataAtualizacao = DateTime.Now,
        DataCriacao = DateTime.Now.AddDays(-10),
        DataPublicacao = DateTime.Now.AddDays(-5),
        Numero = $"{id}",
        IdSessaoPlenaria = idSessaoPlenaria,
        Tipo = NotasTipo.Ordinaria,
        Anexos = GetSessoesPlenariasNotasTaquigraficasAnexos(id, idSessaoPlenaria),
    };

    public static List<SessaoPlenariaNotasTaquigraficas> GetSessoesPlenariasNotasTaquigraficas(int idSessaoPlenaria) =>
        [
            GetSessaoPlenariaNotasTaquigraficas(2 * idSessaoPlenaria - 1, idSessaoPlenaria),
                GetSessaoPlenariaNotasTaquigraficas(2 * idSessaoPlenaria, idSessaoPlenaria)
        ];
    public static SessaoPlenariaNotasTaquigraficasAnexos GetSessoesPlenariasNotasTaquigraficasAnexo(
        int id = 1,
        int idSessaoPlenariaNotasTaquigraficas = 1,
        int ordem = 1,
        string tipoArquivo = "pdf"
        ) => new SessaoPlenariaNotasTaquigraficasAnexos
        {
            Id = id,
            IdSessaoPlenariaNotasTaquigraficas = idSessaoPlenariaNotasTaquigraficas,
            NomeExibicao = $"Anexo {id}",
            Link = $"http://example.com/anexo{id}.pdf",
            Ordem = ordem,
            TipoArquivo = $"{tipoArquivo}",
        };

    public static List<SessaoPlenariaNotasTaquigraficasAnexos> GetSessoesPlenariasNotasTaquigraficasAnexos(int idSessaoPlenariaNotasTaquigraficas, int idSessaoPlenaria) =>
        [
            GetSessoesPlenariasNotasTaquigraficasAnexo(2 * idSessaoPlenariaNotasTaquigraficas - 1, idSessaoPlenariaNotasTaquigraficas, 1, "pdf"),
                GetSessoesPlenariasNotasTaquigraficasAnexo(2 * idSessaoPlenariaNotasTaquigraficas, idSessaoPlenariaNotasTaquigraficas, 2, "docx")
        ];
}
