using Blog.Application.Contracts.Reactions;
using Blog.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Application.Interfaces
{
    public interface IReactionService
    {
        Task<IEnumerable<ReactionDto>> GetAllReactionsAsync(int page, int pageSize);
        Task<IEnumerable<ReactionDto>> GetReactionsByPostIdAsync(Guid postId, int page, int pageSize);
        Task<ReactionDto> AddReactionAsync(CreateReactionRequest request);
        Task DeleteReactionAsync(Guid id);
    }
}
