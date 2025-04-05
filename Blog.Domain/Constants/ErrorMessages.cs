using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Domain.Constants
{
    public static class ErrorMessages
    {
        // User Errors
        public const string UserNotFound = "The specified user was not found.";
        public const string InvalidUsernameOrPassword = "Invalid username or password.";
        public const string EmailAlreadyInUse = "The email is already associated with another account.";

        // Post Errors
        public const string PostNotFound = "The specified post was not found.";
        public const string InvalidPostStatus = "The post status is invalid.";

        // Category Errors
        public const string CategoryAlreadyExists = "A category with the same name already exists.";

        // General Errors
        public const string InvalidInput = "The provided input is invalid.";
        public const string UnexpectedError = "An unexpected error occurred.";
    }
}
