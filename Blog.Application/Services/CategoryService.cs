using Blog.Application.Contracts.Category;
using Blog.Application.DTOs;
using Blog.Application.Interfaces;
using Blog.Domain.Entities;
using Blog.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Application.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IRepository<Category> _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(IRepository<Category> categoryRepository, IUnitOfWork unitOfWork)
        {
            _categoryRepository = categoryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();
            return categories.Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                CreatedDate = c.CreatedDate,
                LastModifiedDate = c.LastModifiedDate
            });
        }

        public async Task<CategoryDto> GetCategoryByIdAsync(Guid id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
                throw new KeyNotFoundException($"Category with ID {id} not found.");

            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                CreatedDate = category.CreatedDate,
                LastModifiedDate = category.LastModifiedDate
            };
        }

        public async Task<CategoryDto> CreateCategoryAsync(CreateCategoryRequest request)
        {
            // استفاده از سازنده ریچ برای تضمین صحت داده
            var category = new Category(request.Name, request.Description);

            _categoryRepository.Add(category);
            await _unitOfWork.CommitAsync();

            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                CreatedDate = category.CreatedDate
            };
        }

        public async Task UpdateCategoryAsync(Guid id, UpdateCategoryRequest request)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
                throw new KeyNotFoundException($"Category with ID {id} not found.");

            // استفاده از متدهای داخلی انتیتی برای اعمال تغییرات
            category.SetName(request.Name);
            category.SetDescription(request.Description); // استفاده از متد داخلی

            category.UpdateLastModifiedDate(); // به‌روزرسانی تاریخ تغییر

            _categoryRepository.Update(category);
            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteCategoryAsync(Guid id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
                throw new KeyNotFoundException($"Category with ID {id} not found.");

            _categoryRepository.Remove(category);
            await _unitOfWork.CommitAsync();
        }
    }
}
