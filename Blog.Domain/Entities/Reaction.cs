using Blog.Domain.Base;
using Blog.Domain.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Domain.Entities
{
    public class Reaction : EntityBase
    {
        public Guid PostId { get; private set; } // شناسه پست
        public Guid UserId { get; private set; } // شناسه کاربر
        public ReactionType Type { get; private set; } // نوع واکنش
        public DateTime CreatedDate { get; private set; } // تاریخ ایجاد

        // ناوبری‌ها
        public Post Post { get; private set; } // ارتباط با پست
        public User User { get; private set; } // ارتباط با کاربر

        // سازنده برای مقداردهی اولیه
        public Reaction(Guid postId, Guid userId, ReactionType type)
        {
            AssignToPost(postId);
            AssignToUser(userId);
            SetType(type);
            CreatedDate = DateTime.UtcNow; // تنظیم تاریخ ایجاد
        }

        // متد برای تنظیم نوع واکنش
        public void SetType(ReactionType type)
        {
            if (!Enum.IsDefined(typeof(ReactionType), type))
            {
                throw new ArgumentException("Invalid reaction type.", nameof(type));
            }

            Type = type;
        }

        // متد برای ارتباط با یک پست
        public void AssignToPost(Guid postId)
        {
            if (postId == Guid.Empty)
            {
                throw new ArgumentException("Post ID cannot be empty.", nameof(postId));
            }

            PostId = postId;
        }

        // متد برای ارتباط با یک کاربر
        public void AssignToUser(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                throw new ArgumentException("User ID cannot be empty.", nameof(userId));
            }

            UserId = userId;
        }
    }


}
