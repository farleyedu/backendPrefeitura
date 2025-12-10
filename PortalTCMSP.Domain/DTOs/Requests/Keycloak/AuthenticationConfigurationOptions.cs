using System.ComponentModel.DataAnnotations;

namespace PortalTCMSP.Domain.DTOs.Requests.Keycloak
{
    public sealed class AuthenticationConfigurationOptions
    {
        public const string SectionPath = "Keycloak:AuthenticationOptions";
        [Required]
        public string Authority { get; init; } = default!;

        [Required]
        public string Audience { get; init; } = default!;

        [Required]
        public string ValidIssuerName { get; init; } = default!;
        public bool RequireHttpsMetadata { get; init; } = true;
        public bool ValidateIssuer { get; init; } = true;
        public bool ValidateAudience { get; init; } = true;
        public string? MetadataAddress { get; init; }
    }
}
