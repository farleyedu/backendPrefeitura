namespace PortalTCMSP.Infra.Helpers
{
    public static class S3UrlBuilder
    {
        public static string BuildPublicUrl(string endpointUrl, string bucket, string key, bool forcePathStyle, bool useSsl)
        {
            endpointUrl = endpointUrl.TrimEnd('/');
            if (forcePathStyle)
                return $"{endpointUrl}/{bucket}/{key}";

            // virtual hosted-style
            var uri = new Uri(endpointUrl);
            var scheme = string.IsNullOrEmpty(uri.Scheme) ? (useSsl ? "https" : "http") : uri.Scheme;
            var host = string.IsNullOrEmpty(uri.Host) ? endpointUrl.Replace("https://", "").Replace("http://", "") : uri.Host;
            var port = uri.IsDefaultPort ? "" : $":{uri.Port}";
            return $"{scheme}://{bucket}.{host}{port}/{key}";
        }
    }
}
