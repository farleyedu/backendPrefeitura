using PortalTCMSP.Domain.DTOs.Requests.Consulta;
using PortalTCMSP.Domain.DTOs.Responses.Base;
using PortalTCMSP.Domain.DTOs.Responses.Consulta;

namespace PortalTCMSP.Domain.Services.Home
{
    public interface IConsultaService
    {
        ResultadoPaginadoResponse<ConsultaNoticiaItemResponse> BuscarNoticiasConsolidado(ConsultaNoticiasRequest request);
    }
}
