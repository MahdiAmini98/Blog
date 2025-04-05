using Blog.Application.Contracts.Reactions;
using Blog.Application.DTOs;
using Blog.Application.Interfaces;
using Blog.Domain.Entities;
using Blog.Domain.Enumerations;
using Blog.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace Blog.Application.Services
{
    public class ReactionService : IReactionService
    {
        private readonly IRepository<Reaction> _reactionRepository;
        private readonly IRepository<Post> _postRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ReactionService(
            IRepository<Reaction> reactionRepository,
            IRepository<Post> postRepository,
            IRepository<User> userRepository,
            IUnitOfWork unitOfWork)
        {
            _reactionRepository = reactionRepository;
            _postRepository = postRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<ReactionDto>> GetAllReactionsAsync(int page, int pageSize)
        {
            var reactions = _reactionRepository.Query()
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return reactions.Select(r => new ReactionDto
            {
                Id = r.Id,
                PostId = r.PostId,
                UserId = r.UserId,
                ReactionType = r.Type.ToString(),
                CreatedDate = r.CreatedDate
            });
        }

        public async Task<IEnumerable<ReactionDto>> GetReactionsByPostIdAsync(Guid postId, int page, int pageSize)
        {
            var reactions = _reactionRepository.Query()
                .Where(r => r.PostId == postId)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return reactions.Select(r => new ReactionDto
            {
                Id = r.Id,
                PostId = r.PostId,
                UserId = r.UserId,
                ReactionType = r.Type.ToString(),
                CreatedDate = r.CreatedDate
            });
        }

        public async Task<ReactionDto> AddReactionAsync(CreateReactionRequest request)
        {
            var post = await _postRepository.GetByIdAsync(request.PostId);
            if (post == null)
                throw new KeyNotFoundException($"Post with ID {request.PostId} not found.");

            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user == null)
                throw new KeyNotFoundException($"User with ID {request.UserId} not found.");

            if (!Enum.TryParse<ReactionType>(request.ReactionType, out var reactionType))
                throw new ArgumentException("Invalid reaction type.", nameof(request.ReactionType));

            var reaction = new Reaction(post.Id, user.Id, reactionType);
            _reactionRepository.Add(reaction);
            await _unitOfWork.CommitAsync();

            return new ReactionDto
            {
                Id = reaction.Id,
                PostId = reaction.PostId,
                UserId = reaction.UserId,
                ReactionType = reaction.Type.ToString(),
                CreatedDate = reaction.CreatedDate
            };
        }

        public async Task DeleteReactionAsync(Guid id)
        {
            var reaction = await _reactionRepository.GetByIdAsync(id);
            if (reaction == null)
                throw new KeyNotFoundException($"Reaction with ID {id} not found.");

            _reactionRepository.Remove(reaction);
            await _unitOfWork.CommitAsync();
        }


    }
}
