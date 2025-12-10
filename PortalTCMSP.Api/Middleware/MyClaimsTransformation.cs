using Microsoft.AspNetCore.Authentication;
using Newtonsoft.Json.Linq;
using System.Security.Claims;

namespace PortalTCMSP.Api.Middleware
{
    public class MyClaimsTransformation : IClaimsTransformation
    {
        public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            var keycloakClaimTypes = new[] { "realm_access", "resource_access" };
            var roleClaims = principal.Claims
                .Where(c => keycloakClaimTypes.Contains(c.Type))
                .SelectMany(c => ExtractRolesFromClaim(c.Value))
                .Distinct()
                .Select(role => new Claim(ClaimTypes.Role, role))
                .ToList();

            if (roleClaims.Count > 0)
            {
                var identity = new ClaimsIdentity(roleClaims);
                principal.AddIdentity(identity);
            }

            return Task.FromResult(principal);
        }

        private static List<string> ExtractRolesFromClaim(string claimValue)
        {
            var roles = new List<string>();
            var json = JObject.Parse(claimValue);

            if (json.ContainsKey("roles"))
            {
                roles.AddRange(json["roles"]?.ToObject<List<string>>() ?? []);
            }
            else
            {
                foreach (var property in json.Properties())
                {
                    var rolesToken = property.Value["roles"];
                    if (rolesToken != null)
                    {
                        roles.AddRange(rolesToken.ToObject<List<string>>() ?? []);
                    }
                }
            }

            return roles;
        }
    }
}
