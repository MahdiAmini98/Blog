using Blog.Application.DTOs;
using Blog.Application.DTOs.Posts.BlogUIPosts;
using Blog.Application.DTOs.Posts.WebUIPosts;


namespace Blog.Application.Interfaces.BlogUIService
{
    public interface IBlogUIPostService
    {
        Task<PaginatedList<BlogUIPostListDto>> GetPostsAsync(int pageIndex, int pageSize, string? searchKey = null);
        Task<PostDetailDto> GetPostDetailBySlugAsync(string slug);
    }
}
