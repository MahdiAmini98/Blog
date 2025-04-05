using Blog.Domain.Base;
using Blog.Domain.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Domain.Entities
{
    public class Post : EntityBase
    {
        public string Title { get; private set; } // عنوان پست
        public string Slug { get; private set; } // URL یکتا
        public string Content { get; private set; } // متن پست
        public string? Summary { get; private set; } // خلاصه محتوا (اختیاری)
        public string? ThumbnailUrl { get; private set; } // تصویر شاخص (اختیاری)
        public DateTime PublishedDate { get; private set; } // تاریخ انتشار
        public int ViewCount { get; private set; } // تعداد بازدیدها
        public Guid AuthorId { get; private set; } // شناسه نویسنده
        public PostStatus Status { get; private set; } // وضعیت پست

        // ناوبری‌ها
        private readonly List<Category> _categories = new();
        public IReadOnlyCollection<Category> Categories => _categories.AsReadOnly();

        private readonly List<Tag> _tags = new();
        public IReadOnlyCollection<Tag> Tags => _tags.AsReadOnly();

        private readonly List<Comment> _comments = new();
        public IReadOnlyCollection<Comment> Comments => _comments.AsReadOnly();

        private readonly List<Reaction> _reactions = new();
        public IReadOnlyCollection<Reaction> Reactions => _reactions.AsReadOnly();

        public User Author { get; private set; }


        private Post(string title, string slug, string content, Guid authorId)
        {
            SetTitle(title);
            GenerateSlug(slug);
            SetContent(content);
            AuthorId = authorId;
            Status = PostStatus.Draft;
            PublishedDate = DateTime.UtcNow;
        }

        // متد برای ایجاد پست جدید
        public static Post Create(string title, string content, Guid authorId)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException("Title cannot be empty.", nameof(title));
            }

            if (string.IsNullOrWhiteSpace(content))
            {
                throw new ArgumentException("Content cannot be empty.", nameof(content));
            }

            var slug = GenerateSlug(title);
            return new Post(title, slug, content, authorId);
        }

        // متد برای تنظیم عنوان
        public void SetTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException("Title cannot be empty.", nameof(title));
            }

            Title = title;
            Slug = GenerateSlug(title); // به‌روزرسانی Slug هنگام تغییر عنوان
        }

        // متد برای تنظیم Slug
        private static string GenerateSlug(string input)
        {
            return input.ToLower().Replace(" ", "-").Replace("--", "-");
        }

        // متد برای تنظیم محتوا
        public void SetContent(string content)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                throw new ArgumentException("Content cannot be empty.", nameof(content));
            }

            Content = content;
        }

        // متد برای تنظیم خلاصه محتوا
        public void SetSummary(string summary)
        {
            Summary = summary?.Length > 500
                ? throw new ArgumentException("Summary cannot exceed 500 characters.", nameof(summary))
                : summary;
        }

        // متد برای تنظیم تصویر شاخص
        public void SetThumbnailUrl(string thumbnailUrl)
        {
            if (!string.IsNullOrWhiteSpace(thumbnailUrl) && !Uri.IsWellFormedUriString(thumbnailUrl, UriKind.Absolute))
            {
                throw new ArgumentException("Invalid thumbnail URL.", nameof(thumbnailUrl));
            }

            ThumbnailUrl = thumbnailUrl;
        }

        // متد برای تغییر وضعیت پست
        public void ChangeStatus(PostStatus newStatus)
        {
            if (Status == PostStatus.Archived)
            {
                throw new InvalidOperationException("Cannot change status of an archived post.");
            }

            Status = newStatus;
        }

        // متد برای افزایش تعداد بازدیدها
        public void IncrementViewCount()
        {
            ViewCount++;
        }

        // متد برای مدیریت دسته‌بندی‌ها
        public void AddCategory(Category category)
        {
            if (category == null)
            {
                throw new ArgumentNullException(nameof(category), "Category cannot be null.");
            }

            if (_categories.Contains(category))
            {
                throw new InvalidOperationException("Category is already assigned to this post.");
            }

            _categories.Add(category);
        }

        public void RemoveCategory(Category category)
        {
            if (category == null || !_categories.Contains(category))
            {
                throw new InvalidOperationException("Category is not assigned to this post.");
            }

            _categories.Remove(category);
        }

        // متد برای مدیریت برچسب‌ها
        public void AddTag(Tag tag)
        {
            if (tag == null)
            {
                throw new ArgumentNullException(nameof(tag), "Tag cannot be null.");
            }

            if (_tags.Contains(tag))
            {
                throw new InvalidOperationException("Tag is already assigned to this post.");
            }

            _tags.Add(tag);
        }

        public void RemoveTag(Tag tag)
        {
            if (tag == null || !_tags.Contains(tag))
            {
                throw new InvalidOperationException("Tag is not assigned to this post.");
            }

            _tags.Remove(tag);
        }

        // متد برای افزودن نظر
        public void AddComment(Comment comment)
        {
            if (comment == null)
            {
                throw new ArgumentNullException(nameof(comment), "Comment cannot be null.");
            }

            _comments.Add(comment);
        }
    }


}
