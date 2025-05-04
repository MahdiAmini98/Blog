using Blog.PanelAdmin.Models.Pagination;
using Blog.PanelAdmin.Models.Posts;
using System.Net.Http.Json;
using System.Web;

namespace Blog.PanelAdmin.Services.Posts
{
    public interface IPostService
    {
        Task<PaginatedList<PostListResponseDto>> GetPostsForListAsync(PostQueryParameters postQueryParameters);
        Task<bool> DeletePostAsync(Guid id);
        Task<bool> CreatePostAsync(CreatePostRequestDto post);
        Task<bool> UpdatePostAsync(UpdatePostRequestDto post);
        Task<PostDto?> GetPostByIdAsync(Guid id);
    }

    public class PostService : IPostService
    {
        private readonly HttpClient _httpClient;

        public PostService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> CreatePostAsync(CreatePostRequestDto post)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/posts", post);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeletePostAsync(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"/api/posts/{id}");
            return response.IsSuccessStatusCode;
        }

        public async Task<PostDto?> GetPostByIdAsync(Guid id)
        {
            return await _httpClient.GetFromJsonAsync<PostDto>($"/api/posts/{id}");
        }

        public async Task<PaginatedList<PostListResponseDto>> GetPostsForListAsync(PostQueryParameters postQueryParameters)
        {
            var queryParams = HttpUtility.ParseQueryString(string.Empty);
            queryParams["page"] = postQueryParameters.Page.ToString();
            queryParams["pageSize"] = postQueryParameters.PageSize.ToString();
            if (!string.IsNullOrEmpty(postQueryParameters.SearchText))
            {
                queryParams["searchText"] = postQueryParameters.SearchText;
            }
            if (!string.IsNullOrEmpty(postQueryParameters.Tag))
            {
                queryParams["tag"] = postQueryParameters.Tag;
            }
            if (postQueryParameters.FromDate.HasValue)
            {
                queryParams["fromDate"] = postQueryParameters.FromDate.Value.ToString("yyyy-MM-dd");
            }
            if (postQueryParameters.ToDate.HasValue)
            {
                queryParams["toDate"] = postQueryParameters.ToDate.Value.ToString("yyyy-MM-dd");
            }

            var url = $"/api/posts?{queryParams}";
            var response = await _httpClient.GetFromJsonAsync<PaginatedList<PostListResponseDto>>(url);
            return response ?? new PaginatedList<PostListResponseDto>(
                new List<PostListResponseDto>(),
                0,
                postQueryParameters.Page,
                postQueryParameters.PageSize);


        }

        public async Task<bool> UpdatePostAsync(UpdatePostRequestDto post)
        {
            var response = await _httpClient.PutAsJsonAsync($"/api/posts/{post.Id}", post);
            return response.IsSuccessStatusCode;
        }
    }
}
