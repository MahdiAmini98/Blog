using Blog.Application.DTOs;
using Blog.Application.DTOs.Posts.BlogUIPosts;


namespace Blog.Application.Interfaces.BlogUIService
{
    public interface IBlogUIPostService
    {
        Task<PaginatedList<BlogUIPostListDto>> GetPostsAsync(
                    int pageIndex,
                    int pageSize,
                    string? searchKey = null,
                    string? categorySlug = null,
                    string? tagSlug = null,
                    DateTime? fromDate = null,
                    DateTime? toDate = null);
        Task<PostDetailDto> GetPostDetailBySlugAsync(string slug);
    }
}
