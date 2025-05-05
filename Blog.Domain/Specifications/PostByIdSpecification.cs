using Blog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Domain.Specifications
{
    public class PostByIdSpecification : Specification<Post>
    {

        private Guid _postId;
        public PostByIdSpecification(Guid postId)
        {

            _postId = postId;

            AddInclude(p => p.Author);
            AddInclude(p => p.Tags);
            AddInclude(p => p.Categories);
        }

        public override Expression<Func<Post, bool>> ToExpression()
        {
            return post => post.Id == _postId;
        }
    }
}
