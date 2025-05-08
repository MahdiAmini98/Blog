using Blog.Domain.Enumerations;


namespace Blog.Application.DTOs.Posts.BlogUIPosts
{
    public class BlogUIPostListDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public DateTime PublishedDate { get; set; }
        public List<ReactionType> Reactions { get; set; }
    }
}
