using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Application.Contracts.Posts
{
    public class PostQueryParameters
    {
        public string? Tag { get; set; }
        public string? SearchText { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
