using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using PortalTCMSP.Domain.DTOs.Requests.Keycloak;

namespace PortalTCMSP.Api.Middleware
{
    public static class AuthenticationMiddlewareExtensions
    {
        public static IServiceCollection AddCustomAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var authConfig = configuration
                .GetSection("Keycloak:AuthenticationOptions")
                .Get<AuthenticationConfigurationOptions>();

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.Authority = authConfig.Authority;
                    options.Audience = authConfig.Audience;

                    options.RequireHttpsMetadata = false;

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = authConfig.ValidIssuerName,
                        ValidAudience = options.Audience
                    };
                });

            return services;
        }
    }
}
