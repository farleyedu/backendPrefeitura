using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using PortalTCMSP.Infra.Helpers;

namespace PortalTCMSP.Infra.Extensions
{
    public static class S3ServiceCollectionExtensions
    {
        public static IServiceCollection AddS3Configuration(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<S3Options>(configuration.GetSection("S3"));

            // logging opcional
            AWSConfigs.LoggingConfig.LogTo = LoggingOptions.Console;
            AWSConfigs.LoggingConfig.LogResponses = ResponseLoggingOption.Never;
            AWSConfigs.LoggingConfig.LogMetricsFormat = LogMetricsFormatOption.JSON;

            services.AddSingleton<IAmazonS3>(sp =>
            {
                var opt = sp.GetRequiredService<IOptions<S3Options>>().Value;

                var s3Config = new AmazonS3Config
                {
                    ServiceURL = opt.BaseUrl,
                    ForcePathStyle = opt.ForcePathStyle,
                    UseHttp = !opt.EnableSSL,
                    DisableHostPrefixInjection = true,
                    RequestChecksumCalculation = RequestChecksumCalculation.WHEN_REQUIRED
                };

                if (!opt.VerifySSL || !string.IsNullOrWhiteSpace(opt.CaCertPath))
                {
                    var handler = new HttpClientHandler
                    {
                        // PROD: se tiver CA própria, implemente a validação correta.
                        ServerCertificateCustomValidationCallback = (_, __, ___, ____) => true
                    };

                    s3Config.HttpClientFactory = new CustomHttpClientFactory(
                        () => new HttpClient(handler, disposeHandler: true)
                    );
                }

                var creds = new BasicAWSCredentials(opt.AccessKey, opt.SecretKey);
                return new AmazonS3Client(creds, s3Config);
            });

            return services;
        }
    }
}
