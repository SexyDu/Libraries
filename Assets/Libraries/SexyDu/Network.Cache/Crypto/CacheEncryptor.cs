using System;
using System.Threading.Tasks;
using SexyDu.Crypto;

namespace SexyDu.Network.Cache
{
    . // 여기도 주석 달고 정리
    public interface ICacheEncryptor : IBytesEncryptor
    {
        char[] GetKey();
        char[] GetIv();
        char[] GetHmacKey();

        ICacheEncryptor UseDefaultHmac();
    }

    public class CacheEncryptor : ICacheEncryptor
    {
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

        public char[] GetHmacKey()
        {
            return key;
        }

        public char[] GetIv()
        {
            return iv;
        }

        public char[] GetKey()
        {
            return hmacKey;
        }

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