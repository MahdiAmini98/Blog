using Blog.Application.Contracts.Comments;
using Blog.Application.DTOs;
using Blog.Application.Interfaces;
using Blog.Domain.Entities;
using Blog.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Blog.Application.Services
{
    public class CommentService : ICommentService
    {
        private readonly IRepository<Comment> _commentRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CommentService(IRepository<Comment> commentRepository, IUnitOfWork unitOfWork)
        {
            _commentRepository = commentRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<CommentDto> CreateCommentAsync(CreateCommentRequest request)
        {
            var comment = new Comment(request.Content, request.PostId, request.AuthorId);
            _commentRepository.Add(comment);
            await _unitOfWork.CommitAsync();

            return new CommentDto
            {
                Id = comment.Id,
                Content = comment.Content,
                PostId = comment.PostId,
                AuthorId = comment.AuthorId,
                CreatedDate = comment.CreatedDate
            };
        }

        public async Task<IEnumerable<CommentDto>> GetCommentsByPostIdAsync(Guid postId)
        {
            var comments = await _commentRepository.FindAsync(c => c.PostId == postId);
            return comments.Select(c => new CommentDto
            {
                Id = c.Id,
                Content = c.Content,
                PostId = c.PostId,
                AuthorId = c.AuthorId,
                CreatedDate = c.CreatedDate
            });
        }

        public async Task<CommentDto> GetCommentByIdAsync(Guid id)
        {
            var comment = await _commentRepository.GetByIdAsync(id);
            if (comment == null)
                throw new KeyNotFoundException($"Comment with ID {id} not found.");

            return new CommentDto
            {
                Id = comment.Id,
                Content = comment.Content,
                PostId = comment.PostId,
                AuthorId = comment.AuthorId,
                CreatedDate = comment.CreatedDate
            };
        }

        public async Task UpdateCommentAsync(Guid id, UpdateCommentRequest request)
        {
            var comment = await _commentRepository.GetByIdAsync(id);
            if (comment == null)
                throw new KeyNotFoundException($"Comment with ID {id} not found.");

            comment.SetContent(request.Content);
            comment.UpdateLastModifiedDate();

            _commentRepository.Update(comment);
            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteCommentAsync(Guid id)
        {
            var comment = await _commentRepository.GetByIdAsync(id);
            if (comment == null)
                throw new KeyNotFoundException($"Comment with ID {id} not found.");

            _commentRepository.Remove(comment);
            await _unitOfWork.CommitAsync();
        }
    }
}
