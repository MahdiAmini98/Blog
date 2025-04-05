using Blog.Application.Contracts.Comments;
using Blog.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Application.Interfaces
{
    public interface ICommentService
    {
        Task<CommentDto> CreateCommentAsync(CreateCommentRequest request);
        Task<IEnumerable<CommentDto>> GetCommentsByPostIdAsync(Guid postId);
        Task<CommentDto> GetCommentByIdAsync(Guid id);
        Task UpdateCommentAsync(Guid id, UpdateCommentRequest request);
        Task DeleteCommentAsync(Guid id);
    }
}
