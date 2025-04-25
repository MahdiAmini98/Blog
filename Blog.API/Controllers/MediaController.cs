using Blog.API.Contracts;
using Blog.Application.Contracts.Medias;
using Blog.Application.DTOs;
using Blog.Application.Interfaces;
using Blog.Application.Interfaces.FileStorage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Blog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MediaController : ControllerBase
    {
        private readonly IMediaService _mediaService;
        private readonly IFileStorageService _fileStorageService;

        public MediaController(IMediaService mediaService, IFileStorageService fileStorageService)
        {
            _mediaService = mediaService;
            _fileStorageService = fileStorageService;
        }

        /// <summary>
        /// آپلود یک فایل جدید
        /// </summary>
        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        [RequestSizeLimit(500 * 1024 * 1024)] // محدودیت 20MB
        public async Task<IActionResult> UploadMedia([FromForm] UploadMediaRequestDto request)
        {
            if (request.Files == null || request.Files.Count == 0)
            {
                return BadRequest(new { Message = "هیچ فایلی انتخاب نشده است." });
            }

            var userId = GetAuthenticatedUserId();
            if (userId == Guid.Empty)
            {
                return Unauthorized(new { Message = "خطای احراز هویت." });
            }

            var uploadedMedia = new List<MediaDto>();

            foreach (var file in request.Files)
            {
                string fileExtension = Path.GetExtension(file.FileName).ToLower();
                string fileType = GetFileType(fileExtension);

                // تغییر نام فایل: اضافه کردن GUID برای یکتایی
                string uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";

                using var stream = file.OpenReadStream();
                string fileUrl = await _fileStorageService.UploadFileAsync(stream, uniqueFileName);

                var mediaRequest = new UploadMediaRequest
                {
                    Url = fileUrl,
                    Type = fileType,
                    UploadedBy = userId
                };

                var mediaDto = await _mediaService.UploadMediaAsync(mediaRequest);
                uploadedMedia.Add(mediaDto);
            }

            return Ok(uploadedMedia); // لیست فایل‌های آپلود شده را برمی‌گرداند
        }


        /// <summary>
        /// دریافت لیست فایل‌ها   
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllMedia([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var mediaList = await _mediaService.GetAllMediaAsync(page, pageSize);
            return Ok(mediaList);
        }


        /// <summary>
        /// دریافت یک فایل بر اساس ای دی
        /// </summary>
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetMediaById(Guid id)
        {
            try
            {
                var media = await _mediaService.GetMediaByIdAsync(id);
                return Ok(media);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new { Message = "فایل موردنظر یافت نشد." });
            }
        }

        /// <summary>
        /// حذف یک فایل   
        /// </summary>
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteMedia(Guid id)
        {
            var userId = GetAuthenticatedUserId();
            if (userId == Guid.Empty)
            {
                return Unauthorized(new { Message = "خطای احراز هویت." });
            }

            var media = await _mediaService.GetMediaByIdAsync(id);
            if (media == null)
            {
                return NotFound(new { Message = "فایل موردنظر یافت نشد." });
            }

            //if (media.UploadedById != userId)
            //{
            //    return Forbid(); // فقط کاربر آپلودکننده مجاز به حذف است
            //}

            await _fileStorageService.DeleteFileAsync(media.Url);
            await _mediaService.DeleteMediaAsync(id);

            return NoContent();
        }

        /// <summary>
        /// استخراج شناسه کاربر از توکن احراز هویت شده
        /// </summary>
        private Guid GetAuthenticatedUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Guid.TryParse(userIdClaim, out var userId) ? userId : Guid.Empty;
        }

        /// <summary>
        /// تعیین نوع فایل بر اساس پسوند
        /// </summary>
        private string GetFileType(string fileExtension)
        {
            return fileExtension switch
            {
                // 📷 Image Formats
                ".jpg" or ".jpeg" or ".png" or ".gif" or ".bmp" or ".tiff" or ".webp" or ".svg" => "image",

                // 🎥 Video Formats
                ".mp4" or ".avi" or ".mov" or ".mkv" or ".wmv" or ".flv" or ".webm" or ".mpeg" or ".3gp" => "video",

                // 📄 Document Formats
                ".pdf" or ".doc" or ".docx" or ".xls" or ".xlsx" or ".ppt" or ".pptx" or ".txt" or ".csv" => "document",

                // 🎵 Audio Formats
                ".mp3" or ".wav" or ".aac" or ".flac" or ".ogg" or ".m4a" or ".wma" => "audio",

                // 📦 Archive Formats
                ".zip" or ".rar" or ".7z" or ".tar" or ".gz" or ".bz2" or ".xz" => "archive",

                // ❌ Unknown Format - Throw Exception
                _ => throw new NotSupportedException($"The file format '{fileExtension}' is not supported.")
            };
        }
    }

    public class UploadMediaRequestDto
    {
        public List<IFormFile> Files { get; set; } = new();
    }
}
