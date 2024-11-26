using System;
using System.Security.Cryptography;

namespace SexyDu.Crypto
{
    /// <summary>
    /// AES 암호화 기반 클래스
    /// </summary>
    public abstract class AesEncryptor : IDisposable
    {
        // 암호화 객체
        protected readonly Aes aes = null;

        public AesEncryptor()
        {
            aes = Aes.Create();
        }

        public AesEncryptor(byte[] key, byte[] iv) : this()
        {
            aes.Key = key;
            aes.IV = iv;
        }

        public virtual void Dispose()
        {
            aes.Dispose();
        }

        /// <summary>
        /// 키 설정
        /// </summary>
        /// <param name="base64">base64 문자 배열</param>
        public AesEncryptor SetKey(char[] base64)
        {
            aes.Key = Convert.FromBase64CharArray(base64, 0, base64.Length);
            return this;
        }

        /// <summary>
        /// IV 설정
        /// </summary>
        /// <param name="base64">base64 문자 배열</param>
        public AesEncryptor SetIv(char[] base64)
        {
            aes.IV = Convert.FromBase64CharArray(base64, 0, base64.Length);
            return this;
        }

        /// <summary>
        /// 암호화 모드 설정
        /// * 기본값 CipherMode.CBC
        /// </summary>
        /// <param name="mode">암호화 모드</param>
        public AesEncryptor SetMode(CipherMode mode)
        {
            aes.Mode = mode;
            return this;
        }
        /// <summary>
        /// 패딩 설정
        /// * 기본값 PaddingMode.PKCS7
        /// </summary>
        /// <param name="padding">패딩 모드</param>
        public AesEncryptor SetPadding(PaddingMode padding)
        {
            aes.Padding = padding;
            return this;
        }

        /// <summary>
        /// 블록 사이즈 설정
        /// * 기본값 128
        /// </summary>
        /// <param name="blockSize">블록 사이즈</param>
        public AesEncryptor SetBlockSize(int blockSize)
        {
            aes.BlockSize = blockSize;
            return this;
        }

        /// <summary>
        /// 키 사이즈 설정
        /// * 기본값 256
        /// </summary>
        /// <param name="keySize">키 사이즈</param>
        public AesEncryptor SetKeySize(int keySize)
        {
            aes.KeySize = keySize;
            return this;
        }
    }
}