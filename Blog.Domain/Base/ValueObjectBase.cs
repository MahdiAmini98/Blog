using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Domain.Base
{
    public abstract class ValueObjectBase
    {
        /// <summary>
        /// برای مقایسه Value Object ها
        /// </summary>
        /// <param name="obj">شیء برای مقایسه</param>
        /// <returns>True اگر برابر باشند</returns>
        public override bool Equals(object? obj)
        {
            if (obj == null || obj.GetType() != GetType())
                return false;

            var other = (ValueObjectBase)obj;

            return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
        }

        /// <summary>
        /// تولید Hash Code بر اساس مقادیر اعضا
        /// </summary>
        /// <returns>Hash Code تولید شده</returns>
        public override int GetHashCode()
        {
            return GetEqualityComponents()
                .Aggregate(1, (current, obj) =>
                {
                    unchecked
                    {
                        return current * 23 + (obj?.GetHashCode() ?? 0);
                    }
                });
        }

        /// <summary>
        /// متد انتزاعی برای تعریف مقادیر قابل مقایسه
        /// </summary>
        /// <returns>اجزای مقایسه‌ای</returns>
        protected abstract IEnumerable<object> GetEqualityComponents();

        /// <summary>
        /// عملگر برابر برای Value Object ها
        /// </summary>
        public static bool operator ==(ValueObjectBase left, ValueObjectBase right)
        {
            return Equals(left, right);
        }

        /// <summary>
        /// عملگر نابرابر برای Value Object ها
        /// </summary>
        public static bool operator !=(ValueObjectBase left, ValueObjectBase right)
        {
            return !Equals(left, right);
        }
    }
}
