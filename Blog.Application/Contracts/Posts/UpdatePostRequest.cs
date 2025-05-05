using Blog.Domain.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Application.Contracts.Posts
{
    public class UpdatePostRequest
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string MetaTitle { get; set; }
        public string MetaDescription { get; set; }
        public string Slug { get; set; } = string.Empty;
        public string? ThumbnailUrl { get; set; }
        public string? Summary { get; set; }
        public PostStatus Status { get; set; } = PostStatus.Draft;
        public List<Guid> CategoryIds { get; set; } = new();
        public List<Guid> TagIds { get; set; } = new();
    }
}
