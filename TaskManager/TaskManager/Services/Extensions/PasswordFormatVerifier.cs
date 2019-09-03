using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManager.Services.Extensions
{
    public static class PasswordFormatVerifier
    {
        public static bool HasCorrectFormat(this string password)
        {
            return password.Length > 8 &&                           // at least 8 characters
                   !password.ToLower().Equals(password) &&          // contains upper-case
                   !password.ToUpper().Equals(password) &&          // contains lower-case
                   !password.All(Char.IsLetterOrDigit) &&           // contains non-alphanumeric
                   password.Any(ch => "0123456789".Contains(ch));   // contains number
        }
    }
}
