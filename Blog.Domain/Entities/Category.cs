using Blog.Domain.Base;
using Blog.Domain.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Domain.Entities
{
    public class Category : EntityBase
    {
        public string Name { get; private set; } // نام دسته‌بندی
        public string Slug { get; private set; } // URL یکتا
        public string? Description { get; private set; } // توضیحات (اختیاری)

        private readonly List<Post> _posts = new(); // لیست خصوصی برای مدیریت داخلی پست‌ها
        public IReadOnlyCollection<Post> Posts => _posts.AsReadOnly(); // دسترسی فقط خواندنی به پست‌ها

        // سازنده برای مقداردهی اولیه
        public Category(string name, string? description = null)
        {

            SetName(name);
            Description = description;
            Slug = GenerateSlug(name); // تولید Slug در هنگام ساخت
        }

        // متد برای تنظیم نام و تولید Slug
        public void SetName(string name)
        {

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException("Category name cannot be empty.", nameof(name));
            }

            if (name.Length > ValidationConstants.CategoryNameMaxLength)
            {
                throw new ArgumentException($"Category name cannot exceed {ValidationConstants.CategoryNameMaxLength} characters.", nameof(name));
            }

            Name = name;
            Slug = GenerateSlug(name);
        }

        public void SetDescription(string? description)
        {
            if (string.IsNullOrWhiteSpace(description))
            {
                throw new ArgumentException("Category description cannot be empty.", nameof(description));
            }

            Description = description; // اعمال تغییرات
        }

        // متد برای افزودن پست به دسته‌بندی
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


        // متد برای تولید Slug
        private string GenerateSlug(string input)
        {
            return input.ToLower().Replace(" ", "-").Replace("--", "-");
        }
    }

}
