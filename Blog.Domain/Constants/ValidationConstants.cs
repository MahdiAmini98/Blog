using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Domain.Constants
{
    public static class ValidationConstants
    {
        // User Validation
        public const int UsernameMinLength = 3;
        public const int UsernameMaxLength = 50;

        public const int PasswordMinLength = 8;
        public const int PasswordMaxLength = 100;

        // Post Validation
        public const int PostTitleMaxLength = 200;
        public const int PostContentMinLength = 10;

        // Category Validation
        public const int CategoryNameMaxLength = 100;

        // Tag Validation
        public const int TagNameMaxLength = 50;
    }
}
