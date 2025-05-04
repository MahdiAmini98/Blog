namespace Blog.PanelAdmin.Models.Posts
{
    public class PostListResponseDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTime PublishedDate { get; set; }
        public string? Summary { get; set; }
        public string? AuthorName { get; set; }
        public string? ThumbnailUrl { get; set; }
        public int ViewCount { get; set; }
        public string Status { get; set; } = "Draft";
    }
}
