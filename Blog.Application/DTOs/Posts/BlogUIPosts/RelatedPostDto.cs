
namespace Blog.Application.DTOs.Posts.BlogUIPosts
{
    public class RelatedPostDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Image { get; set; }
        public DateTime PublishedDate { get; set; }
        public string Category { get; set; }
        public string ShortDescription { get; set; }
    }
}
