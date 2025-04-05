using Blog.Application.Contracts.Comments;
using Blog.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Blog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentsController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        /// <summary>
        /// دریافت لیست کامنت‌ها بر اساس شناسه پست
        /// </summary>
        [HttpGet("post/{postId}")]
        public async Task<IActionResult> GetCommentsByPostId(Guid postId)
        {
            var comments = await _commentService.GetCommentsByPostIdAsync(postId);
            return Ok(comments);
        }

        /// <summary>
        /// دریافت کامنت بر اساس شناسه
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCommentById(Guid id)
        {
            if (id == Guid.Empty)
                return BadRequest(new { message = "Invalid comment ID." });


            try
            {
                var comment = await _commentService.GetCommentByIdAsync(id);
                return Ok(comment);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = "Comment NotFound." });
            }
        }


        /// <summary>
        /// به‌روزرسانی کامنت
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateComment(Guid id, UpdateCommentRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);


            await _commentService.UpdateCommentAsync(id, request);
            return NoContent();
        }

        /// <summary>
        /// حذف کامنت
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(Guid id)
        {
            try
            {
                await _commentService.DeleteCommentAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {

                return NotFound(new { message = $"The requested {id} comment was not found." });
            }
        }
    }
}
