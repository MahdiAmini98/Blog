using Blog.Application.DTOs;
using Blog.Application.DTOs.Posts.WebUIPosts;


namespace Blog.Application.Interfaces.WebUIService
{
    public interface IWebUIPostService
    {
        Task<PaginatedList<WebUIPostListDto>> GetPostsAsync(int pageIndex, int pageSize, string? searchKey = null);
        Task<PostDetailDto> GetPostDetailBySlugAsync(string slug);
    }
}
