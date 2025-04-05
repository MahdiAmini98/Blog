using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Domain.Aggregates
{
    //public class BlogAggregate
    //{
    //    // اجزای اصلی Aggregate
    //    private readonly List<Post> _posts = new();
    //    private readonly List<Category> _categories = new();
    //    private readonly List<Tag> _tags = new();

    //    // متدها برای مدیریت اجزا
    //    public Post CreatePost(string title, string content, Guid authorId, IEnumerable<string> tags)
    //    {
    //        var post = Post.Create(title, GenerateSlug(title), content, authorId);

    //        // اختصاص tags‌ به پست
    //        foreach (var tagName in tags)
    //        {
    //            var tag = FindOrCreateTag(tagName);
    //            post.Tags.Add(tag);
    //        }

    //        _posts.Add(post);
    //        return post;
    //    }

    //    public void AddCategory(string categoryName)
    //    {
    //        if (_categories.Exists(c => c.Name == categoryName))
    //            throw new InvalidOperationException("Category already exists.");

    //        _categories.Add(new Category { Name = categoryName, Slug = GenerateSlug(categoryName) });
    //    }

    //    // جستجو یا ایجاد tag
    //    private Tag FindOrCreateTag(string tagName)
    //    {
    //        var existingTag = _tags.Find(t => t.Name == tagName);
    //        if (existingTag != null) return existingTag;

    //        var newTag = new Tag { Name = tagName, Slug = GenerateSlug(tagName) };
    //        _tags.Add(newTag);
    //        return newTag;
    //    }

    //    private string GenerateSlug(string input)
    //    {
    //        // تبدیل متن به URL یکتا
    //        return input.ToLower().Replace(" ", "-");
    //    }
    //}
}
