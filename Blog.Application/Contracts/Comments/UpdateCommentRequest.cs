using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Application.Contracts.Comments
{
    public class UpdateCommentRequest
    {
        public string Content { get; set; } = string.Empty;
    }
}
