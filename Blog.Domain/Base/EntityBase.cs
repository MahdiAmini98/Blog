using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Domain.Base
{
    public abstract class EntityBase
    {
        // شناسه یکتا برای همه انتیتی‌ها
        public Guid Id { get; set; } = Guid.NewGuid();

        // تاریخ ایجاد
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        // تاریخ آخرین به‌روزرسانی
        public DateTime? LastModifiedDate { get; set; }

        public byte[] RowVersion { get; set; }


        // متد برای به‌روزرسانی تاریخ آخرین تغییر
        public void UpdateLastModifiedDate()
        {
            LastModifiedDate = DateTime.UtcNow;
        }

        // متد برای مقایسه دو انتیتی بر اساس Id
        public override bool Equals(object? obj)
        {
            if (obj is not EntityBase otherEntity) return false;
            return Id == otherEntity.Id;
        }

        // هش کردن Id برای استفاده در مقایسه
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
