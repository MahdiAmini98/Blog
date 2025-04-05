using Blog.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Domain.Entities
{
    public class Media : EntityBase
    {
        public string Url { get; private set; } // آدرس فایل
        public string Type { get; private set; } // نوع فایل (Image, Video, etc.)
        public Guid UploadedById { get; private set; } // شناسه آپلودکننده

        // ناوبری‌ها
        public User UploadedBy { get; private set; } // ارتباط با کاربر آپلودکننده

        // لیست انواع فایل مجاز
        private static readonly string[] AllowedFileTypes = { "image", "video", "document" };




        // Parameterless constructor for EF Core
        private Media() { }


        // سازنده برای مقداردهی اولیه
        public Media(string url, string type, User uploadedBy)
        {
            SetUrl(url);
            SetType(type);
            AssignUploader(uploadedBy);
            CreatedDate = DateTime.UtcNow; // تنظیم تاریخ آپلود
        }

        // متد برای تنظیم آدرس فایل
        public void SetUrl(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentException("File URL cannot be empty.", nameof(url));
            }

            // اعتبارسنجی فرمت URL
            if (!Uri.TryCreate(url, UriKind.Absolute, out _))
            {
                throw new ArgumentException("Invalid file URL format.", nameof(url));
            }

            Url = url;
        }

        // متد برای تنظیم نوع فایل
        public void SetType(string type)
        {
            if (string.IsNullOrWhiteSpace(type))
            {
                throw new ArgumentException("File type cannot be empty.", nameof(type));
            }

            if (Array.IndexOf(AllowedFileTypes, type.ToLower()) == -1)
            {
                throw new ArgumentException($"File type '{type}' is not allowed.", nameof(type));
            }

            Type = type.ToLower();
        }

        // متد برای تنظیم آپلودکننده
        public void AssignUploader(User uploadedBy)
        {
            if (uploadedBy == null)
            {
                throw new ArgumentNullException(nameof(uploadedBy), "Uploader cannot be null.");
            }

            UploadedBy = uploadedBy;
            UploadedById = uploadedBy.Id;
        }
    }


}
