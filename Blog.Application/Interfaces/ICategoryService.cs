using Blog.Application.Contracts.Category;
using Blog.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Application.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync();
        Task<CategoryDto> GetCategoryByIdAsync(Guid id);
        Task<CategoryDto> CreateCategoryAsync(CreateCategoryRequest request);
        Task UpdateCategoryAsync(Guid id, UpdateCategoryRequest request);
        Task DeleteCategoryAsync(Guid id);
    }
}
