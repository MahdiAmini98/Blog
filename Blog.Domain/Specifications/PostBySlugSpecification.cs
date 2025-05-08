using Blog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Domain.Specifications
{
    public class PostBySlugSpecification : Specification<Post>
    {
        private readonly string _slug;

        public PostBySlugSpecification(string slug)
        {
            _slug = slug?.ToLower().Trim();
            AddInclude(p => p.Author);
            AddInclude(p => p.Tags);
            AddInclude(p => p.Categories);
            AddInclude(p => p.Comments);
            AddInclude(p => p.Reactions);
        }

        public override Expression<Func<Post, bool>> ToExpression()
        {
            return post => post.Slug == _slug;
        }
    }
}
