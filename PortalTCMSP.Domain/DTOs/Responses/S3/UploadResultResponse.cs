using System.Diagnostics.CodeAnalysis;

namespace PortalTCMSP.Domain.DTOs.Responses.S3
{
    [ExcludeFromCodeCoverage]
    public class UploadResultResponse
    {
        public string Status { get; set; } = "";
        public string? Tipo { get; set; }
        public string? Url { get; set; }
        public string? Mime { get; set; }
        public string? Ext { get; set; }
        public string? Grupo { get; set; }
        public int Ordem { get; set; }
        public string? S3Bucket { get; set; }
        public string? S3Key { get; set; }
    }
}
