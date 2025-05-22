
namespace Blog.Application.Interfaces.BlogUIService
{
    public interface IBlogUIReactionService
    {
        Task ToggleLikeAsync(Guid postId, Guid userId);
        Task<bool> HasUserLikedAsync(Guid postId, Guid userId);
        Task<int> GetLikeCountAsync(Guid postId);
    }
}
