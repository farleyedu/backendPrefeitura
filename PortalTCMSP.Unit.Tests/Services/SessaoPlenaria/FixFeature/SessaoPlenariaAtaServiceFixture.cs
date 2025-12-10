using Amazon.S3.Model;
using PortalTCMSP.Domain.DTOs.Requests.SessaoPlenaria.Ata;
using PortalTCMSP.Domain.DTOs.Responses.SessaoPlenaria.Ata;
using PortalTCMSP.Domain.Enum;

namespace PortalTCMSP.Unit.Tests.Services.FixFeature.SessaoPlenariaServiceFixture;

public class SessaoPlenariaAtaServiceFixture
{
    public static SessaoPlenariaAtaResponse GetSessaoPlenariaAtaResponse(int id = 1, int idSessaoPlenaria = 1, int idSessao = 1)
    {
        return new SessaoPlenariaAtaResponse
        {
            Id = id,
            DataPublicacao = DateTime.UtcNow,
            DataAtualizacao = DateTime.UtcNow,
            DataCriacao = DateTime.UtcNow.AddDays(-10),
            IdSessaoPlenaria = idSessaoPlenaria,
            Numero = "1",
            Tipo = AtaTipo.Ordinaria,
            Anexos = GetSessaoPlenariaAtaAnexoItemResponses(id)
        };
    }

    public static List<SessaoPlenariaAtaAnexoItemResponse> GetSessaoPlenariaAtaAnexoItemResponses(int id)
    {
        return new List<SessaoPlenariaAtaAnexoItemResponse>
        {
            new SessaoPlenariaAtaAnexoItemResponse
            {
                Id = 2 * id - 1,
                Link = "http://example.com/anexo1.pdf",
                NomeExibicao = "Anexo 1",
                Ordem = 1,
                TipoArquivo = "pdf",
            },
            new SessaoPlenariaAtaAnexoItemResponse
            {
                Id = 2 * id,
                Link = "http://example.com/anexo2.docx",
                NomeExibicao = "Anexo 2",
                Ordem = 2,
                TipoArquivo = "docx"
            }
        };
    }

    public static SessaoPlenariaAtaCreateRequest GetSessaoPlenariaAtaCreateRequest(int idSessaoPlenaria = 1)
    {
        return new SessaoPlenariaAtaCreateRequest
        {
            IdSessaoPlenaria = idSessaoPlenaria,
            Numero = "1",
            Tipo = AtaTipo.Ordinaria,
            DataPublicacao = DateTime.UtcNow,
            Anexos = GetSessaoPlenariaAtaAnexoItemRequests()
        };
    }

    public static List<SessaoPlenariaAtaAnexoItemRequest> GetSessaoPlenariaAtaAnexoItemRequests()
    {
        return new List<SessaoPlenariaAtaAnexoItemRequest>
        {
            new SessaoPlenariaAtaAnexoItemRequest
            {
                Link = "http://example.com/anexo1.pdf",
                NomeExibicao = "Anexo 1",
                Ordem = 1,
                TipoArquivo = "pdf",
            },
            new SessaoPlenariaAtaAnexoItemRequest
            {
                Link = "http://example.com/anexo2.docx",
                NomeExibicao = "Anexo 2",
                Ordem = 2,
                TipoArquivo = "docx"
            }
        };
    }

    public static SessaoPlenariaAtaUpdateRequest GetUpdateRequest()
    {
        return new SessaoPlenariaAtaUpdateRequest
        {
            Numero = "2",
            Tipo = AtaTipo.Extraordinaria,
            DataPublicacao = DateTime.UtcNow,
            Anexos = GetSessaoPlenariaAtaAnexoItemRequests()
        };
    }
}
