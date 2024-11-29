using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace SexyDu.Crypto
{
    /// <summary>
    /// string AES 암호화 클래스
    /// </summary>
    public class AesString : AesEncryptor, IEncryptString, IDecryptString
    {
        public AesString() : base() { }

        public AesString(byte[] key, byte[] iv) : base(key, iv) { }

        public AesString(char[] key, char[] iv) : base(key, iv) { }

        /// <summary>
        /// 문자열 암호화
        /// </summary>
        /// <param name="plainText">암호화할 문자열</param>
        /// <returns>암호화된 문자열</returns>
        public string Encrypt(string plainText)
        {
#if true
            using (var ms = new MemoryStream())
            using (var encryptor = aes.CreateEncryptor())
            using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
            using (var writer = new StreamWriter(cs, Encoding.UTF8))
            {
                writer.Write(plainText);
                writer.Flush(); // 버퍼의 데이터를 스트림에 쓰기 (버퍼 비우기)
                cs.FlushFinalBlock(); // 암호화 스트림 마지막 블록 처리
                return Convert.ToBase64String(ms.ToArray());
            }
#else // 위에서 writer.Flush, cs.FlushFinalBlock은 Dispose 시에 자동 처리되므로 아래와 같이 작성해도 무방
            using (var ms = new MemoryStream())
            {
                using (var encryptor = aes.CreateEncryptor())
                using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                using (var writer = new StreamWriter(cs, Encoding.UTF8))
                {
                    writer.Write(plainText);
                }
                return Convert.ToBase64String(ms.ToArray());
            }
#endif
        }

        /// <summary>
        /// 문자열 복호화
        /// </summary>
        /// <param name="cipherText">복호화할 문자열</param>
        /// <returns>복호화된 문자열</returns>
        public string Decrypt(string cipherText)
        {
            using (var ms = new MemoryStream(Convert.FromBase64String(cipherText)))
            using (var decryptor = aes.CreateDecryptor())
            using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
            using (var reader = new StreamReader(cs, Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }
    }
}