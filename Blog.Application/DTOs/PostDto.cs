using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Application.DTOs
{
    public class PostDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string MetaTitle { get; set; }
        public string MetaDescription { get; set; }
        public string Slug { get; set; } = string.Empty;
        public DateTime PublishedDate { get; set; }
        public string AuthorName { get; set; }
        public string? ThumbnailUrl { get; set; }
        public string? Summary { get; set; }
        public List<KeyValueDto<Guid>> Categories { get; set; } = new();
        public List<KeyValueDto<Guid>> Tags { get; set; } = new();
        public int ViewCount { get; set; }
    }
}
