using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Application.DTOs.Posts.WebUIPosts
{
    public class RelatedPostDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Image { get; set; }
        public DateTime PublishedDate { get; set; }
        public string Category { get; set; }
        public string ShortDescription { get; set; }
    }
}
