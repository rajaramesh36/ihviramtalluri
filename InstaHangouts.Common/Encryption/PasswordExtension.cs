namespace InstaHangouts.Common.Encryption
{
    using System;
    using System.Configuration;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// Password Extension to hash password.
    /// </summary>
    public static class PasswordExtension
    {
        /// <summary>
        /// Password to hash password
        /// </summary>
        /// <param name="plainPassword">The plain password</param>
        /// <returns>the string value.</returns>
        public static string ToHashPassword(this string plainPassword)
        {
            if (string.IsNullOrEmpty(plainPassword))
            {
                return string.Empty;
            }
            
            //// step 1, calculate MD5 hash from input
            MD5 md5 = MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(plainPassword);
            byte[] hash = md5.ComputeHash(inputBytes);

            //// step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }

            return sb.ToString();
        }
    }
}
