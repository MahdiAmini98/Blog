using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Domain.Enumerations
{
    public enum PostStatus
    {
        [Display(Name = "پیش‌نویس")]
        Draft = 0,

        [Display(Name = "منتشر شده")]
        Published = 1,

        [Display(Name = "آرشیو شده")]
        Archived = 2
    }
    public enum ReactionType
    {
        [Display(Name = "لایک")]
        Like = 0,

        [Display(Name = "دیسلایک")]
        Dislike = 1,

        [Display(Name = "علاقه‌مندی")]
        Love = 2,

        [Display(Name = "عصبانیت")]
        Angry = 3
    }
    public enum UserRole
    {
        [Display(Name = "مدیر")]
        Admin = 0,

        [Display(Name = "نویسنده")]
        Author = 1,

        [Display(Name = "کاربر عادی")]
        User = 2
    }
}
