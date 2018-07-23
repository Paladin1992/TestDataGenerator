using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TestDataGenerator.Common
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// Generates a hash and a salt for this string instance.
        /// </summary>
        /// <param name="password"></param>
        /// <param name="salt">The generated salt for the string.</param>
        /// <returns></returns>
        public static string HashPassword(this string password, out string salt)
        {
            // Generate the hash, with an automatic 32 byte salt
            var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, 32)
            {
                IterationCount = 10000
            };
            
            byte[] _hash = rfc2898DeriveBytes.GetBytes(20);
            byte[] _salt = rfc2898DeriveBytes.Salt;
            
            //Return the salt and the hash
            salt = Convert.ToBase64String(_salt);

            return Convert.ToBase64String(_hash);
        }

        public static string ToCamelCase(this string str)
        {
            if (string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str))
            {
                return "";
            }

            return str[0].ToString().ToLower() + str.Substring(1);
        }
    }
}
