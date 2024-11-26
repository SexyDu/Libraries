using System;
using System.Security.Cryptography;
using System.Text;

namespace SexyDu.Crypto
{
    .// 이거 코드 정리하고 주석 달자
    /// <summary>
    /// SHA-256 암호화
    /// </summary>
    public class SHA256Encryptor : IBufferCombine
    {
        public string Encrypt(string plainText)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(plainText);
            return ToHashString(Encrypt(bytes));
        }

        public string Encrypt(string plainText, string salt)
        {
            byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
            byte[] saltBytes = Convert.FromBase64String(salt);

            byte[] combined = BufferTool.Combine(plainBytes, saltBytes);
            return ToHashString(Encrypt(combined));
        }

        public string Encrypt(string plainText, string salt, int iteration)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(plainText);
            byte[] saltBytes = Convert.FromBase64String(salt);
            for (int i = 0; i < iteration; i++)
            {
                bytes = BufferTool.Combine(bytes, saltBytes);
                bytes = Encrypt(bytes);
            }
            return ToHashString(bytes);
        }

        public string Encrypt(string plainText, params string[] salts)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(plainText);
            foreach (string salt in salts)
            {
                bytes = BufferTool.Combine(bytes, Convert.FromBase64String(salt));
                bytes = Encrypt(bytes);
            }
            return ToHashString(bytes);
        }

        private string ToHashString(byte[] hashBytes)
        {
            // 해시값과 Salt를 함께 반환 (Salt를 저장할 수 있도록 설계)
            StringBuilder hashString = new StringBuilder();
            foreach (byte b in hashBytes)
                hashString.Append(b.ToString("x2"));

            return hashString.ToString();
        }

        public byte[] Encrypt(byte[] bytes)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(bytes);
            }
        }
    }
}