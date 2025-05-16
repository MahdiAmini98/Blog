using Blog.Application.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Application.Interfaces.BlogUIService
{
    public interface IBlogCommentService
    {
        Task<CommentDto> AddCommentAsync(Guid postId, NewCommentModel newComment, Guid userId);
    }

    public class NewCommentModel
    {
        [Required(ErrorMessage = "وارد کردن پیام الزامی است.")]
        public string Content { get; set; }
    }
}
