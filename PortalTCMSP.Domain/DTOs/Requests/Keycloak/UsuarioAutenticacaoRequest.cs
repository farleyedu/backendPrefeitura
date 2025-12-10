using System.Text.Json.Serialization;

namespace PortalTCMSP.Domain.DTOs.Requests.Keycloak
{
    public class UsuarioAutenticacaoRequest
    {
        [JsonPropertyName("access_token")]
        public string Token { get; set; } = string.Empty;
    }
}
