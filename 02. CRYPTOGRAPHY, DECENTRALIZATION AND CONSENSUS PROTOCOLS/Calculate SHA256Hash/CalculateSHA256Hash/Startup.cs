using System;
using System.Security.Cryptography;
using System.Text;

namespace CalculateSHA256Hash
{
    public class Startup
    {
        private static void Main()
        {
            string result = GetHashSha256("Hello_World");

            Console.WriteLine(result);
        }

        private static string GetHashSha256(string text)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(text);

            SHA256Managed manager = new SHA256Managed();

            byte[] hash = manager.ComputeHash(bytes);

            string hashString = null;

            foreach (byte b in hash)
            {
                hashString += String.Format("{0:x2}", b);
            }

            return hashString;
        }
    }
}
