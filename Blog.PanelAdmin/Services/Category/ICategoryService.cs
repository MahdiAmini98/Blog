﻿using Blog.PanelAdmin.Models.Category;
using System.Net.Http.Json;

namespace Blog.PanelAdmin.Services.Category
{
    public interface ICategoryService
    {
        Task<List<CategoryDto>> GetCategoriesAsync();
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
    }
}
