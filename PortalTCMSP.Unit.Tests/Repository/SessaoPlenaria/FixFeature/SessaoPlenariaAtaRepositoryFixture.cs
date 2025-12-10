using PortalTCMSP.Domain.Entities.SessaoPlenariaEntity;
using PortalTCMSP.Domain.Enum;
using System.Diagnostics.CodeAnalysis;


namespace PortalTCMSP.Unit.Tests.Repository.SessaoPlenaria.FixFeature;

[ExcludeFromCodeCoverage]
public class SessaoPlenariaAtaRepositoryFixture
{
    public static SessaoPlenariaAta GetSessaoPlenariaAta(int id = 1, int idSessaoPlenaria = 1, int idSessao = 1) => new SessaoPlenariaAta
    {
        Id = id,
        Tipo = AtaTipo.Ordinaria,
        Anexos = GetSessoesPlenariasAtasAnexos(id),
        DataAtualizacao = DateTime.Now,
        DataCriacao = DateTime.Now.AddDays(-10),
        DataPublicacao = DateTime.Now.AddDays(-5),
        IdSessaoPlenaria = idSessaoPlenaria,
        Numero = $"{id}",
    };

    public static List<SessaoPlenariaAta> GetSessoesPlenariasAtas(int idSessaoPlenaria) =>
        [
            GetSessaoPlenariaAta(2 * idSessaoPlenaria - 1, idSessaoPlenaria),
            GetSessaoPlenariaAta(2 * idSessaoPlenaria, idSessaoPlenaria)
        ];

    public static SessaoPlenariaAtaAnexo GetSessoesPlenariasAtasAnexo(
        int id = 1,
        int idSessaoPlenariaAta = 1,
        int ordem = 1,
        string tipoArquivo = "pdf"
        ) => new SessaoPlenariaAtaAnexo
        {
            Id = id,
            IdSessaoPlenariaAta = idSessaoPlenariaAta,
            NomeExibicao = $"Anexo {id}",
            Link = $"http://example.com/anexo{id}.pdf",
            Ordem = ordem,
            TipoArquivo = $"{tipoArquivo}"
        };

    public static List<SessaoPlenariaAtaAnexo> GetSessoesPlenariasAtasAnexos(int idSessaoPlenariaAta) =>
        [
            GetSessoesPlenariasAtasAnexo(2 * idSessaoPlenariaAta, idSessaoPlenariaAta, 1, "pdf"),
            GetSessoesPlenariasAtasAnexo(2 * idSessaoPlenariaAta + 1, idSessaoPlenariaAta, 2, "docx")
        ];
}
