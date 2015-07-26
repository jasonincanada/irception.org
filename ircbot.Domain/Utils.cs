using System;
using System.Security.Cryptography;
using System.Text;

namespace ircbot.Domain
{
    public static class Utils
    {
        public static string Get32ByteUID()
        {
            string guid = Guid.NewGuid().ToString();
            string salt = "irception";

            return CreateMD5(guid + salt);
        }

        /// <remarks>
        /// From: http://stackoverflow.com/questions/11454004/calculate-a-md5-hash-from-a-string
        /// </remarks>
        /// <summary>
        /// Return the 32-byte lowercase MD5 of the passed string
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            MD5 md5 = MD5.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            // Convert the byte array to hexadecimal string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("X2"));
            }
            return sb.ToString().ToLower();
        }

        /// <summary>
        /// Return SHA256 hash of string
        /// </summary>
        /// <remarks>
        /// From: http://stackoverflow.com/questions/12416249/hashing-a-string-with-sha256
        /// </remarks>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string GetSHA256(string text)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(text);
            SHA256Managed hashstring = new SHA256Managed();
            byte[] hash = hashstring.ComputeHash(bytes);
            string hashString = string.Empty;
            foreach (byte x in hash)
            {
                hashString += String.Format("{0:x2}", x);
            }
            return hashString;
        }
    }
}
