using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Application.Contracts.Medias
{
    public class UploadMediaRequest
    {
        public string Url { get; set; }
        public string Type { get; set; }
        public Guid UploadedBy { get; set; }
    }
}
