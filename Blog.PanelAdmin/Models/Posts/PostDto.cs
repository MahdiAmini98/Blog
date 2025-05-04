namespace Blog.PanelAdmin.Models.Posts
{
    public class PostDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string? Summary { get; set; }
        public string? ThumbnailUrl { get; set; }
        public DateTime PublishedDate { get; set; }
        public int ViewCount { get; set; }
        public PostStatus Status { get; set; } = PostStatus.Draft;
        public string MetaTitle { get; set; } = string.Empty;
        public string MetaDescription { get; set; } = string.Empty;
        public List<KeyValue> Categories { get; set; } = new();
        public List<KeyValue> Tags { get; set; } = new();
    }

    public class KeyValue
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
