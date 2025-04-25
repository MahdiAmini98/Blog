using Blog.PanelAdmin.Models.Categories;
using System.Net.Http.Json;

namespace Blog.PanelAdmin.Services.Categories
{
    public interface ICategoryService
    {
        Task<List<CategoryDto>> GetCategoriesAsync();
        Task<CategoryDto> GetCategoryByIdAsync(Guid id);
        Task<CategoryDto> CreateCategoryAsync(CreateCategoryRequest request);
        Task UpdateCategoryAsync(Guid id, UpdateCategoryRequest request);
        Task DeleteCategoryAsync(Guid id);
    }

    public class CategoryService : ICategoryService
    {
        private readonly HttpClient _httpClient;

        public CategoryService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<CategoryDto>> GetCategoriesAsync()
        {
            var response = await _httpClient.GetFromJsonAsync<List<CategoryDto>>("/api/Categories");
            return response ?? new List<CategoryDto>();
        }

        public async Task<CategoryDto> GetCategoryByIdAsync(Guid id)
        {
            var response = await _httpClient.GetAsync($"/api/Categories/{id}");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<CategoryDto>();
            }
            throw new HttpRequestException($"Error fetching category with ID {id}: {response.ReasonPhrase}");
        }

        public async Task<CategoryDto> CreateCategoryAsync(CreateCategoryRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/Categories", request);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<CategoryDto>();
            }
            throw new HttpRequestException($"Error creating category: {response.ReasonPhrase}");
        }

        public async Task UpdateCategoryAsync(Guid id, UpdateCategoryRequest request)
        {
            var response = await _httpClient.PutAsJsonAsync($"/api/Categories/{id}", request);
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Error updating category with ID {id}: {response.ReasonPhrase}");
            }
        }

        public async Task DeleteCategoryAsync(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"/api/Categories/{id}");
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Error deleting category with ID {id}: {response.ReasonPhrase}");
            }
        }
    }
}
