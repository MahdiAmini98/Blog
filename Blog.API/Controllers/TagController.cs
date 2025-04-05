using Blog.API.Contracts;
using Blog.Application.Contracts.Tags;
using Blog.Application.DTOs;
using Blog.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Blog.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class TagController : ControllerBase
    {
        private readonly ITagService _tagService;
        private readonly ILogger<TagController> _logger;

        public TagController(ITagService tagService, ILogger<TagController> logger)
        {
            _tagService = tagService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TagDto>>> GetAllTags()
        {
            try
            {
                var tags = await _tagService.GetAllTagsAsync();
                return Ok(tags);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting all tags.");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    error = "InternalServerError",
                    message = "An unexpected error occurred. Please try again later."
                });
            }
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<TagDto>> GetTagById(Guid id)
        {
            try
            {
                var tag = await _tagService.GetTagByIdAsync(id);
                return Ok(tag);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new ErrorResponse
                {
                    Error = "TagNotFound",
                    Message = "The requested tag was not found."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while getting tag with ID {id}.");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    error = "InternalServerError",
                    message = "An unexpected error occurred. Please try again later."
                });
            }
        }

        [HttpPost]
        public async Task<ActionResult<TagDto>> CreateTag([FromBody] CreateTagRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ErrorResponse
                {
                    Error = "InvalidData",
                    Message = "The provided data is invalid."
                });

            try
            {
                var createdTag = await _tagService.CreateTagAsync(request);
                return CreatedAtAction(nameof(GetTagById), new { id = createdTag.Id }, createdTag);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a new tag.");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    error = "InternalServerError",
                    message = "An unexpected error occurred. Please try again later."
                });
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateTag(Guid id, [FromBody] UpdateTagRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(new
                {
                    error = "InvalidData",
                    message = "The provided data is invalid."
                });

            try
            {
                await _tagService.UpdateTagAsync(id, request);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new ErrorResponse
                {
                    Error = "TagNotFound",
                    Message = "The requested tag was not found."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating tag with ID {id}.");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    error = "InternalServerError",
                    message = "An unexpected error occurred. Please try again later."
                });
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteTag(Guid id)
        {
            try
            {
                await _tagService.DeleteTagAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {

                return NotFound(new ErrorResponse
                {

                    Error = "TagNotFound",
                    Message = "The requested tag was not found."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting tag with ID {id}.");
                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    error = "InternalServerError",
                    message = "An unexpected error occurred. Please try again later."
                });
            }
        }
    }
}
