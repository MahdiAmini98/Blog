using Blog.Application.DTOs;
using Blog.Application.DTOs.Posts.BlogUIPosts;
using Blog.Application.Interfaces.BlogUIService;
using Blog.Domain.Entities;
using Blog.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Application.Services.BlogUIService
{
    public class BlogSidebarService : IBlogSidebarService
    {
        private readonly IRepository<Post> _postRepository;
        private readonly IRepository<Category> _categoryRepository;
        private readonly IRepository<Tag> _tagRepository;

        public BlogSidebarService(IRepository<Post> postRepository,
                                  IRepository<Category> categoryRepository,
                                  IRepository<Tag> tagRepository)
        {
            _postRepository = postRepository;
            _categoryRepository = categoryRepository;
            _tagRepository = tagRepository;
        }

        public async Task<List<BlogUIPostListDto>> GetPopularPostsAsync()
        {

            var posts = await _postRepository.FindAsync(p => p.Reactions.Any());
            var popularPosts = posts
                .OrderByDescending(p => p.Reactions.Count)
                .Take(5)
                .Select(p => new BlogUIPostListDto
                {
                    Id = p.Id,
                    Title = p.Title,
                    Description = !string.IsNullOrWhiteSpace(p.Summary)
                                    ? p.Summary
                                    : (p.Content.Length > 150 ? p.Content.Substring(0, 150) + "..." : p.Content),
                    ImageUrl = p.ThumbnailUrl ?? string.Empty,
                    PublishedDate = p.PublishedDate,
                    Author = p.Author?.Name ?? "ناشناس",
                    Slug = p.Slug,
                })
                .ToList();
            return popularPosts;
        }

        public async Task<List<CategoryDto>> GetCategoriesAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();
            return categories.Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name
            }).ToList();
        }

        public async Task<List<TagDto>> GetTagsAsync()
        {
            var tags = await _tagRepository.GetAllAsync();
            return tags.Select(t => new TagDto
            {
                Id = t.Id,
                Name = t.Name
            }).ToList();
        }
    }
}
