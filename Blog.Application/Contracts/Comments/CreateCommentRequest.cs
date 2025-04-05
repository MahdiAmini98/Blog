using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Application.Contracts.Comments
{
    public class CreateCommentRequest
    {
        public string Content { get; set; } = string.Empty;
        public Guid PostId { get; set; }
        public Guid AuthorId { get; set; }
    }
}
