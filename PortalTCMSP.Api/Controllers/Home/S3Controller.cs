using Microsoft.AspNetCore.Mvc;
using PortalTCMSP.Api.Swagger;
using PortalTCMSP.Domain.Services.Home;

namespace PortalTCMSP.Api.Controllers.Home
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "Home")] 
    [ApiGroups("Home", "Noticia", /*"Institucional",*/ "Fiscalizacao", "SessoesPlenarias")] 
    public class S3Controller : ControllerBase
    {
        private readonly IS3Service _s3Service;
        private readonly ILogger<S3Controller> _logger;
        public S3Controller(IS3Service s3Service, ILogger<S3Controller> logger)
        {
            _s3Service = s3Service;
            _logger = logger;
        }

        /// <summary>
        /// Faz upload para S3 recebendo namePath e tipoArquivo na URL, e o base64 no body.
        /// Exemplo:
        /// PATCH /api/s3/upload/minha-area/imagem
        /// Body: "data:image/png;base64,iVBORw0KGgoAAA..."
        /// </summary>
        [HttpPatch("upload/{namePath}/{tipoArquivo}")]
        public async Task<IActionResult> Upload(
            [FromRoute] string namePath,
            [FromRoute] string tipoArquivo,
            [FromBody] string fullBase64)
        {
            var result = await _s3Service.UploadAsync(namePath, tipoArquivo, fullBase64);

            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        /// <summary>
        /// Upload via multipart/form-data recebendo arquivo "file".
        /// Exemplo: POST /api/s3/upload-file/{namePath}/{tipoArquivo}
        /// FormData: file={input file}
        /// </summary>
        [HttpPost("upload-file/{namePath}/{tipoArquivo}")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadFile(
            [FromRoute] string namePath,
            [FromRoute] string tipoArquivo,
            IFormFile file) 
        {
            if (file is null || file.Length == 0)
                return BadRequest("Arquivo não enviado ou vazio.");

            await using var stream = file.OpenReadStream();
            var result = await _s3Service.UploadStreamAsync(
                areaPath: namePath,
                tipo: tipoArquivo,
                fileStream: stream,
                contentType: file.ContentType ?? "application/octet-stream",
                originalFileName: file.FileName,
                contentLength: file.Length
            );

            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
