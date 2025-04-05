using Blog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Domain.Specifications
{
    public class PostFilterSpecification : Specification<Post>
    {
        private readonly string? _tag;
        private readonly string? _searchText;
        private readonly DateTime? _fromDate;
        private readonly DateTime? _toDate;

        public PostFilterSpecification(string? tag = null, string? searchText = null, DateTime? fromDate = null, DateTime? toDate = null)
        {
            _tag = tag;
            _searchText = searchText;
            _fromDate = fromDate;
            _toDate = toDate;
        }


        public override Expression<Func<Post, bool>> ToExpression()
        {
            return post =>
    (_tag == null || post.Tags.Any(tag => tag.Name == _tag)) &&
    (_searchText == null || post.Title.Contains(_searchText) || post.Content.Contains(_searchText)) &&
    (_fromDate == null || post.PublishedDate >= _fromDate) &&
    (_toDate == null || post.PublishedDate <= _toDate);
        }
    }
}

