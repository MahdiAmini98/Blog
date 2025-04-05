using Blog.API.Contracts;
using Blog.Application.Contracts.Medias;
using Blog.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Blog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MediaController : ControllerBase
    {
        private readonly IMediaService _mediaService;
        private readonly ILogger<MediaController> _logger;

        public MediaController(IMediaService mediaService, ILogger<MediaController> logger)
        {
            _mediaService = mediaService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMedia([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var media = await _mediaService.GetAllMediaAsync(page, pageSize);
                return Ok(media);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching media list.");
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse
                {
                    Error = "InternalServerError",
                    Message = "An unexpected error occurred. Please try again later."
                });
            }
        }

        [HttpPost]
        public async Task<IActionResult> UploadMedia([FromBody] UploadMediaRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ErrorResponse
                {
                    Error = "InvalidData",
                    Message = "The provided data is invalid."
                });
            }

            try
            {
                var media = await _mediaService.UploadMediaAsync(request);
                return CreatedAtAction(nameof(GetMediaById), new { id = media.Id }, media);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while uploading media.");
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse
                {
                    Error = "InternalServerError",
                    Message = "An unexpected error occurred. Please try again later."
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMediaById(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest(new ErrorResponse
                {
                    Error = "InvalidId",
                    Message = "The provided media ID is invalid."
                });
            }

            try
            {
                var media = await _mediaService.GetMediaByIdAsync(id);
                return Ok(media);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new ErrorResponse
                {
                    Error = "MediaNotFound",
                    Message = "The requested media was not found."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while fetching media with ID {id}.");
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse
                {
                    Error = "InternalServerError",
                    Message = "An unexpected error occurred. Please try again later."
                });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMedia(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest(new ErrorResponse
                {
                    Error = "InvalidId",
                    Message = "The provided media ID is invalid."
                });
            }

            try
            {
                await _mediaService.DeleteMediaAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new ErrorResponse
                {
                    Error = "MediaNotFound",
                    Message = "The requested media was not found."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting media with ID {id}.");
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse
                {
                    Error = "InternalServerError",
                    Message = "An unexpected error occurred. Please try again later."
                });
            }
        }
    }
}
