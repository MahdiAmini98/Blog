using Blog.Application.Interfaces.BlogUIService;
using Blog.Domain.Entities;
using Blog.Domain.Enumerations;
using Blog.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Application.Services.BlogUIService
{
    public class BlogUIReactionService : IBlogUIReactionService
    {
        private readonly IRepository<Reaction> _reactionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public BlogUIReactionService(IRepository<Reaction> reactionRepository, IUnitOfWork unitOfWork)
        {
            _reactionRepository = reactionRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task ToggleLikeAsync(Guid postId, Guid userId)
        {

            var existing = (await _reactionRepository.FindAsync(
                r => r.PostId == postId && r.UserId == userId && r.Type == ReactionType.Like))
                .FirstOrDefault();
            if (existing != null)
            {
                _reactionRepository.Remove(existing);
            }
            else
            {
                var newReaction = new Reaction(postId, userId, ReactionType.Like);
                _reactionRepository.Add(newReaction);
            }
            await _unitOfWork.CommitAsync();
        }
        public async Task<bool> HasUserLikedAsync(Guid postId, Guid userId)
        {
            var reaction = (await _reactionRepository.FindAsync(
                r => r.PostId == postId && r.UserId == userId && r.Type == ReactionType.Like))
                .FirstOrDefault();
            return reaction != null;
        }
        public async Task<int> GetLikeCountAsync(Guid postId)
        {
            return await _reactionRepository.CountAsync(
                r => r.PostId == postId && r.Type == ReactionType.Like);
        }
    }
}
