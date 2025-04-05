using Blog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Domain.Specifications
{
    public class PostWithTagsSpecification : Specification<Post>
    {
        private readonly string[] _requiredTags;

        public PostWithTagsSpecification(string[] requiredTags)
        {
            _requiredTags = requiredTags ?? throw new ArgumentNullException(nameof(requiredTags));
        }

        public override Expression<Func<Post, bool>> ToExpression()
        {
            return post => post.Tags.Any(tag => _requiredTags.Contains(tag.Name));
        }
    }
}
