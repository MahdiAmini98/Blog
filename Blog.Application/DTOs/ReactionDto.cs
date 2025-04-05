using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Application.DTOs
{
    public class ReactionDto
    {
        public Guid Id { get; set; }
        public Guid PostId { get; set; }
        public Guid UserId { get; set; }
        public string ReactionType { get; set; } // Like, Dislike, etc.
        public DateTime CreatedDate { get; set; }
    }
}
