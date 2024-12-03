using System;
using System.Threading.Tasks;
using SexyDu.Crypto;

namespace SexyDu.Network.Cache
{
    /// <summary>
    /// 캐시 암호화 인터페이스
    /// </summary>
    public interface ICacheEncryptor : IBytesEncryptor
    {
        /// <summary>
        /// 암호화 Key 반환
        /// </summary>
        /// <returns>암호화 Key</returns>
        char[] Key { get; }
        /// <summary>
        /// 암호화 IV 반환
        /// </summary>
        /// <returns>암호화 IV</returns>
        char[] Iv { get; }
        /// <summary>
        /// HMAC Key 반환
        /// </summary>
        /// <returns>HMAC Key</returns>
        char[] HmacKey { get; }

        /// <summary>
        /// 기본 HMAC Key 설정
        /// </summary>
        /// <returns>자기 자신</returns>
        ICacheEncryptor UseDefaultHmac();
    }

    /// <summary>
    /// 캐시 암호화 객체
    /// </summary>
    public class CacheEncryptor : ICacheEncryptor
    {
        // 암호화 객체
        private readonly IBytesEncryptor encryptor = null;

        private CacheEncryptor()
        {
            encryptor = new AesBytes(key, iv);
        }

        public CacheEncryptor(char[] key, char[] iv) : this()
        {
            this.key = key;
            this.iv = iv;
        }

        public CacheEncryptor(IBytesEncryptor encryptor)
        {
            this.encryptor = encryptor;
        }

        // 암호화 키
        public char[] Key => key;
        // 암호화 IV
        public char[] Iv => iv;
        // HMAC Key
        public char[] HmacKey => hmacKey;

        #region Proxy IBytesEncryptor
        public void Dispose()
        {
            encryptor.Dispose();
            hmacKey = null;
        }

        public bool UseHmac => hmacKey != null;

        public byte[] Decrypt(byte[] bytes)
        {
            return encryptor.Decrypt(bytes);
        }

        public Task<byte[]> DecryptAsync(byte[] bytes)
        {
            return encryptor.DecryptAsync(bytes);
        }

        public byte[] Encrypt(byte[] bytes)
        {
            return encryptor.Encrypt(bytes);
        }

        public Task<byte[]> EncryptAsync(byte[] bytes)
        {
            return encryptor.EncryptAsync(bytes);
        }

        public IBytesEncryptor SetHmac(char[] base64key)
        {
            hmacKey = base64key;
            encryptor.SetHmac(base64key);
            return this;
        }

        public IBytesEncryptor SetHmac(byte[] key)
        {
            throw new NotSupportedException("CacheEncryptor does not support SetHmac(byte[])");
        }
        #endregion

        public static CacheEncryptor Default => new CacheEncryptor();

        #region Default Encryption Value
        // 암호화 Key
        /// default : OJwI+0BGHADV4IQj8Nmzq7mdqbnaU/q8vh/S/ussZGU=
        private readonly char[] key = new char[44] { 'O', 'J', 'w', 'I', '+', '0', 'B', 'G', 'H', 'A', 'D', 'V', '4', 'I', 'Q', 'j', '8', 'N', 'm', 'z', 'q', '7', 'm',
             'd', 'q', 'b', 'n', 'a', 'U', '/', 'q', '8', 'v', 'h', '/', 'S', '/', 'u', 's', 's', 'Z', 'G', 'U', '=' };
        // 암호화 IV
        /// default : ioxyC/yzlwSBXs5uMtA==
        private readonly char[] iv = new char[24] { 'i', 'o', 'x', 'y', 'C', '/', 'y', 'z', 'l', 'w', 's', 'B', 'X', 's', '5', 'u', 'M', 't', 'a', '7', '4', 'A', '=', '=' };
        // 검증기 HMAC Key
        private char[] hmacKey = null;
        /// <summary>
        /// 기본 HMAC Key 설정
        /// : ICacheEncryptor
        /// </summary>
        public ICacheEncryptor UseDefaultHmac()
        {
            /// default : fYxJogT0FhHB/BEZYFMBfhqGiAIn90oaDvsxBdWPzO4=
            SetHmac(new char[44] { 'f', 'Y', 'x', 'J', 'o', 'g', 'T', '0', 'F', 'h', 'H', 'B', '/', 'B', 'E', 'Z', 'Y', 'F', 'M', 'B', 'f', 'h', 'q', 'G', 'i', 'A', 'I', 'n', '9',
                '0', 'o', 'a', 'D', 'v', 's', 'x', 'B', 'd', 'W', 'P', 'z', 'O', '4', '=' });
            return this;
        }
        #endregion
    }
}