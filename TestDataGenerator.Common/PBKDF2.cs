using System;
using System.Security.Cryptography;

namespace TestDataGenerator.Common
{
    /// <summary>
    /// A static class for PBKDF2 encryption methods.
    /// </summary>
    public static class PBKDF2
    {
        public const int SALT_BYTE_SIZE = 32;
        public const int HASH_BYTE_SIZE = 20; // to match the size of the PBKDF2-HMAC-SHA-1 hash 
        public const int PBKDF2_ITERATIONS = 10000;

        /// <summary>
        /// Hashes a password with PBKDF2 encryption and returns a 2-Tuple with the generated salt and hash (in this order).
        /// </summary>
        /// <param name="password">The plaintext password to encrypt.</param>
        /// <returns></returns>
        public static (string salt, string hash) HashPassword(string password)
        {
            var cryptoProvider = new RNGCryptoServiceProvider();
            byte[] salt = new byte[SALT_BYTE_SIZE];
            cryptoProvider.GetBytes(salt);

            var hash = GetPbkdf2Bytes(password, salt, PBKDF2_ITERATIONS, HASH_BYTE_SIZE);

            return (Convert.ToBase64String(salt), Convert.ToBase64String(hash));
        }

        /// <summary>
        /// Validates a password using the given stored password and salt hashes.
        /// </summary>
        /// <param name="password">The password we want to validate.</param>
        /// <param name="storedPasswordHash">The stored password hash we want to compare with.</param>
        /// <param name="storedSaltHash">The stored salt hash used to create the hash for comparison.</param>
        /// <returns></returns>
        public static bool ValidatePassword(string password, string storedPasswordHash, string storedSaltHash)
        {
            var hash = Convert.FromBase64String(storedPasswordHash);
            var salt = Convert.FromBase64String(storedSaltHash);

            var testHash = GetPbkdf2Bytes(password, salt, PBKDF2_ITERATIONS, hash.Length);
            return SlowEquals(hash, testHash);
        }

        /// <summary>
        /// Compares two byte arrays bit by bit (using XOR operation) and returns a boolean value
        /// indicating whether they are equal or not.
        /// </summary>
        /// <param name="a">First byte array.</param>
        /// <param name="b">Second byte array.</param>
        /// <returns></returns>
        private static bool SlowEquals(byte[] a, byte[] b)
        {
            var diff = (uint)a.Length ^ (uint)b.Length;
            for (int i = 0; i < a.Length && i < b.Length; i++)
            {
                diff |= (uint)(a[i] ^ b[i]);
            }
            return diff == 0;
        }

        /// <summary>
        /// Creates a PBKDF2 hashed password with the given salt, number of iterations, and size of output bytes.
        /// </summary>
        /// <param name="password">The string to be hashed.</param>
        /// <param name="salt">The salt to use for hashing.</param>
        /// <param name="iterations">The number of iterations of the hashing operation.</param>
        /// <param name="outputBytes">The size of the output byte array to store the generated hash.</param>
        /// <returns></returns>
        private static byte[] GetPbkdf2Bytes(string password, byte[] salt, int iterations, int outputBytes)
        {
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt)
            {
                IterationCount = iterations
            };

            return pbkdf2.GetBytes(outputBytes);
        }
    }
}
