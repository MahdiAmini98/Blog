using Blog.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Domain.ValueObjects
{
    public class Email : ValueObjectBase
    {
        public string Address { get; }

        public Email(string address)
        {
            if (string.IsNullOrWhiteSpace(address))
                throw new ArgumentException("Email address cannot be empty.", nameof(address));

            // اعتبارسنجی فرمت ایمیل (به صورت ساده)
            if (!address.Contains("@"))
                throw new ArgumentException("Invalid email address format.", nameof(address));

            Address = address;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            // فقط آدرس ایمیل برای مقایسه استفاده می‌شود
            yield return Address.ToLower();
        }

        public override string ToString()
        {
            return Address;
        }
    }
}
