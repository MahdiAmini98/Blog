using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Blog.Domain.Entities
{

    public class User : IdentityUser<Guid> // ارث‌بری از IdentityUser با Guid به‌عنوان شناسه
    {
        public string? ProfilePictureUrl { get; private set; } // تصویر پروفایل (اختیاری)
        public string? Bio { get; private set; } // توضیحات مختصر کاربر (اختیاری)

        // ناوبری‌ها
        private readonly List<Post> _posts = new();
        public IReadOnlyCollection<Post> Posts => _posts.AsReadOnly();

        private readonly List<Comment> _comments = new();
        public IReadOnlyCollection<Comment> Comments => _comments.AsReadOnly();
        public ICollection<Reaction> Reactions { get; set; } = new List<Reaction>();

        // متد برای تنظیم تصویر پروفایل
        public void SetProfilePictureUrl(string? profilePictureUrl)
        {
            if (!string.IsNullOrWhiteSpace(profilePictureUrl) && !Uri.IsWellFormedUriString(profilePictureUrl, UriKind.Absolute))
            {
                throw new ArgumentException("Invalid profile picture URL.", nameof(profilePictureUrl));
            }

            ProfilePictureUrl = profilePictureUrl;
        }

        // متد برای تنظیم توضیحات کاربر
        public void SetBio(string? bio)
        {
            if (bio != null && bio.Length > 500)
            {
                throw new ArgumentException("Bio cannot exceed 500 characters.", nameof(bio));
            }

            Bio = bio;
        }

        // متد برای افزودن پست
        public void AddPost(Post post)
        {
            if (post == null)
            {
                throw new ArgumentNullException(nameof(post), "Post cannot be null.");
            }

            _posts.Add(post);
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
