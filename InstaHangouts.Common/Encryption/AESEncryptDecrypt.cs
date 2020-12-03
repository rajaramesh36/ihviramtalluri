
namespace InstaHangouts.Common.Encryption
{
    using System;
    using System.Configuration;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;

    /// <summary>
    /// Encrypt and decrypt password
    /// </summary>
    public class AESEncryptDecrypt
    {
        /// <summary>
        /// Encryption Key
        /// </summary>
        private static byte[] keybytes = Encoding.UTF8.GetBytes(ConfigurationManager.AppSettings.Get("AESKey"));

        /// <summary>
        /// Encryption Key
        /// </summary>
        private static byte[] iv = Encoding.UTF8.GetBytes(ConfigurationManager.AppSettings.Get("IVKey"));

        /// <summary>
        /// Decryption for java script string.
        /// </summary>
        /// <param name="plainText">The Plain Test</param>
        /// <returns>returns string</returns>
        public static string DecryptStringJS(string plainText)
        {
            if (string.IsNullOrEmpty(plainText))
            {
                return string.Empty;
            }

            byte[] data = Convert.FromBase64String(plainText);
            string cipherText = Encoding.UTF8.GetString(data);

            var encrypted = Convert.FromBase64String(cipherText);
            var decriptedFromJavascript = DecryptStringFromBytes(encrypted, keybytes, iv);

            return decriptedFromJavascript;
        }

        /// <summary>
        /// Decryption for java script string.
        /// </summary>
        /// <param name="cipherText">The cipher text</param>
        /// <param name="key">They key value</param>
        /// <param name="iv">the IV key value</param>
        /// <returns>Return string value</returns>
        private static string DecryptStringFromBytes(byte[] cipherText, byte[] key, byte[] iv)
        {
            //// Check arguments.  
            if (cipherText == null || cipherText.Length <= 0)
            {
                throw new ArgumentNullException("cipherText");
            }

            if (key == null || key.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }

            if (iv == null || iv.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }

            //// Declare the string used to hold  
            //// the decrypted text.  
            string plaintext = null;

            //// Create an RijndaelManaged object  
            //// with the specified key and IV.  
            using (var rijAlg = new RijndaelManaged())
            {
                ////Settings  
                rijAlg.Mode = CipherMode.CBC;
                rijAlg.Padding = PaddingMode.PKCS7;
                rijAlg.FeedbackSize = 128;

                rijAlg.Key = key;
                rijAlg.IV = iv;

                //// Create a decrytor to perform the stream transform.  
                var decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

                try
                {
                    //// Create the streams used for decryption.  
                    using (var msDecrypt = new MemoryStream(cipherText))
                    {
                        using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (var srDecrypt = new StreamReader(csDecrypt))
                            {
                                //// Read the decrypted bytes from the decrypting stream  
                                //// and place them in a string.  
                                plaintext = srDecrypt.ReadToEnd();
                            }
                        }
                    }
                }
                catch
                {
                    plaintext = "keyError";
                }
            }

            return plaintext;
        }      
    }   
}
