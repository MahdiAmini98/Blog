using Blog.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Domain.Specifications
{
    public class PostAdvancedFilterSpecification : Specification<Post>
    {
        private readonly string? _categorySlug;
        private readonly string? _tagSlug;
        private readonly string? _searchText;
        private readonly DateTime? _fromDate;
        private readonly DateTime? _toDate;

        public PostAdvancedFilterSpecification(
            string? categorySlug = null,
            string? tagSlug = null,
            string? searchText = null,
            DateTime? fromDate = null,
            DateTime? toDate = null)
        {
            _categorySlug = categorySlug;
            _tagSlug = tagSlug;
            _searchText = searchText;
            _fromDate = fromDate;
            _toDate = toDate;

            //   Include  مورد نیاز
            AddInclude(p => p.Author);
            AddInclude(p => p.Categories);
            AddInclude(p => p.Tags);
        }

        public override Expression<Func<Post, bool>> ToExpression()
        {
            return post =>
                (_categorySlug == null || post.Categories.Any(c => c.Slug == _categorySlug)) &&
                (_tagSlug == null || post.Tags.Any(t => t.Slug == _tagSlug)) &&
                (_searchText == null || post.Title.Contains(_searchText) || post.Content.Contains(_searchText)) &&
                (_fromDate == null || post.PublishedDate >= _fromDate) &&
                (_toDate == null || post.PublishedDate <= _toDate);
        }
    }
}
