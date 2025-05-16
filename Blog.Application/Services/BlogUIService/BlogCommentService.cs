using Blog.Application.DTOs;
using Blog.Application.Interfaces.BlogUIService;
using Blog.Domain.Entities;
using Blog.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Application.Services.BlogUIService
{
    public class BlogCommentService : IBlogCommentService
    {
        private readonly IRepository<Comment> _commentRepository;
        private readonly IUnitOfWork _unitOfWork;

        public BlogCommentService(IRepository<Comment> commentRepository, IUnitOfWork unitOfWork)
        {
            _commentRepository = commentRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<CommentDto> AddCommentAsync(Guid postId, NewCommentModel newComment, Guid userId)
        {

            var comment = new Comment(newComment.Content, postId, userId);
            _commentRepository.Add(comment);
            await _unitOfWork.CommitAsync();

            var dto = new CommentDto
            {
                Id = comment.Id,
                AuthorId = userId,
                Content = comment.Content,
                CreatedDate = comment.CreatedDate
            };

            return dto;
        }
    }
}
