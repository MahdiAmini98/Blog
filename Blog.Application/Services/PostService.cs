using Blog.Application.Contracts.Posts;
using Blog.Application.DTOs;
using Blog.Application.DTOs.Posts;
using Blog.Application.Interfaces;
using Blog.Domain.Entities;
using Blog.Domain.Interfaces;
using Blog.Domain.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Application.Services
{
    public class PostService : IPostService
    {
        private readonly IRepository<Post> _postRepository;
        private readonly IRepository<Category> _categoryRepository;
        private readonly IRepository<Tag> _tagRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<User> _userRepository;
        public PostService(IRepository<Post> postRepository, IUnitOfWork unitOfWork
            , IRepository<Category> categoryRepository
            , IRepository<Tag> tagRepository, IRepository<User> userRepository)
        {
            _postRepository = postRepository;
            _unitOfWork = unitOfWork;
            _categoryRepository = categoryRepository;
            _tagRepository = tagRepository;
            _userRepository = userRepository;
        }

        public async Task<PostDto> CreatePostAsync(CreatePostRequest request)
        {
            var post = Post.Create(
                request.Title,
                request.Content,
                request.MetaTitle,
                request.MetaDescription,
                request.AuthorId
            );

            if (!string.IsNullOrEmpty(request.ThumbnailUrl))
            {
                post.SetThumbnailUrl(request.ThumbnailUrl);
            }

            if (!string.IsNullOrEmpty(request.Summary))
            {
                post.SetSummary(request.Summary);
            }
            post.ChangeStatus(request.Status);
            post.SetSlug(request.Slug);


            // اضافه کردن دسته‌بندی‌ها:
            foreach (var categoryId in request.CategoryIds)
            {
                var category = await _categoryRepository.GetByIdAsync(categoryId);
                if (category != null)
                {
                    post.AddCategory(category);
                }
            }

            // اضافه کردن برچسب‌ها:
            foreach (var tagId in request.TagIds)
            {
                var tag = await _tagRepository.GetByIdAsync(tagId);
                if (tag != null)
                {
                    post.AddTag(tag);
                }
            }


            _postRepository.Add(post);
            await _unitOfWork.CommitAsync();

            return new PostDto
            {
                Id = post.Id,
                Title = post.Title,
                Content = post.Content,
                MetaTitle = post.MetaTitle,
                MetaDescription = post.MetaDescription,
                PublishedDate = post.PublishedDate,
                ThumbnailUrl = post.ThumbnailUrl,
                Summary = post.Summary,
            };
        }


        public async Task<PaginatedList<PostListDto>> GetAllPostsAsync(PostQueryParameters queryParameters)
        {
            var spec = new PostFilterSpecification(
                queryParameters.Tag,
                queryParameters.SearchText,
                queryParameters.FromDate,
                queryParameters.ToDate
            );

            var totalCount = await _postRepository.CountWithSpecificationAsync(spec);
            var posts = await _postRepository.FindWithSpecificationPagedAsync(spec, queryParameters.Page, queryParameters.PageSize);

            var postListDtos = posts.Select(post => new PostListDto
            {
                Id = post.Id,
                Title = post.Title,
                PublishedDate = post.PublishedDate,
                Summary = post.Summary,
                AuthorName = post.Author != null ? post.Author.Name : null,
                ThumbnailUrl = post.ThumbnailUrl,
                ViewCount = post.ViewCount,
                Status = post.Status.ToString()
            }).ToList();

            return new PaginatedList<PostListDto>(postListDtos, totalCount, queryParameters.Page, queryParameters.PageSize);
        }


        public async Task<PostDto> GetPostByIdAsync(Guid id)
        {
            var spec = new PostByIdSpecification(id);
            var posts = await _postRepository.FindWithSpecificationAsync(spec);
            if (posts == null)
                throw new KeyNotFoundException($"Post with ID {id} not found.");

            var post = posts.FirstOrDefault();

            return new PostDto
            {
                Id = post.Id,
                Title = post.Title,
                Content = post.Content,
                MetaTitle = post.MetaTitle,
                MetaDescription = post.MetaDescription,
                Slug = post.Slug,
                PublishedDate = post.PublishedDate,
                ThumbnailUrl = post.ThumbnailUrl,
                Summary = post.Summary,
                ViewCount = post.ViewCount,
                AuthorName = post.Author?.Name ?? "Unknown",
                Categories = post.Categories.Select(c => new KeyValueDto<Guid>
                {
                    Id = c.Id,
                    Name = c.Name
                }).ToList(),
                Tags = post.Tags.Select(t => new KeyValueDto<Guid>
                {

                    Id = t.Id,
                    Name = t.Name,
                }).ToList()
            };
        }


        public async Task UpdatePostAsync(Guid id, UpdatePostRequest request)
        {

            var spec = new PostByIdSpecification(id);
            var posts = await _postRepository.FindWithSpecificationAsync(spec);
            if (posts == null)
                throw new KeyNotFoundException($"Post with ID {id} not found.");

            var post = posts.FirstOrDefault();


            post.SetTitle(request.Title);
            post.SetContent(request.Content);
            post.SetMetaTitle(request.MetaTitle);
            post.SetMetaDescription(request.MetaDescription);
            post.SetSlug(request.Slug);
            post.ChangeStatus(request.Status);
            post.SetSummary(request.Summary);
            post.SetThumbnailUrl(request.ThumbnailUrl);


            var newCategories = await GetCategoriesByIdsAsync(request.CategoryIds);

            post.UpdateCategories(newCategories);


            var newTags = await GetTagsByIdsAsync(request.TagIds);

            post.UpdateTags(newTags);

            _postRepository.Update(post);
            await _unitOfWork.CommitAsync();
        }


        private async Task<IEnumerable<Category>> GetCategoriesByIdsAsync(IEnumerable<Guid> categoryIds)
        {
            var categories = new List<Category>();
            foreach (var catId in categoryIds)
            {
                var category = await _categoryRepository.GetByIdAsync(catId);
                if (category == null)
                    throw new KeyNotFoundException($"Category with ID {catId} not found.");
                categories.Add(category);
            }
            return categories;
        }


        private async Task<IEnumerable<Tag>> GetTagsByIdsAsync(IEnumerable<Guid> tagIds)
        {
            var tags = new List<Tag>();
            foreach (var tagId in tagIds)
            {
                var tag = await _tagRepository.GetByIdAsync(tagId);
                if (tag == null)
                    throw new KeyNotFoundException($"Tag with ID {tagId} not found.");
                tags.Add(tag);
            }
            return tags;
        }


        public async Task DeletePostAsync(Guid id)
        {
            var post = await _postRepository.GetByIdAsync(id);
            if (post == null)
                throw new KeyNotFoundException($"Post with ID {id} not found.");

            _postRepository.Remove(post);
            await _unitOfWork.CommitAsync();
        }


    }
}
