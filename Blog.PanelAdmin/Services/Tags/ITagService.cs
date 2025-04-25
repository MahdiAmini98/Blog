using Blog.PanelAdmin.Models.Tags;
using System.Net.Http.Json;

namespace Blog.PanelAdmin.Services.Tag
{
    public interface ITagService
    {
        Task<List<TagDto>> GetTagsAsync();
        Task<TagDto> GetTagByIdAsync(Guid id);
        Task<TagDto> CreateTagAsync(CreateTagRequestDto request);
        Task UpdateTagAsync(Guid id, UpdateTagRequestDto request);
        Task DeleteTagAsync(Guid id);
    }


    public class TagService : ITagService
    {
        private readonly HttpClient _httpClient;

        public TagService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<TagDto> CreateTagAsync(CreateTagRequestDto request)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/Tag", request);
            return await response.Content.ReadFromJsonAsync<TagDto>()?? new();
        }

        public async Task DeleteTagAsync(Guid id)
        {
            await _httpClient.DeleteAsync($"api/Tag/{id}");
        }

        public async Task<TagDto> GetTagByIdAsync(Guid id)
        {
           return await _httpClient.GetFromJsonAsync<TagDto>($"api/Tag/{id}")??new();
        }

        public async Task<List<TagDto>> GetTagsAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<TagDto>>("api/Tag") ?? new();
        }

        public async Task UpdateTagAsync(Guid id, UpdateTagRequestDto request)
        {
            await _httpClient.PutAsJsonAsync($"/api/Tag/{id}", request);
        }
    }
}
