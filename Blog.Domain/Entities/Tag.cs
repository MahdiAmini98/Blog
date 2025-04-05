using Blog.Domain.Base;
using Blog.Domain.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Domain.Entities
{
    public class Tag : EntityBase
    {
        public string Name { get; private set; } // نام برچسب
        public string Slug { get; private set; } // URL یکتا

        private readonly List<Post> _posts = new(); // لیست خصوصی پست‌ها
        public IReadOnlyCollection<Post> Posts => _posts.AsReadOnly(); // دسترسی فقط خواندنی به پست‌ها

        // سازنده برای مقداردهی اولیه
        public Tag(string name)
        {
            SetName(name); // تنظیم نام و تولید Slug در هنگام ساخت
        }

        // متد برای تنظیم نام و تولید Slug
        public void SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Tag name cannot be empty.", nameof(name));
            }
            if (name.Length > ValidationConstants.TagNameMaxLength)
            {
                throw new ArgumentException($"Tag name cannot exceed {ValidationConstants.TagNameMaxLength} characters.", nameof(name));
            }
            Name = name;
            Slug = GenerateSlug(name); // تولید Slug
        }

        // متد برای افزودن یک پست به برچسب
        public void AddPost(Post post)
        {
            if (post == null)
            {
                throw new ArgumentNullException(nameof(post), "Post cannot be null.");
            }

            if (!_posts.Contains(post))
            {
                _posts.Add(post);
            }
        }

        // متد برای حذف یک پست از برچسب
        public void RemovePost(Post post)
        {
            if (post == null)
            {
                throw new ArgumentNullException(nameof(post), "Post cannot be null.");
            }

            _posts.Remove(post);
        }

        // متد خصوصی برای تولید Slug
        private string GenerateSlug(string input)
        {
            return input.ToLower().Replace(" ", "-").Replace("--", "-");
        }
    }

}
