namespace Blog.Application.DTOs.Posts.BlogUIPosts
{
    public class PostDetailDto
    {
        public Guid Id { get; set; }
        public string Image { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Content { get; set; }
        public DateTime PublishedDate { get; set; }
        public string AuthorName { get; set; }
        public string AuthorProfilePicture { get; set; }
        public string AuthorBio { get; set; }


        public List<CategoryDto> Categories { get; set; } = new();
        public List<TagDto> Tags { get; set; } = new();
        public List<CommentDto> Comments { get; set; } = new();
        public List<RelatedPostDto> RelatedPosts { get; set; } = new();
    }
}
