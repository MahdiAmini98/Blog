using Blog.Domain.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Domain.Interfaces
{
    public interface IRepository<T> where T : class
    {
        // عملیات همگام
        T GetById(Guid id); // گرفتن یک انتیتی بر اساس ID
        IEnumerable<T> GetAll(); // گرفتن تمام انتیتی‌ها
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate); // جستجو با شرط
        int Count(Expression<Func<T, bool>> predicate); // شمارش بر اساس شرط

        void Add(T entity); // اضافه کردن یک انتیتی
        void Remove(T entity); // حذف یک انتیتی
        void Update(T entity); // به‌روزرسانی یک انتیتی

        // عملیات غیرهمگام
        Task<T> GetByIdAsync(Guid id); // گرفتن یک انتیتی بر اساس ID به صورت غیرهمگام
        Task<IEnumerable<T>> GetAllAsync(); // گرفتن تمام انتیتی‌ها به صورت غیرهمگام
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, bool asNoTracking = false); // جستجو با شرط به صورت غیرهمگام
        Task<int> CountAsync(Expression<Func<T, bool>> predicate); // شمارش بر اساس شرط به صورت غیرهمگام

        // پشتیبانی از IQueryable
        IQueryable<T> Query(); // دسترسی به کوئری مستقیم

        // متدهای Specification Pattern
        IEnumerable<T> FindWithSpecification(Specification<T> specification); // استفاده از Specification برای جستجو
        Task<IEnumerable<T>> FindWithSpecificationAsync(Specification<T> specification); // استفاده از Specification به صورت غیرهمگام
        Task<int> CountWithSpecificationAsync(Specification<T> specification);
        Task<IEnumerable<T>> FindWithSpecificationPagedAsync(Specification<T> specification, int page, int pageSize);


        // صفحه‌بندی عمومی
        Task<IEnumerable<T>> GetPagedAsync(int page, int pageSize);
    }
}
