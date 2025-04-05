using Blog.Application.Contracts.Reactions;
using Blog.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Blog.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ReactionsController : ControllerBase
    {
        private readonly IReactionService _reactionService;

        private readonly ILogger<ReactionsController> _logger;

        public ReactionsController(IReactionService reactionService, ILogger<ReactionsController> logger)
        {
            _reactionService = reactionService;
            _logger = logger;
        }


        [HttpGet]
        public async Task<IActionResult> GetAllReactions([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            if (page <= 0 || pageSize <= 0)
                return BadRequest(new { message = "Page and pageSize must be greater than zero." });

            try
            {
                var reactions = await _reactionService.GetAllReactionsAsync(page, pageSize);
                return Ok(reactions);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An unexpected error occurred while processing the request.");


                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    error = "InternalServerError",
                    message = "An unexpected error occurred. Please try again later."
                });
            }

        }

        [HttpGet("post/{postId}")]
        public async Task<IActionResult> GetReactionsByPostId(Guid postId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            if (postId == Guid.Empty)
                return BadRequest(new { message = "Invalid post ID." });

            if (page <= 0 || pageSize <= 0)
                return BadRequest(new { message = "Page and pageSize must be greater than zero." });

            try
            {
                var reactions = await _reactionService.GetReactionsByPostIdAsync(postId, page, pageSize);
                return Ok(reactions);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = "PostNotFound", message = ex.Message });
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An unexpected error occurred while processing the request.");


                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    error = "InternalServerError",
                    message = "An unexpected error occurred. Please try again later."
                });
            }

        }

        [HttpPost]
        public async Task<IActionResult> AddReaction([FromBody] CreateReactionRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var reaction = await _reactionService.AddReactionAsync(request);
                return CreatedAtAction(nameof(GetReactionsByPostId), new { postId = reaction.PostId }, reaction);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = "NotFound", message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = "InvalidData", message = ex.Message });
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An unexpected error occurred while processing the request.");


                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    error = "InternalServerError",
                    message = "An unexpected error occurred. Please try again later."
                });
            }

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReaction(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest(new { message = "Invalid reaction ID." });

            try
            {
                await _reactionService.DeleteReactionAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = "ReactionNotFound", message = "Reaction Not Found." });
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "An unexpected error occurred while processing the request.");


                return StatusCode(StatusCodes.Status500InternalServerError, new
                {
                    error = "InternalServerError",
                    message = "An unexpected error occurred. Please try again later."
                });
            }

        }
    }

}
