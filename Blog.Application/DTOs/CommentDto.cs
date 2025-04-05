using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Application.DTOs
{
    public class CommentDto
    {
        public Guid Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public Guid PostId { get; set; }
        public Guid AuthorId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }
}
