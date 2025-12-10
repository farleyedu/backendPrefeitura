using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Options;
using PortalTCMSP.Domain.DTOs.Responses.S3;
using PortalTCMSP.Domain.Services.Home;
using PortalTCMSP.Domain.Shared;
using PortalTCMSP.Infra.Helpers;
using System.Text;
using System.Text.RegularExpressions;

namespace PortalTCMSP.Infra.Services.Home
{
    public class S3Service : IS3Service
    {
        private readonly IAmazonS3 _s3;
        private readonly S3Options _opt;

        // Whitelists (mesmas que você já tinha)
        private static readonly HashSet<string> ExtImg = new(StringComparer.OrdinalIgnoreCase)
            { "jpg","jpeg","png","gif","bmp","webp","svg" };
        private static readonly HashSet<string> ExtVid = new(StringComparer.OrdinalIgnoreCase)
            { "mp4","webm","ogg","avi","mov","wmv" };
        private static readonly HashSet<string> ExtDoc = new(StringComparer.OrdinalIgnoreCase)
            { "pdf","docx","xlsx","pptx","doc","xls","ppt","odt","ods","odp" };

        private static readonly HashSet<string> MimeDoc = new(StringComparer.OrdinalIgnoreCase)
        {
            "application/pdf",
            "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "application/vnd.openxmlformats-officedocument.presentationml.presentation",
            "application/msword",
            "application/vnd.ms-excel",
            "application/vnd.ms-powerpoint",
            "application/vnd.oasis.opendocument.text",
            "application/vnd.oasis.opendocument.spreadsheet",
            "application/vnd.oasis.opendocument.presentation"
        };

        private static readonly Dictionary<string, string> SubtypeToExt = new(StringComparer.OrdinalIgnoreCase)
        {
            ["jpeg"] = "jpg",
            ["vnd.openxmlformats-officedocument.wordprocessingml.document"] = "docx",
            ["vnd.openxmlformats-officedocument.spreadsheetml.sheet"] = "xlsx",
            ["vnd.openxmlformats-officedocument.presentationml.presentation"] = "pptx",
            ["msword"] = "doc",
            ["vnd.ms-excel"] = "xls",
            ["vnd.ms-powerpoint"] = "ppt",
            ["vnd.oasis.opendocument.text"] = "odt",
            ["vnd.oasis.opendocument.spreadsheet"] = "ods",
            ["vnd.oasis.opendocument.presentation"] = "odp",
            ["pdf"] = "pdf"
        };

        public S3Service(IAmazonS3 s3, IOptions<S3Options> options)
        {
            _s3 = s3;
            _opt = options.Value;
        }

        // ---------- Helpers compartilhados ----------
        private static bool IsValidType(string t) => t is "imagem" or "video" or "documento";

        private static string SanitizePath(string? p)
        {
            if (string.IsNullOrWhiteSpace(p)) return "root";
            var s = p.Trim().Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder(s.Length);
            foreach (var ch in s)
            {
                var cat = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(ch);
                if (cat != System.Globalization.UnicodeCategory.NonSpacingMark) sb.Append(ch);
            }
            s = sb.ToString().Normalize(NormalizationForm.FormC);
            s = Regex.Replace(s, @"\s+", "-");
            s = s.Replace("..", "");
            s = Regex.Replace(s, @"[^\w\-/]", "-");
            s = Regex.Replace(s, @"/{2,}", "/");
            s = s.Trim('/').ToLowerInvariant();
            return string.IsNullOrEmpty(s) ? "root" : s;
        }

        private static bool TipoCombina(string tipo, string media)
            => tipo == "imagem" && media == "image"
            || tipo == "video" && media == "video"
            || tipo == "documento" && media == "application";

        private static bool ValidateFileSize(string tipo, long size)
        {
            var max = tipo switch
            {
                "imagem" => 10L * 1024 * 1024,
                "video" => 200L * 1024 * 1024,
                "documento" => 30L * 1024 * 1024,
                _ => 0L
            };
            return max == 0 || size <= max;
        }

        private static string MapExt(string subType)
            => SubtypeToExt.TryGetValue(subType, out var mapped) ? mapped : subType;

        private static bool ExtPermitida(string tipo, string ext)
            => tipo == "imagem" && ExtImg.Contains(ext)
            || tipo == "video" && ExtVid.Contains(ext)
            || tipo == "documento" && ExtDoc.Contains(ext);

        private static string GetMimeForExt(string ext) => ext.ToLower() switch
        {
            "jpg" or "jpeg" => "image/jpeg",
            "png" => "image/png",
            "gif" => "image/gif",
            "bmp" => "image/bmp",
            "webp" => "image/webp",
            "svg" => "image/svg+xml",
            "mp4" => "video/mp4",
            "webm" => "video/webm",
            "ogg" => "video/ogg",
            "avi" => "video/x-msvideo",
            "mov" => "video/quicktime",
            "wmv" => "video/x-ms-wmv",
            "pdf" => "application/pdf",
            "docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
            "xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "pptx" => "application/vnd.openxmlformats-officedocument.presentationml.presentation",
            "doc" => "application/msword",
            "xls" => "application/vnd.ms-excel",
            "ppt" => "application/vnd.ms-powerpoint",
            "odt" => "application/vnd.oasis.opendocument.text",
            "ods" => "application/vnd.oasis.opendocument.spreadsheet",
            "odp" => "application/vnd.oasis.opendocument.presentation",
            _ => "application/octet-stream"
        };

        private static (string media, string sub) SplitContentType(string contentType)
        {
            // Ex.: "image/png" -> media=image, sub=png
            var parts = (contentType ?? "").Split('/', 2, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            if (parts.Length == 2) return (parts[0].ToLowerInvariant(), parts[1].ToLowerInvariant());
            return ("application", "octet-stream");
        }

        private static string? ExtFromFileName(string? name)
        {
            if (string.IsNullOrWhiteSpace(name)) return null;
            var dot = name.LastIndexOf('.');
            if (dot < 0 || dot == name.Length - 1) return null;
            return name[(dot + 1)..].ToLowerInvariant();
        }

        private static (bool ok, byte[]? bytes, string? media, string? sub, string? err) ParseBase64(string data)
        {
            var rx = new Regex(@"^data:(image|video|application)/([a-z0-9.+-]+);base64,([A-Za-z0-9/\r\n+=]+)$",
                RegexOptions.IgnoreCase | RegexOptions.Compiled);

            var m = rx.Match(data ?? string.Empty);
            if (!m.Success) return (false, null, null, null, "Formato base64 inválido (esperado Data URL)");

            try
            {
                var bytes = Convert.FromBase64String(m.Groups[3].Value);
                if (bytes.Length < 100) return (false, null, m.Groups[1].Value.ToLower(), m.Groups[2].Value.ToLower(), "base64 inválido ou muito curto");
                return (true, bytes, m.Groups[1].Value.ToLower(), m.Groups[2].Value.ToLower(), null);
            }
            catch
            {
                return (false, null, m.Groups[1].Value.ToLower(), m.Groups[2].Value.ToLower(), "Erro ao processar base64");
            }
        }
        // ---------- Fim helpers ----------

        // (LEGADO) Mantém seu método existente (ajustado para usar helpers)
        public async Task<Result<UploadResultResponse>> UploadAsync(string areaPath, string tipo, string base64Data)
        {
            try
            {
                if (!IsValidType(tipo))
                    return Result<UploadResultResponse>.Fail("Tipo inválido. Use: imagem, video ou documento.");

                var parsed = ParseBase64(base64Data);
                if (!parsed.ok)
                    return Result<UploadResultResponse>.Fail(parsed.err ?? "Base64 inválido.");

                if (!TipoCombina(tipo, parsed.media!))
                    return Result<UploadResultResponse>.Fail("Tipo declarado não corresponde ao mediaType do base64.");

                if (!ValidateFileSize(tipo, parsed.bytes!.LongLength))
                    return Result<UploadResultResponse>.Fail($"Arquivo excede o limite para {tipo}.");

                var ext = MapExt(parsed.sub!);
                if (!ExtPermitida(tipo, ext))
                    return Result<UploadResultResponse>.Fail($"Extensão não permitida para {tipo}: .{ext}");

                var mime = GetMimeForExt(ext);

                if (tipo == "documento" && !MimeDoc.Contains(mime))
                    return Result<UploadResultResponse>.Fail($"MIME de documento não permitido: {mime}");

                var safeArea = SanitizePath(areaPath);
                var folder = $"uploads/{safeArea}/{tipo}";
                var fileName = $"{Guid.NewGuid():N}.{ext}";
                var s3Key = $"{folder}/{fileName}";

                using var ms = new MemoryStream(parsed.bytes!);
                var req = new PutObjectRequest
                {
                    InputStream = ms,
                    Key = s3Key,
                    BucketName = _opt.DefaultBucketName,
                    ContentType = mime,
                    CannedACL = S3CannedACL.Private
                };
                await _s3.PutObjectAsync(req);

                var url = S3UrlBuilder.BuildPublicUrl(
                    _opt.BaseUrl, _opt.DefaultBucketName, s3Key, _opt.ForcePathStyle, _opt.EnableSSL);

                var resp = new UploadResultResponse
                {
                    Status = "success",
                    Tipo = tipo,
                    Url = url,
                    Mime = mime,
                    Ext = ext,
                    Grupo = safeArea,
                    Ordem = 0,
                    S3Bucket = _opt.DefaultBucketName,
                    S3Key = s3Key
                };
                return Result<UploadResultResponse>.Ok(resp);
            }
            catch (Exception ex)
            {
                return Result<UploadResultResponse>.Fail($"Erro ao fazer upload: {ex.Message}");
            }
        }

        // (NOVO) Upload direto do arquivo/stream (sem base64)
        public async Task<Result<UploadResultResponse>> UploadStreamAsync(
            string areaPath,
            string tipo,
            Stream fileStream,
            string contentType,
            string? originalFileName = null,
            long? contentLength = null)
        {
            try
            {
                if (!IsValidType(tipo))
                    return Result<UploadResultResponse>.Fail("Tipo inválido. Use: imagem, video ou documento.");

                var (media, sub) = SplitContentType(contentType);
                // Se não der pra confiar no contentType, tenta pela extensão do arquivo
                var extByName = ExtFromFileName(originalFileName);
                string ext = sub is not null ? MapExt(sub) : (extByName ?? "bin");

                // Se o contentType vier genérico (application/octet-stream), tenta derivar pelo nome
                if (string.Equals(contentType, "application/octet-stream", StringComparison.OrdinalIgnoreCase) && extByName is not null)
                {
                    ext = extByName;
                    media = ext switch
                    {
                        "jpg" or "jpeg" or "png" or "gif" or "bmp" or "webp" or "svg" => "image",
                        "mp4" or "webm" or "ogg" or "avi" or "mov" or "wmv" => "video",
                        _ => "application"
                    };
                }

                if (!TipoCombina(tipo, media))
                    return Result<UploadResultResponse>.Fail("Tipo declarado não corresponde ao mediaType do arquivo.");

                if (!ExtPermitida(tipo, ext))
                    return Result<UploadResultResponse>.Fail($"Extensão não permitida para {tipo}: .{ext}");

                var mime = GetMimeForExt(ext);
                if (tipo == "documento" && !MimeDoc.Contains(mime))
                    return Result<UploadResultResponse>.Fail($"MIME de documento não permitido: {mime}");

                // Tamanho
                long size;
                if (contentLength.HasValue)
                {
                    size = contentLength.Value;
                }
                else if (fileStream.CanSeek)
                {
                    size = fileStream.Length;
                }
                else
                {
                    // copia só pra medir (raríssimo precisar)
                    using var tmp = new MemoryStream();
                    await fileStream.CopyToAsync(tmp);
                    size = tmp.Length;
                    fileStream = new MemoryStream(tmp.ToArray()); // reposiciona
                }

                if (!ValidateFileSize(tipo, size))
                    return Result<UploadResultResponse>.Fail($"Arquivo excede o limite para {tipo}.");

                var safeArea = SanitizePath(areaPath);
                var folder = $"uploads/{safeArea}/{tipo}";
                var fileName = $"{Guid.NewGuid():N}.{ext}";
                var s3Key = $"{folder}/{fileName}";

                // garante posição no início
                if (fileStream.CanSeek) fileStream.Position = 0;

                var req = new PutObjectRequest
                {
                    InputStream = fileStream,
                    Key = s3Key,
                    BucketName = _opt.DefaultBucketName,
                    ContentType = mime,
                    CannedACL = S3CannedACL.Private
                };
                await _s3.PutObjectAsync(req);

                var url = S3UrlBuilder.BuildPublicUrl(
                    _opt.BaseUrl, _opt.DefaultBucketName, s3Key, _opt.ForcePathStyle, _opt.EnableSSL);

                var resp = new UploadResultResponse
                {
                    Status = "success",
                    Tipo = tipo,
                    Url = url,
                    Mime = mime,
                    Ext = ext,
                    Grupo = safeArea,
                    Ordem = 0,
                    S3Bucket = _opt.DefaultBucketName,
                    S3Key = s3Key
                };
                return Result<UploadResultResponse>.Ok(resp);
            }
            catch (Exception ex)
            {
                return Result<UploadResultResponse>.Fail($"Erro ao fazer upload: {ex.Message}");
            }
        }
    }
}
