using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;

namespace WindowsPet.Models
{
    /// <summary>
    /// Model To Check If UserInput Is Vaild
    /// </summary>
    /// <param name="email" name="name"></param>
    /// <returns></returns>
    internal static class VerifyInput
    {
        
        public static bool IsValidEmailFormat(string? email)
        {
            try
            {
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
        public static bool IsStrongPassword(string? password)
        {
            if (password.Length < 8)
                return false;

            bool hasUpper = password.Any(char.IsUpper);
            bool hasLower = password.Any(char.IsLower);
            bool hasDigit = password.Any(char.IsDigit);
            bool hasSymbol = password.Any(ch => !char.IsLetterOrDigit(ch));

            return hasUpper && hasLower && hasDigit && hasSymbol;
        }
        public static bool IsPasswordEqual(string? password, string? confirmPassword)
        {
            if(string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
                return false;
            return password == confirmPassword;
        }
    }
}
