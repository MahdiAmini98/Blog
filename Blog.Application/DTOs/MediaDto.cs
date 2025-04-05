using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Application.DTOs
{
    public class MediaDto
    {
        public Guid Id { get; set; }
        public string Url { get; set; }
        public string Type { get; set; }
        public Guid UploadedById { get; set; }
        public DateTime UploadDate { get; set; }
    }
}
