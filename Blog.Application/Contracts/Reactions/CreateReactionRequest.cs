using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Application.Contracts.Reactions
{
    public class CreateReactionRequest
    {
        public Guid PostId { get; set; }
        public Guid UserId { get; set; }
        public string ReactionType { get; set; }
    }
}
