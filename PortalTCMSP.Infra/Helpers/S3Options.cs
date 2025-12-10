namespace PortalTCMSP.Infra.Helpers
{
    public sealed class S3Options
    {
        public string BaseUrl { get; set; } = string.Empty;
        public string AccessKey { get; set; } = string.Empty;
        public string SecretKey { get; set; } = string.Empty;
        public string DefaultBucketName { get; set; } = string.Empty;
        public bool EnableSSL { get; set; } = true;
        public bool ForcePathStyle { get; set; } = true;
        public bool VerifySSL { get; set; } = true;
        public string? CaCertPath { get; set; }
        public string Region { get; set; } = "none";   
    }
}
