using PortalTCMSP.Domain.DTOs.Responses.S3;
using PortalTCMSP.Domain.Shared;

namespace PortalTCMSP.Domain.Services.Home
{
    public interface IS3Service
    {
        Task<Result<UploadResultResponse>> UploadAsync(string areaPath, string tipo, string base64Data);
        Task<Result<UploadResultResponse>> UploadStreamAsync(string areaPath, string tipo, Stream fileStream, string contentType, string? originalFileName = null, long? contentLength = null);
    }
}
