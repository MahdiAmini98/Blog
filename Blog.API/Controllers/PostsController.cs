using Blog.API.Contracts;
using Blog.Application.Contracts.Posts;
using Blog.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Blog.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly ILogger<PostsController> _logger;

        public PostsController(IPostService postService, ILogger<PostsController> logger)
        {
            _postService = postService;
            _logger = logger;
        }

        // GET: api/posts
        [HttpGet]
        public async Task<IActionResult> GetPosts([FromQuery] PostQueryParameters queryParameters)
        {
            try
            {
                var posts = await _postService.GetAllPostsAsync(queryParameters);
                return Ok(posts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching posts.");
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse
                {
                    Error = "InternalServerError",
                    Message = "An unexpected error occurred. Please try again later."
                });
            }
        }

        // GET: api/posts/{id}
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetPostById(Guid id)
        {
            try
            {
                var post = await _postService.GetPostByIdAsync(id);
                return Ok(post);
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new ErrorResponse
                {
                    Error = "PostNotFound",
                    Message = "The requested post was not found."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while fetching post with ID {id}.");
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse
                {
                    Error = "InternalServerError",
                    Message = "An unexpected error occurred. Please try again later."
                });
            }
        }

        // POST: api/posts
        [HttpPost]
        public async Task<IActionResult> CreatePost([FromBody] CreatePostRequest request)
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
                var post = await _postService.CreatePostAsync(request);
                return CreatedAtAction(nameof(GetPostById), new { id = post.Id }, post);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating a post.");
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse
                {
                    Error = "InternalServerError",
                    Message = "An unexpected error occurred. Please try again later."
                });
            }
        }

        // PUT: api/posts/{id}
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdatePost(Guid id, [FromBody] UpdatePostRequest request)
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
                await _postService.UpdatePostAsync(id, request);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new ErrorResponse
                {
                    Error = "PostNotFound",
                    Message = "The requested post was not found."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while updating post with ID {id}.");
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse
                {
                    Error = "InternalServerError",
                    Message = "An unexpected error occurred. Please try again later."
                });
            }
        }

        // DELETE: api/posts/{id}
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeletePost(Guid id)
        {
            try
            {
                await _postService.DeletePostAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new ErrorResponse
                {
                    Error = "PostNotFound",
                    Message = "The requested post was not found."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An error occurred while deleting post with ID {id}.");
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponse
                {
                    Error = "InternalServerError",
                    Message = "An unexpected error occurred. Please try again later."
                });
            }
        }
    }
}
