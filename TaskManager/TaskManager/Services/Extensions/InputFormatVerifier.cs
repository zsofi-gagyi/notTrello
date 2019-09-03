using System;
using System.Linq;

namespace TaskManager.Services.Extensions
{
    public static class InputFormatVerifier
    {
        public static bool IsIncorrectPassword(this string password)
        {
            return  password.Length < 8 ||                 //IS FALSE: at least 8 characters
                    password.ToLower().Equals(password) ||          // contains upper-case letter
                    password.ToUpper().Equals(password) ||          // contains lower-case letter
                    password.All(char.IsLetterOrDigit) ||           // contains non-alphanumeric
                   !password.Any(char.IsDigit) ||                   // contains number
                    password.Any(char.IsWhiteSpace);                // contains no whitespace
        }

        public static bool IsIncorrectUserName(this string userName)
        {
            return  userName.Length < 3 ||                 //IS FALSE: at least 3 characters
                    userName.Length > 256 ||                        // not longer than the maximum allowed by EntityFramework
                    userName.Any(char.IsWhiteSpace);                // contains no whitespace
        }
    }
}
