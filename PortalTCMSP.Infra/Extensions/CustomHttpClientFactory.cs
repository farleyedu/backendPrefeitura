using Amazon.Runtime;

namespace PortalTCMSP.Infra.Extensions
{
    public sealed class CustomHttpClientFactory : HttpClientFactory
    {
        private readonly Func<HttpClient> _clientFactory;

        public CustomHttpClientFactory(Func<HttpClient> clientFactory)
        {
            _clientFactory = clientFactory ?? throw new ArgumentNullException(nameof(clientFactory));
        }

        public override HttpClient CreateHttpClient(IClientConfig clientConfig)
        {
            return _clientFactory();
        }
    }
}
