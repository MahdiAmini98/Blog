using Blog.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Domain.Entities
{
    public class UserToken : EntityBase
    {
        public Guid UserId { get; private set; }
        public string AccessToken { get; private set; }//   توکن
        public string RefreshToken { get; private set; } // رفرش توکن
        public DateTime ExpirationTime { get; private set; } // زمان انقضای رفرش توکن
        public bool IsRevoked { get; private set; } // وضعیت لغو شدن توکن



        public User User { get; private set; }


        private UserToken() { }


        public UserToken(Guid userId, string accessToken, string refreshToken, DateTime expirationTime)
        {
            if (string.IsNullOrWhiteSpace(accessToken))
                throw new ArgumentException("Access token cannot be null or empty.", nameof(accessToken));


            if (string.IsNullOrWhiteSpace(refreshToken))
                throw new ArgumentException("Refresh token cannot be null or empty.", nameof(refreshToken));

            if (expirationTime <= DateTime.UtcNow)
                throw new ArgumentException("Expiration time must be in the future.", nameof(expirationTime));

            UserId = userId;
            AccessToken = accessToken;
            RefreshToken = refreshToken;
            ExpirationTime = expirationTime;
            IsRevoked = false;
        }

        // متد برای لغو کردن توکن
        public void Revoke()
        {
            if (IsRevoked)
                throw new InvalidOperationException("Token is already revoked.");

            IsRevoked = true; // لغو توکن
        }

        // متد برای بررسی معتبر بودن توکن
        public bool IsValid()
        {
            return !IsRevoked && ExpirationTime > DateTime.UtcNow;
        }

        // متد برای تمدید توکن
        public void ExtendExpiration(DateTime newExpirationTime)
        {
            if (newExpirationTime <= DateTime.UtcNow)
                throw new ArgumentException("New expiration time must be in the future.", nameof(newExpirationTime));

            ExpirationTime = newExpirationTime; // تنظیم زمان انقضای جدید
        }

        // متد برای اعتبارسنجی زمان انقضای توکن
        public void ValidateExpiration()
        {
            if (ExpirationTime <= DateTime.UtcNow)
                throw new InvalidOperationException("The token has expired.");
        }
    }
}
