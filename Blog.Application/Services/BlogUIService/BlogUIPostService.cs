using Blog.Application.DTOs;
using Blog.Application.DTOs.Posts.BlogUIPosts;
using Blog.Application.Interfaces.BlogUIService;
using Blog.Domain.Entities;
using Blog.Domain.Interfaces;
using Blog.Domain.Specifications;

namespace Blog.Application.Services.BlogUIService
{
    public class BlogUIPostService : IBlogUIPostService
    {
        private readonly IRepository<Post> _postRepository;
        private readonly IUnitOfWork _unitOfWork;

        public BlogUIPostService(IRepository<Post> postRepository, IUnitOfWork unitOfWork)
        {
            _postRepository = postRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<PaginatedList<BlogUIPostListDto>> GetPostsAsync(int pageIndex, int pageSize, string? searchKey = null)
        {
            var query = await _postRepository.GetAllAsync();

            if (!string.IsNullOrWhiteSpace(searchKey))
            {
                query = query.Where(p => p.Title.Contains(searchKey) || p.Content.Contains(searchKey));
            }

            int totalCount = query.Count();

            var posts = query.OrderByDescending(p => p.PublishedDate)
                                   .Skip((pageIndex - 1) * pageSize)
                                   .Take(pageSize)
                                   .ToList();

            var dtos = posts.Select(p => new BlogUIPostListDto
            {
                Id = p.Id,
                Title = p.Title,
                Description = !string.IsNullOrWhiteSpace(p.Summary)
                              ? p.Summary.Length > 50 ? p.Summary.Substring(0, 50) + "..." : p.Summary
                              : (p.Content.Length > 50 ? p.Content.Substring(0, 50) + "..." : p.Content),
                Author = p.Author != null ? p.Author.Name : "Unknown",
                ImageUrl = p.ThumbnailUrl ?? string.Empty,
                Slug = p.Slug ?? string.Empty,
                Reactions = p.Reactions.Select(r => r.Type).ToList(),
                PublishedDate = p.PublishedDate,

            }).ToList();

            return new PaginatedList<BlogUIPostListDto>(dtos, totalCount, pageIndex, pageSize);
        }

        public async Task<PostDetailDto> GetPostDetailBySlugAsync(string slug)
        {
            var spec = new PostBySlugSpecification(slug);
            var posts = await _postRepository.FindWithSpecificationAsync(spec);
            var post = posts.FirstOrDefault();

            if (post == null)
            {
                return null;
            }


            var dto = new PostDetailDto
            {
                Id = post.Id,
                Image = post.ThumbnailUrl ?? string.Empty,
                Title = post.Title,
                Slug = post.Slug,
                Content = post.Content,
                PublishedDate = post.PublishedDate,
                AuthorName = post.Author?.Name ?? "ناشناس",
                AuthorProfilePicture = post.Author?.ProfilePictureUrl ?? "images/avatar_placeholder.webp",
                AuthorBio = string.IsNullOrWhiteSpace(post.Author?.Bio)
                             ? "مهدی امینی، توسعه دهنده ارشد دات نت با بیش از ۵ سال تجربه؛ متخصص در بلیزور و تکنولوژی‌های مدرن وب هستم."
                             : post.Author.Bio,

                Categories = post.Categories
                    .Select(c => new CategoryDto
                    {
                        Id = c.Id,
                        Name = c.Name
                    }).ToList(),
                Tags = post.Tags
                    .Select(t => new TagDto
                    {
                        Id = t.Id,
                        Name = t.Name
                    }).ToList(),
                Comments = post.Comments
                    .Select(cm => new CommentDto
                    {
                        Id = cm.Id,
                        Content = cm.Content,
                        AuthorName = cm.Author?.Name ?? "ناشناس",
                        AuthorAvatar = cm.Author?.ProfilePictureUrl ?? "images/avatar_placeholder.webp",
                        CreatedDate = cm.CreatedDate,

                    }).ToList()
            };


            dto.RelatedPosts = await GetRelatedPostsAsync(post);

            return dto;
        }

        private async Task<List<RelatedPostDto>> GetRelatedPostsAsync(Post post)
        {

            var firstTag = post.Tags.FirstOrDefault();
            if (firstTag == null)
                return new List<RelatedPostDto>();

            var related = await _postRepository.FindAsync(
                p => p.Tags.Any(t => t.Id == firstTag.Id) && p.Id != post.Id
            );

            return related
               .OrderByDescending(r => r.PublishedDate)
               .Take(3)
               .Select(r => new RelatedPostDto
               {
                   Id = r.Id,
                   Title = r.Title,
                   Slug = r.Slug,
                   Image = r.ThumbnailUrl ?? string.Empty,
                   PublishedDate = r.PublishedDate,
                   Category = r.Categories.FirstOrDefault()?.Name ?? string.Empty,
                   ShortDescription = !string.IsNullOrWhiteSpace(r.Summary)
                                        ? r.Summary
                                        : (r.Content.Length > 150 ? r.Content.Substring(0, 150) + "..." : r.Content)
               })
               .ToList();
        }
    }
}
