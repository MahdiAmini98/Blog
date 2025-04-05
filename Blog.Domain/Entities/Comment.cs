using Blog.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Domain.Entities
{
    public class Comment : EntityBase
    {
        public string Content { get; private set; } // متن کامنت
        public Guid PostId { get; private set; } // شناسه پست
        public Guid AuthorId { get; private set; } // شناسه نویسنده

        // ناوبری‌ها
        public Post Post { get; private set; } // ارتباط با پست
        public User Author { get; private set; } // ارتباط با نویسنده

        // سازنده برای مقداردهی اولیه
        public Comment(string content, Guid postId, Guid authorId)
        {
            SetContent(content); // تنظیم و اعتبارسنجی محتوای کامنت
            PostId = postId;
            AuthorId = authorId;
            CreatedDate = DateTime.UtcNow; // تاریخ ایجاد به‌طور خودکار تنظیم می‌شود
        }

        // متد برای به‌روزرسانی محتوا
        public void SetContent(string content)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                throw new ArgumentException("Comment content cannot be empty.", nameof(content));
            }

            if (content.Length > 500) // محدودیت طول محتوا
            {
                throw new ArgumentException("Comment content cannot exceed 500 characters.", nameof(content));
            }

            Content = content;
        }

        // متد برای ارتباط با یک پست
        public void AssignToPost(Post post)
        {
            if (post == null)
            {
                throw new ArgumentNullException(nameof(post), "Post cannot be null.");
            }

            Post = post;
            PostId = post.Id;
        }

        // متد برای ارتباط با یک نویسنده
        public void AssignToAuthor(User author)
        {
            if (author == null)
            {
                throw new ArgumentNullException(nameof(author), "Author cannot be null.");
            }

            Author = author;
            AuthorId = author.Id;
        }
    }
}
