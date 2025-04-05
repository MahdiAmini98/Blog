using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Domain.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        // گرفتن Repository مربوط به انتیتی
        IRepository<T> Repository<T>() where T : class;

        // مدیریت تراکنش‌ها
        void Commit(); // ذخیره تغییرات
        Task CommitAsync(); // ذخیره تغییرات به صورت غیرهمگام

        void Rollback(); // بازگشت تغییرات
        Task RollbackAsync(); // بازگشت تغییرات به صورت غیرهمگام

        // مدیریت تراکنش‌ها (اختیاری)
        void BeginTransaction(); // شروع تراکنش
        Task BeginTransactionAsync(); // شروع تراکنش به صورت غیرهمگام

        void CommitTransaction(); // ذخیره تراکنش
        Task CommitTransactionAsync(); // ذخیره تراکنش به صورت غیرهمگام

        void RollbackTransaction(); // بازگشت تراکنش
        Task RollbackTransactionAsync(); // بازگشت تراکنش به صورت غیرهمگام
    }
}
