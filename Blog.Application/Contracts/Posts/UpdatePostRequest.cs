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
    }
}
