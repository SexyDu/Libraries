using System;
using System.Security.Cryptography;
using System.Text;

namespace SexyDu.Crypto
{
    /// <summary>
    /// SHA-256 암호화
    /// </summary>
    public class SHA256Encryptor : IEncryptString
    {
        public void Dispose() { }

        /// <summary>
        /// 문자열 암호화
        /// </summary>
        /// <param name="plainText">암호화할 문자열</param>
        /// <returns>암호화된 문자열</returns>
        public string Encrypt(string plainText)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(plainText);
            return ToHashString(Encrypt(bytes));
        }

        /// <summary>
        /// 문자열 암호화
        /// </summary>
        /// <param name="plainText">암호화할 문자열</param>
        /// <param name="salt">Salt</param>
        /// <returns>암호화된 문자열</returns>
        public string Encrypt(string plainText, string salt)
        {
            byte[] plainBytes = Encoding.UTF8.GetBytes(plainText);
            byte[] saltBytes = Convert.FromBase64String(salt);

            byte[] combined = BufferTool.Combine(plainBytes, saltBytes);
            return ToHashString(Encrypt(combined));
        }

        /// <summary>
        /// 문자열 암호화
        /// </summary>
        /// <param name="plainText">암호화할 문자열</param>
        /// <param name="salt">Salt</param>
        /// <param name="iteration">반복 횟수</param>
        /// <returns>암호화된 문자열</returns>
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

        /// <summary>
        /// 문자열 암호화
        /// </summary>
        /// <param name="plainText">암호화할 문자열</param>
        /// <param name="salts">Salt</param>
        /// <returns>암호화된 문자열</returns>
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

        /// <summary>
        /// 해시값을 문자열로 변환
        /// </summary>
        /// <param name="hashBytes">해시값</param>
        /// <returns>해시값 문자열</returns>
        private string ToHashString(byte[] hashBytes)
        {
            // 해시값과 Salt를 함께 반환 (Salt를 저장할 수 있도록 설계)
            StringBuilder hashString = new StringBuilder();
            foreach (byte b in hashBytes)
                hashString.Append(b.ToString("x2"));

            return hashString.ToString();
        }

        /// <summary>
        /// 바이트 암호화
        /// </summary>
        /// <param name="bytes">암호화할 바이트</param>
        /// <returns>암호화된 바이트</returns>
        public byte[] Encrypt(byte[] bytes)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(bytes);
            }
        }
    }
}